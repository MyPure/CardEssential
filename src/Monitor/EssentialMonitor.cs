using BepInEx;
using CardEssential.Monitor.Stat;
using HarmonyLib;
using UnityEngine;
using UniverseLib.Input;

namespace CardEssential.Monitor;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class EssentialMonitor : BaseUnityPlugin
{
    public static EssentialMonitor Instance { get; set; }

    private Harmony m_Harmony;

    private void Awake()
    {
        Instance = this;
        StatMonitorManager.Init();
        LitJsonExtension.RegisterUnityObject();
        
        m_Harmony = new Harmony("com.mypure.cardessential.monitor");
        m_Harmony.PatchAll(typeof(StatPatch));
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
