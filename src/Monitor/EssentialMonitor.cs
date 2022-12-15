using BepInEx;
using CardEssential.Monitor.Config;
using CardEssential.Monitor.Stat;
using UnityEngine;
using UniverseLib.Input;

namespace CardEssential.Monitor;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class EssentialMonitor : BaseUnityPlugin
{
    public static EssentialMonitor Instance { get; set; }

    private void Awake()
    {
        Instance = this;
        StatMonitorManager.Init();
        LitJsonExtension.RegisterUnityObject();
    }

    private void Update()
    {
        if (InputManager.GetKey(KeyCode.LeftControl) && InputManager.GetKeyDown(KeyCode.F))
        {
            StatMonitorManager.ToggleDisplay();
        }
    }

    public void LogInfo(string msg)
    {
        Logger.LogInfo(msg);
    }

    public void LogWarning(string msg)
    {
        Logger.LogWarning(msg);
    }
    
    public void LogError(string msg)
    {
        Logger.LogError(msg);
    }
}
