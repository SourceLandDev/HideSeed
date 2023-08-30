using LiteLoader.Event;
using LiteLoader.Hook;
using LiteLoader.NET;
using System.Runtime.InteropServices;

namespace RandomSeed;
[PluginMain("RandomSeed")]
public sealed class Main : IPluginInitializer
{
    public string Introduction => "随机种子";
    public Dictionary<string, string> MetaData => new();
    internal static long SeedCache { get; private set; }
    public void OnInitialize()
    {
        PlayerPreJoinEvent.Event += e =>
        {
            SeedCache = new Random(e.Player.ClientId.GetHashCode()).NextInt64();
            return true;
        };
        Thook.RegisterHook<RandomSeedHook, RandomSeedHookDelegate>();
    }
}

internal delegate void RandomSeedHookDelegate(nint a1, nint a2);
[HookSymbol("?write@StartGamePacket@@UEBAXAEAVBinaryStream@@@Z")]
internal class RandomSeedHook : THookBase<RandomSeedHookDelegate>
{
    public override RandomSeedHookDelegate Hook => (a1, a2) =>
    {
        Marshal.WriteInt64(a1, 48, Main.SeedCache);
        Original(a1, a2);
    };
}
