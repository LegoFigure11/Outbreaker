using PKHeX.Core;
using SysBot.Base;
using System.Diagnostics;
using System.Text.Json;
using Outbreaker.Core.Connection;
using static Outbreaker.WinForms.WinFormsUtil;

namespace Outbreaker.WinForms;

public partial class MainWindow : Form
{
    private static CancellationTokenSource Source = new();
    private static readonly Lock _connectLock = new();

    public ClientConfig Config;
    private ConnectionWrapperAsync ConnectionWrapper = default!;
    private readonly SwitchConnectionConfig ConnectionConfig;

    public readonly GameStrings Strings = GameInfo.GetStrings("en");

    private readonly Version CurrentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version!;

    public MainWindow()
    {
        Config = new ClientConfig();
        var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        if (File.Exists(configPath))
        {
            var text = File.ReadAllText(configPath);
            Config = JsonSerializer.Deserialize<ClientConfig>(text)!;
        }
        else
        {
            Config = new();
        }

        ConnectionConfig = new()
        {
            IP = Config.IP,
            Port = Config.Protocol is SwitchProtocol.WiFi ? 6000 : Config.UsbPort,
            Protocol = Config.Protocol,
        };

        var v = CurrentVersion;
#if DEBUG
        var build = "";

        var asm = System.Reflection.Assembly.GetEntryAssembly();
        var gitVersionInformationType = asm?.GetType("GitVersionInformation");
        var sha = gitVersionInformationType?.GetField("ShortSha");

        if (sha is not null) build += $"#{sha.GetValue(null)}";

        var date = File.GetLastWriteTime(AppContext.BaseDirectory);
        build += $" (dev-{date:yyyyMMdd})";

#else
        var build = "";
#endif

        Text = $"Outbreaker v{v.Major}.{v.Minor}.{v.Build}{build}";

        Application.SetColorMode(Config.Theme);

        InitializeComponent();
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
        CenterToScreen();

        if (Config.Protocol is SwitchProtocol.WiFi)
        {
            TB_SwitchIP.Text = Config.IP;
        }
        else
        {
            L_SwitchIP.Text = "USB Port:";
            TB_SwitchIP.Text = $"{Config.UsbPort}";
        }

        TB_Status.Text = "Not Connected.";

        CB_Theme.SelectedIndex = (int)Config.Theme;

        CB_Species.DataSource = Enum.GetValues<Species>()
            .Where(z => z != Species.MAX_COUNT)
            .ToArray();

        ConnectionWrapper = new(ConnectionConfig, UpdateStatus);

        CheckForUpdates();
    }

