using System;
using CardEssential.Monitor.Stat.Data;

namespace CardEssential.Monitor.Config;

[Serializable]
public class ConfigFile
{
    public static ConfigFile Default()
    {
        return new ConfigFile();
    }
}