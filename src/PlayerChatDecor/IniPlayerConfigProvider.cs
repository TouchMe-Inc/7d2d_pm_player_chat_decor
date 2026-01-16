using System.Collections.Generic;
using System.IO;
using IniParser;
using IniParser.Model;

namespace PlayerChatDecor;

public class IniPlayerConfigProvider : IPlayerConfigProvider
{
    private readonly Dictionary<string, PlayerInfo> _players;

    public IniPlayerConfigProvider(string iniPath)
    {
        _players = LoadPlayers(iniPath);
    }

    private Dictionary<string, PlayerInfo> LoadPlayers(string iniPath)
    {
        var result = new Dictionary<string, PlayerInfo>();

        if (!File.Exists(iniPath))
        {
            return result;
        }

        var parser = new FileIniDataParser();
        IniData data = parser.ReadFile(iniPath);

        if (data == null || data.Sections == null)
        {
            return result;
        }

        foreach (var section in data.Sections)
        {
            var id = section.SectionName;

            result[id] = new PlayerInfo
            {
                NickColorHex = section.Keys.ContainsKey("NickColor") ? section.Keys["NickColor"] : string.Empty,
                MessageColorHex = section.Keys.ContainsKey("MessageColor") ? section.Keys["MessageColor"] : string.Empty,
                Tag = section.Keys.ContainsKey("Tag") ? section.Keys["Tag"] : string.Empty
            };
        }

        return result;
    }

    public bool TryGetPlayer(string id, out PlayerInfo info) => _players.TryGetValue(id, out info);

    public int Count => _players.Count;
}