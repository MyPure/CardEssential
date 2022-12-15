using System.IO;

namespace CardEssential.Monitor.FileUtility;

public static class FileUtils
{
    public const string PLUGIN_FILE_FOLDER_PATH = "BepInEx/plugins/CardEssential";

    public static string ReadFile(string relativeFilePath)
    {
        var path = GetRealPath(relativeFilePath);

        if (!File.Exists(path))
        {
            return null;
        }
        
        using StreamReader sr = new StreamReader(path);
        return sr.ReadToEnd();
    }
    
    public static void WriteFile(string relativeFilePath, string content)
    {
        if (!Directory.Exists(PLUGIN_FILE_FOLDER_PATH))
        {
            Directory.CreateDirectory(PLUGIN_FILE_FOLDER_PATH);
        }
        
        var path = GetRealPath(relativeFilePath);

        using StreamWriter sw = new StreamWriter(path);
        sw.Write(content);
    }

    public static bool ExistFile(string relativeFilePath)
    {
        return File.Exists(GetRealPath(relativeFilePath));
    }

    private static string GetRealPath(string relativeFilePath)
    {
        var path = Path.Combine(PLUGIN_FILE_FOLDER_PATH, relativeFilePath);
        return path;
    }
}