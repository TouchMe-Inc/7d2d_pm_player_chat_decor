using System.IO;
using PluginManager.Api;
using PluginManager.Api.Capabilities.Implementations.Events.GameEvents;
using PluginManager.Api.Contracts;
using PluginManager.Api.Hooks;

namespace PlayerChatDecor;

public class PlayerChatDecor : BasePlugin
{
    public override string ModuleName => "PlayerChatDecor";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "TouchMe-Inc";
    public override string ModuleDescription => "The plugin allows you to add tags to players";

    private IPlayerConfigProvider _configProvider;

    protected override void OnLoad()
    {
        var iniPath = Path.Combine(ModulePath, "players.ini");
        _configProvider = new IniPlayerConfigProvider(iniPath);

        Log.Out($"[{ModuleName}] Loaded {_configProvider.Count} player with chat overrides");

        RegisterEventHandler<ChatMessageEvent>(OnChatMessage, HookMode.Pre);
    }

    private HookResult OnChatMessage(ChatMessageEvent evt)
    {
        if (!_configProvider.TryGetPlayer(evt.ClientInfo.CrossplatformId, out var info)) return HookResult.Continue;

        var nickColor = !string.IsNullOrEmpty(info.NickColorHex) ? $"[{info.NickColorHex}]" : "";
        var messageColor = !string.IsNullOrEmpty(info.MessageColorHex) ? $"[{info.MessageColorHex}]" : "";

        evt.Name = !string.IsNullOrEmpty(info.Tag) ? $"{info.Tag} {nickColor}{evt.Name}" : $"{nickColor}{evt.Name}";
        evt.Message = $"{messageColor}{evt.Message}";
        evt.BBMode = BbCodeSupportMode.Supported;

        return HookResult.Changed;
    }
}