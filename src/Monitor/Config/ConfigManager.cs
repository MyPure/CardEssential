using CardEssential.Monitor.FileUtility;
using LitJson;

namespace CardEssential.Monitor.Config;

public static class ConfigManager
{
    private static ConfigFile m_Config;
    public static ConfigFile Config
    {
        get
        {
            if (m_Config != null)
            {
                return m_Config;
            }
            
            if (!ConfigExist)
            {
                CreateNewConfig();
            }
            
            var json = FileUtils.ReadFile(ConfigConst.ConfigFilePath);
            m_Config = JsonMapper.ToObject<ConfigFile>(json);
            
            return m_Config;
        }
    }

    public static bool ConfigExist => FileUtils.ExistFile(ConfigConst.ConfigFilePath);

    public static ConfigFile CreateNewConfig()
    {
        ConfigFile configFile = ConfigFile.Default();
        var json = JsonMapper.ToJson(configFile, true);

        FileUtils.WriteFile(ConfigConst.ConfigFilePath, json);

        return configFile;
    }
}