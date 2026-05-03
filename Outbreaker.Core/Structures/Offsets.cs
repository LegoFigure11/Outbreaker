namespace Outbreaker.Core.Structures;

public abstract class Offsets
{
    public const string ScarletID = "0100A3D008C5C000";
    public const string VioletID = "01008F6008C5E000";

    public static IReadOnlyList<long> BlockKeyPointer => [0x47350D8, 0xD8, 0x0, 0x0, 0x30, 0x0];

    public static uint BCATOutbreakEnabled => 0x61552076;

    public static IReadOnlyList<uint> MassOutbreakTotalKeys => [0x6C375C8A, 0xBD7C2A04, 0x19A98811];
    public static IReadOnlyList<uint> BCATMassOutbreakTotalKeys => [0x7478FD9A, 0x0D326604, 0x1B4ECAC3];

    public static IReadOnlyList<uint> OutbreakSpeciesBlockKeysPaldea => [0x76A2F996, 0x76A0BCF3, 0x76A97E38, 0x76A6E26D, 0x76986F3A, 0x76947F97, 0x769D40DC, 0x769B11D1];
    public static IReadOnlyList<uint> OutbreakSpeciesBlockKeysKitakami => [0x37E55F64, 0x37E33059, 0x37DFB442, 0x37DD779F];
    public static IReadOnlyList<uint> OutbreakSpeciesBlockKeysBlueberry => [0xB8E99C8D, 0xB8ED11D8, 0xB8E37713, 0xB8E766B6, 0xB8DEA571];

    public static IReadOnlyList<uint> BCATOutbreakSpeciesBlockKeysPaldea => [0x84AB44A6, 0x84A7C1C3, 0x84B15C88, 0x84AD7A7D, 0x849F074A, 0x849D3767, 0x84A58BEC, 0x84A2F021, 0x84C2791E, 0x84BFCFBB];
    public static IReadOnlyList<uint> BCATOutbreakSpeciesBlockKeysKitakami => [0x0F4D3B64, 0x0F4B0C59, 0x0F479042, 0x0F45539F, 0x0F5978C0, 0x0F560375, 0x0F53CD9E, 0x0F51243B, 0x0F36E06C, 0x0F3444A1];
    public static IReadOnlyList<uint> BCATOutbreakSpeciesBlockKeysBlueberry => [0x03B50A2B, 0x03B8F9CE, 0x03BAC2E5, 0x03BEA4F0, 0x03AA7FCF, 0x03AC4FB2, 0x03B03889, 0x03B26794, 0x039DD5B3, 0x03A1C556];
}

