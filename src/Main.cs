using LiteLoader.Hook;
using LiteLoader.NET;
using System.Runtime.InteropServices;

namespace Ptrarc;
[PluginMain("RandomSeed")]
public class RandomSeed : IPluginInitializer
{
    public string Introduction => "随机种子";
    public Dictionary<string, string> MetaData => new();
    public Version Version => new();
    public void OnInitialize() => Thook.RegisterHook<RandomSeedHook, RandomSeedHookDelegate>();
}

internal delegate void RandomSeedHookDelegate(nint a1, nint a2);
[HookSymbol("?write@StartGamePacket@@UEBAXAEAVBinaryStream@@@Z")]
internal class RandomSeedHook : THookBase<RandomSeedHookDelegate>
{
    public override RandomSeedHookDelegate Hook => (a1, a2) =>
    {
        Marshal.WriteInt64(a1, 48, Random.Shared.NextInt64());
        Original(a1, a2);
    };
}
