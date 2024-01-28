using LiteLoader.Event;
using LiteLoader.Hook;
using LiteLoader.NET;
using System.Runtime.InteropServices;

namespace HiddenSeed;
[PluginMain("HiddenSeed")]
public sealed class Main : IPluginInitializer
{
    public string Introduction => "隐藏种子";
    public Dictionary<string, string> MetaData => new();
    public void OnInitialize()
    {
        Thook.RegisterHook<WriteStartGamePacketHook, WriteStartGamePacketHookDelegate>();
    }
}

internal delegate void WriteStartGamePacketHookDelegate(nint @this, nint a2);
[HookSymbol("?write@StartGamePacket@@UEBAXAEAVBinaryStream@@@Z")]
internal class WriteStartGamePacketHook : THookBase<WriteStartGamePacketHookDelegate>
{
    public override WriteStartGamePacketHookDelegate Hook => (@this, a2) =>
    {
        Marshal.WriteInt64(@this, 48, default);
        Original(@this, a2);
    };
}
