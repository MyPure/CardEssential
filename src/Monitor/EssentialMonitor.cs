﻿using BepInEx;
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
}
