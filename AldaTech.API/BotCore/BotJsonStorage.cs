using AldaTech_api.BotFactory;
using Newtonsoft.Json;

namespace AldaTech_api.BotCore;

public class BotJsonStorage
{
    // public BotManager BotManager { get; set; }

    public static BotManager ReadBotManager(string path)
    {
        using (var sr = new StreamReader(path))
        {
            using (var jsonReader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                var botManager = serializer.Deserialize<BotManager>(jsonReader);
                return botManager;
            }
        }
    }
    public static BotData ReadBotData(string path)
    {
        using (var sr = new StreamReader(path))
        {
            using (var jsonReader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                var botData = serializer.Deserialize<BotData>(jsonReader);
                return botData;
            }
        }
    }
    
    public static void WriteBotData(string path, BotData botData)
    {
        using (var sw = new StreamWriter(path))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                serializer.Serialize(jsonWriter, botData);
            }
        }
    }
    
    public static BotManager ReadBotManagerFromString(string str)
    {
        using (var sr = new StringReader(str))
        {
            using (var jsonReader = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                var botManager = serializer.Deserialize<BotManager>(jsonReader);
                return botManager;
            }
        }
    }
    
    public static void SaveBotManager(string path, BotManager botManager)
    {
        using (var sw = new StreamWriter(path))
        {
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = Formatting.Indented;
                var serializer = new JsonSerializer { TypeNameHandling = TypeNameHandling.Auto };
                serializer.Serialize(jsonWriter, botManager);
            }
        }
    }
    
}