    private void Connect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                SetControlEnabledState(false, B_Connect);
                try
                {
                    ConnectionConfig.IP = TB_SwitchIP.Text;
                    (bool success, string err) = await ConnectionWrapper
                        .Connect(token)
                        .ConfigureAwait(false);
                    if (!success)
                    {
                        SetControlEnabledState(true, B_Connect);
                        this.DisplayMessageBox(err);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    SetControlEnabledState(true, B_Connect);
                    this.DisplayMessageBox(ex.Message);
                    return;
                }

                SetControlEnabledState(true, B_Disconnect);
                UpdateStatus("Connected!");
            },
            token
        );
    }

    private void Disconnect(CancellationToken token)
    {
        Task.Run(
            async () =>
            {
                SetControlEnabledState(false, B_Disconnect);
                try
                {
                    (bool success, string err) = await ConnectionWrapper.DisconnectAsync(token).ConfigureAwait(false);
                    if (!success) this.DisplayMessageBox(err);
                }
                catch (Exception ex)
                {
                    this.DisplayMessageBox(ex.Message);
                }

                await Source.CancelAsync().ConfigureAwait(false);
                Source = new();
                SetControlEnabledState(true, B_Connect);
            },
            token
        );
    }

    private void UpdateStatus(string status)
    {
        SetTextBoxText(status, TB_Status);
    }

    public void SetTextBoxText(string text, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is not TextBox tb)
                continue;

            if (InvokeRequired)
                Invoke(() => tb.Text = text);
            else
                tb.Text = text;
        }
    }

    public void SetControlEnabledState(bool state, params object[] obj)
    {
        foreach (object o in obj)
        {
            if (o is not Control c)
                continue;

            if (InvokeRequired)
                Invoke(() => c.Enabled = state);
            else
                c.Enabled = state;
        }
    }

    private void B_Connect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is { Connected: true })
                return;

            ConnectionWrapper = new(ConnectionConfig, UpdateStatus);
            Connect(Source.Token);
        }
    }

    private void B_Disconnect_Click(object sender, EventArgs e)
    {
        lock (_connectLock)
        {
            if (ConnectionWrapper is not { Connected: true })
                return;

            Disconnect(Source.Token);
        }
    }

    private void B_SearchOutbreaks_Click(object sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            try
            {
                if (ConnectionWrapper.Connected)
                {
                    bool found = false;
                    var time = await ConnectionWrapper.GetCurrentTime(Source.Token).ConfigureAwait(false);
                    while (!found)
                    {
                        await ConnectionWrapper.SaveGame(Source.Token).ConfigureAwait(false);

                        var target = CB_Species.GetText();

                        if (CB_Paldea.GetIsChecked())
                        {
                            UpdateStatus("Checking Paldea...");
                            var species = await ConnectionWrapper.GetOutbreaks(ConnectionWrapperAsync.MapArea.Paldea, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("P : " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (CB_PaldeaBCAT.GetIsChecked())
                        {
                            UpdateStatus("Checking Paldea BCAT...");
                            var species = await ConnectionWrapper.GetOutbreaksBCAT(ConnectionWrapperAsync.MapArea.Paldea, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("PB: " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (CB_Kitakami.GetIsChecked())
                        {
                            UpdateStatus("Checking Kitakami...");
                            var species = await ConnectionWrapper.GetOutbreaks(ConnectionWrapperAsync.MapArea.Kitakami, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("K : " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (CB_KitakamiBCAT.GetIsChecked())
                        {
                            UpdateStatus("Checking Kitakami BCAT...");
                            var species = await ConnectionWrapper.GetOutbreaksBCAT(ConnectionWrapperAsync.MapArea.Kitakami, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("KB: " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (CB_Blueberry.GetIsChecked())
                        {
                            UpdateStatus("Checking Blueberry...");
                            var species = await ConnectionWrapper.GetOutbreaks(ConnectionWrapperAsync.MapArea.Blueberry, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("B : " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (CB_BlueberryBCAT.GetIsChecked())
                        {
                            UpdateStatus("Checking Blueberry BCAT...");
                            var species = await ConnectionWrapper.GetOutbreaksBCAT(ConnectionWrapperAsync.MapArea.Blueberry, Source.Token).ConfigureAwait(false);
                            for (int i = 0; i < species.Count; i++)
                            {
                                var sn = SpeciesName.GetSpeciesName((ushort)species[i], 2);
                                Debug.WriteLine("BK: " + sn);
                                if (sn == target) found = true;
                            }
                        }

                        if (!found)
                        {
                            Debug.WriteLine("Resetting...");
                            await ConnectionWrapper.SetCurrentTime(time, Source.Token).ConfigureAwait(false);
                        }
                    }
                    UpdateStatus("Result found!");
                    this.DisplayMessageBox("Target outbreak found!", "Result found");
                }
            }
            catch (Exception ex)
            {
                this.DisplayMessageBox(ex.Message);
                return;
            }
        });
    }

    private void TB_SwitchIP_TextChanged(object sender, EventArgs e)
    {
        if (Config.Protocol is SwitchProtocol.WiFi)
        {
            Config.IP = TB_SwitchIP.Text;
            ConnectionConfig.IP = TB_SwitchIP.Text;
        }
        else
        {
            if (int.TryParse(TB_SwitchIP.Text, out int port) && port >= 0)
            {
                Config.UsbPort = port;
                ConnectionConfig.Port = port;
                return;
            }

            MessageBox.Show("Please enter a valid numerical USB port.");
        }
    }

    private readonly JsonSerializerOptions options = new() { WriteIndented = true };
    private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
    {
        var configpath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
        string output = JsonSerializer.Serialize(Config, options);
        using StreamWriter sw = new(configpath);
        sw.Write(output);

        if (ConnectionWrapper is { Connected: true })
        {
            try
            {
                _ = ConnectionWrapper.DisconnectAsync(Source.Token).ConfigureAwait(false);
            }
            catch
            {
                // ignored
            }
        }

        Source.Cancel();
        Source = new();
    }

    private bool first = true;
    private bool hasShownThemePopup = false;
    private void CB_Theme_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!first)
        {
            Config.Theme = (SystemColorMode)CB_Theme.SelectedIndex;
            if (!hasShownThemePopup) MessageBox.Show("Theme selection will be applied next time the program is launched.");
            hasShownThemePopup = true;
        }
        first = false;
    }

    private void CheckForUpdates()
    {
        Task.Run(async () =>
        {
            Version? latestVersion;
            try { latestVersion = GetLatestVersion(); }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception while checking for latest version: {ex}");
                return;
            }

            if (latestVersion is null || latestVersion <= CurrentVersion)
                return;

            while (!IsHandleCreated) // Wait for form to be ready
                await Task.Delay(2_000).ConfigureAwait(false);
            await InvokeAsync(() => NotifyNewVersionAvailable(latestVersion));
        });
    }

    private void NotifyNewVersionAvailable(Version version)
    {
        Text += $" - Update v{version.Major}.{version.Minor}.{version.Build} available!";

#if !DEBUG
        /*using UpdateNotifPopup nup = new(CurrentVersion, version);
        if (nup.ShowDialog() == DialogResult.OK)
        {
            Process.Start(new ProcessStartInfo("https://github.com/LegoFigure11/Outbreaker/releases/")
            {
                UseShellExecute = true
            });
        }*/
#endif
    }


    public static Version? GetLatestVersion()
    {
        const string endpoint = "https://api.github.com/repos/LegoFigure11/Outbreaker/releases/latest";
        var response = NetUtil.GetStringFromURL(new Uri(endpoint));
        if (response is null) return null;

        const string tag = "tag_name";
        var index = response.IndexOf(tag, StringComparison.Ordinal);
        if (index == -1) return null;

        var first = response.IndexOf('"', index + tag.Length + 1) + 1;
        if (first == 0) return null;

        var second = response.IndexOf('"', first);
        if (second == -1) return null;

        var tagString = response.AsSpan()[first..second].TrimStart('v');

        var patchIndex = tagString.IndexOf('-');
        if (patchIndex != -1) tagString = tagString.ToString().Remove(patchIndex).AsSpan();

        return !Version.TryParse(tagString, out var latestVersion) ? null : latestVersion;
    }
}
