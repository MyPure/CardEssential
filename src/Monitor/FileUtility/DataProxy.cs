using System;
using LitJson;

namespace CardEssential.Monitor.FileUtility;

public static class DataProxy<T> where T : BaseDataFile, new()
{
    private static T m_Proxy = new T();
    
    public static T Read()
    { 
        var path = m_Proxy.DataFilePath;

        var json = "";
        T data = null;
        if (!FileUtils.ExistFile(path) || string.IsNullOrEmpty(json = FileUtils.ReadFile(path)))
        {
            data = m_Proxy.Default as T;
            Save(data);
            
            EssentialMonitor.Instance.LogWarning($"Data file {m_Proxy.DataFilePath} not found. Create default.");
            return data;
        }

        try
        {
            data = JsonMapper.ToObject<T>(json);
        }
        catch (Exception e)
        {
            EssentialMonitor.Instance.LogWarning($"Data {m_Proxy.DataFilePath} deserialize error. Use default. {e.Message}");
            return m_Proxy.Default as T;
        }
        
        if (data.Valid)
        {
            return data;
        }
        else
        {
            EssentialMonitor.Instance.LogWarning($"Data {m_Proxy.DataFilePath} has invalid content. Use default.");
            return m_Proxy.Default as T;
        }
    }

    public static void Save(T data)
    {
        var path = m_Proxy.DataFilePath;
        var json = JsonMapper.ToJson(data, true);
        FileUtils.WriteFile(path, json);
    }
}