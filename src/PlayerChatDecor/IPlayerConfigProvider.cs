namespace PlayerChatDecor;

public interface IPlayerConfigProvider
{
    bool TryGetPlayer(string id, out PlayerInfo info);
    int Count { get; }
}
