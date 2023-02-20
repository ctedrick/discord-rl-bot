using Newtonsoft.Json;

namespace CodyTedrick.DiscordBot;

public struct ConfigJson
{
    [JsonProperty("token")]
    public string Token{ get; private set; }
    
    [JsonProperty("TestGuildId")]
    public ulong TestGuildId{ get; private set; }
}