using System;
using System.Collections.Generic;
using BepInEx;
using CardEssential.Injector.Stat;
using HarmonyLib;
using UnityEngine;

namespace CardEssential.Injector;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class EssentialInjector : BaseUnityPlugin
{
    public static EssentialInjector Instance { get; set; }
    public static GameManager GM => MBSingleton<GameManager>.Instance;

    public StatInjector StatInjector { get; set; }

    private Harmony m_Harmony;

    private void Awake()
    {
        Instance = this;
        
        m_Harmony = new Harmony("com.mypure.cardessential.injector");

        SetInjector();
    }

    private void SetInjector()
    {
        m_Harmony.PatchAll(typeof(StatInjector));
        StatInjector = new StatInjector();
    }

    public void LogInfo(string msg)
    {
        Logger.LogInfo(msg);
    }
}