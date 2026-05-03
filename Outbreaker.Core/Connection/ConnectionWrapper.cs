using Outbreaker.Core.Structures;
using PKHeX.Core;
using SysBot.Base;
using System.Net.Sockets;
using System.Text;
using static SysBot.Base.SwitchButton;
using static SysBot.Base.SwitchCommand;
using static System.Buffers.Binary.BinaryPrimitives;

namespace Outbreaker.Core.Connection;

public class ConnectionWrapperAsync(SwitchConnectionConfig Config, Action<string> StatusUpdate) : Offsets
{
    public readonly ISwitchConnectionAsync Connection = Config.Protocol switch
    {
        SwitchProtocol.USB => new SwitchUSBAsync(Config.Port),
        _ => new SwitchSocketAsync(Config),
    };

    public bool Connected => IsConnected;
    private bool IsConnected { get; set; }
    private readonly bool CRLF = Config.Protocol is SwitchProtocol.WiFi;

    private readonly SAV9SV sav = new();

    private static ulong BaseBlockKeyPointer;
    private static bool BCATEnabled;

    public async Task<(bool, string)> Connect(CancellationToken token)
    {
        if (Connected) return (true, "");

        try
        {
            StatusUpdate("Connecting...");
            Connection.Connect();

            BaseBlockKeyPointer = await Connection
                .PointerAll(BlockKeyPointer, token)
                .ConfigureAwait(false);

            BCATEnabled = await ReadBool(BCATOutbreakEnabled, token).ConfigureAwait(false);

            _baseCountCache = [0, 0, 0];
            _bcatCountCache = [0, 0, 0];

            _baseCache = [
                [0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0],
                [0, 0, 0, 0, 0]
            ];

            _bcatCache = [
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
                [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
            ];

            IsConnected = true;
            StatusUpdate("Connected!");
            return (true, "");
        }
        catch (SocketException e)
        {
            IsConnected = false;
            return (false, e.Message);
        }
    }

    public async Task<(bool, string)> DisconnectAsync(CancellationToken token)
    {
        if (!Connected) return (true, "");

        try
        {
            StatusUpdate("Disconnecting controller");
            await Connection.SendAsync(DetachController(CRLF), token).ConfigureAwait(false);

            StatusUpdate("Disconnecting...");
            Connection.Disconnect();
            IsConnected = false;
            StatusUpdate("Disconnected!");
            return (true, "");
        }
        catch (SocketException e)
        {
            IsConnected = false;
            return (false, e.Message);
        }
    }

    public async Task<(byte, ulong)> ReadEncryptedBlockByte(uint key, ulong init, CancellationToken token)
    {
        var (header, address) = await ReadEncryptedBlockHeader(key, init, token).ConfigureAwait(false);
        return (header[1], address);
    }

    public async Task<(byte[], ulong)> ReadEncryptedBlockHeader(uint key, ulong init, CancellationToken token)
    {
        if (init == 0)
        {
            var address = await SearchSaveKey(key, token).ConfigureAwait(false);
            address = BitConverter.ToUInt64(await Connection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
            init = address;
        }

        var header = await Connection.ReadBytesAbsoluteAsync(init, 5, token).ConfigureAwait(false);
        header = DecryptBlock(key, header);

        return (header, init);
    }

    private async Task<ulong> SearchSaveKey(uint key, CancellationToken token)
    {
        var data = await Connection
            .ReadBytesAbsoluteAsync(BaseBlockKeyPointer + 8, 16, token)
            .ConfigureAwait(false);
        var start = BitConverter.ToUInt64(data.AsSpan()[..8]);
        var end = BitConverter.ToUInt64(data.AsSpan()[8..]);

        while (start < end)
        {
            var block_ct = (end - start) / 48;
            var mid = start + ((block_ct >> 1) * 48);

            data = await Connection.ReadBytesAbsoluteAsync(mid, 4, token).ConfigureAwait(false);
            var found = BitConverter.ToUInt32(data);
            if (found == key)
                return mid;

            if (found >= key)
                end = mid;
            else
                start = mid + 48;
        }
        return start;
    }

    private static byte[] DecryptBlock(uint key, byte[] block)
    {
        var rng = new SCXorShift32(key);
        for (int i = 0; i < block.Length; i++)
            block[i] = (byte)(block[i] ^ rng.Next());
        return block;
    }

    public enum MapArea
    {
        Paldea,
        Kitakami,
        Blueberry,
    };

    public async Task<bool> ReadBool(uint key, CancellationToken token)
    {
        var address = await SearchSaveKey(key, token).ConfigureAwait(false);
        address = BitConverter.ToUInt64(await Connection.ReadBytesAbsoluteAsync(address + 8, 0x8, token).ConfigureAwait(false), 0);
        var data = await Connection.ReadBytesAbsoluteAsync(address, 1, token).ConfigureAwait(false);
        var res = DecryptBlock(key, data);
        return res[0] == 2;
    }

    public async Task<(uint, ulong)> ReadEncryptedBlockUint(uint key, ulong init, CancellationToken token)
    {
        var (header, address) = await ReadEncryptedBlockHeader(key, init, token).ConfigureAwait(false);
        return (ReadUInt32LittleEndian(header.AsSpan()[1..]), address);
    }

    ulong[] _baseCountCache = [0, 0, 0];
    public async Task<byte> GetOutbreakCount(MapArea map, CancellationToken token)
    {
        var i = (int)map;
        var (ct, offs) = await ReadEncryptedBlockByte(MassOutbreakTotalKeys[i], _baseCountCache[i], token).ConfigureAwait(false);
        if (_baseCountCache[i] == 0) _baseCountCache[i] = offs;
        return ct;
    }

    ulong[] _bcatCountCache = [0, 0, 0];
    public async Task<byte> GetOutbreakCountBCAT(MapArea map, CancellationToken token)
    {
        var i = (int)map;
        var (ct, offs) = await ReadEncryptedBlockByte(BCATMassOutbreakTotalKeys[i], _bcatCountCache[i], token).ConfigureAwait(false);
        if (_bcatCountCache[i] == 0) _bcatCountCache[i] = offs;
        return ct;
    }

    private List<ulong>[] _baseCache = [
        [0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0],
        [0, 0, 0, 0, 0]
    ];
    public async Task<List<Species>> GetOutbreaks(MapArea map, CancellationToken token)
    {
        List<Species> s = [];

        var keys = map switch
        {
            MapArea.Paldea => OutbreakSpeciesBlockKeysPaldea,
            MapArea.Kitakami => OutbreakSpeciesBlockKeysKitakami,
            _ => OutbreakSpeciesBlockKeysBlueberry,
        };

        var ct = Math.Min(await GetOutbreakCount(map, token).ConfigureAwait(false), keys.Count);

        var mapidx = (int)map;
        for (var i = 0; i < ct; i++)
        {
            var key = keys[i];
            var (_s, offs) = await ReadEncryptedBlockUint(key, _baseCache[mapidx][i], token).ConfigureAwait(false);
            if (_baseCache[mapidx][i] == 0) _baseCache[mapidx][i] = offs;
            s.Add((Species)SpeciesConverter.GetNational9((ushort)_s));
        }

        return s;
    }

    private List<ulong>[] _bcatCache = [
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0],
        [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
    ];

    public async Task<List<Species>> GetOutbreaksBCAT(MapArea map, CancellationToken token)
    {
        List<Species> s = [];
        if (!BCATEnabled) return s;

        var keys = map switch
        {
            MapArea.Paldea => BCATOutbreakSpeciesBlockKeysPaldea,
            MapArea.Kitakami => BCATOutbreakSpeciesBlockKeysKitakami,
            _ => BCATOutbreakSpeciesBlockKeysBlueberry,
        };

        var ct = Math.Min(await GetOutbreakCountBCAT(map, token).ConfigureAwait(false), keys.Count);

        var mapidx = (int)map;
        for (var i = 0; i < ct; i++)
        {
            var key = keys[i];
            var (_s, offs) = await ReadEncryptedBlockUint(key, _bcatCache[mapidx][i], token).ConfigureAwait(false);
            if (_bcatCache[mapidx][i] == 0) _bcatCache[mapidx][i] = offs;
            s.Add((Species)SpeciesConverter.GetNational9((ushort)_s));
        }

        return s;
    }

    private async Task Click(SwitchButton button, int delay, CancellationToken token)
    {
        await Connection
            .SendAsync(SwitchCommand.Click(button, CRLF), token)
            .ConfigureAwait(false);
        await Task.Delay(delay, token).ConfigureAwait(false);
    }

    public async Task SaveGame(CancellationToken token)
    {
        StatusUpdate("Saving the game...");
        // B out in case we're in some menu.
        for (int i = 0; i < 4; i++)
            await Click(B, 0_500, token).ConfigureAwait(false);

        // Open the menu and save.
        await Click(X, 1_000, token).ConfigureAwait(false);
        await Click(R, 1_000, token).ConfigureAwait(false);
        await Click(A, 1_000, token).ConfigureAwait(false);
        await Click(A, 1_000, token).ConfigureAwait(false);
        await Click(A, 4_000, token).ConfigureAwait(false);

        // Return to overworld.
        for (int i = 0; i < 4; i++)
            await Click(B, 0_500, token).ConfigureAwait(false);
        StatusUpdate("Game saved!");
    }

    public async Task<ulong> GetCurrentTime(CancellationToken token)
    {
        var command = Encoding.ASCII.GetBytes($"getCurrentTime{(CRLF ? "\r\n" : "")}");
        var res = await Connection.ReadRaw(command, 17, token).ConfigureAwait(false);
        ulong.TryParse(Encoding.ASCII.GetString(res).Trim('\n'), System.Globalization.NumberStyles.AllowHexSpecifier, null, out var time);
        return time;
    }

    public async Task SetCurrentTime(ulong date, CancellationToken token)
    {
        var command = Encoding.ASCII.GetBytes($"setCurrentTime {date}{(CRLF ? "\r\n" : "")}");
        await Connection.SendAsync(command, token).ConfigureAwait(false);
    }
}
