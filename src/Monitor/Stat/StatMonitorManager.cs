using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CardEssential.Injector;
using CardEssential.Injector.Stat;
using CardEssential.Monitor.Utils;
using UnityEngine;
using UniverseLib.UI;

namespace CardEssential.Monitor.Stat;

public static class StatMonitorManager
{
    public const string GUID = "com.mypure.essential.monitor.stat";

    private static bool ShowUI
    {
        get => UiBase != null && UiBase.Enabled;
        set
        {
            UiBase.Enabled = value;
        }
    }
    public static UIBase UiBase { get; set; }
    private static StatPanel StatPanel { get; set; }

    private static List<StatPack> StatPacks { get; set; }
    
    public static void Init()
    {
        float startupDelay = 1f;
        UniverseLib.Config.UniverseLibConfig config = new()
        {
            Disable_EventSystem_Override = false,
            Force_Unlock_Mouse = true
        };

        UniverseLib.Universe.Init(startupDelay, OnInitialized, LogHandler, config);
    }

    public static void ToggleDisplay()
    {
        ShowUI = !ShowUI;
        StatPanel.SetActive(ShowUI);
    }

    private static void OnInitialized() 
    {
        UiBase = UniversalUI.RegisterUI(GUID, UiUpdate);
        var statPanel = new StatPanel(UiBase);
        StatPanel = statPanel;

        ShowUI = false;
        EssentialInjector.Instance.StatInjector.OnStatCollected += OnStatCollectedHandler;
        //EssentialInjector.Instance.StatInjector.OnStatListTabCollected += OnStatListTabCollectedHandler;
        EssentialInjector.Instance.StatInjector.OnQuitGame += OnQuitGameHandler;
    }

    private static void OnStatCollectedHandler(List<StatPack> statPacks)
    {
        if (statPacks != null)
        {
            StatPacks = statPacks;
            StatPanel.SetData(StatPacks);
        }
    }
    
    /*private static StatListTab[] StatListTabs { get; set; }

    public static void OnStatListTabCollectedHandler(StatListTab[] statListTabs)
    {
        StatListTabs = statListTabs;
        StreamWriter sw = new StreamWriter("StatListTabs.txt");
        foreach (var statListTab in statListTabs)
        {
            sw.WriteLine(statListTab.TabName);
            foreach (var stat in statListTab.ContainedStats)
            {
                sw.Write($"\"{stat.GameName}\"" + ", ");
            }

            sw.Write("\n");
        }
        sw.Flush();
        sw.Close();
    }*/

    public static void OnQuitGameHandler()
    {
        StatPanel.Clear();
        StatPacks = null;
    }
    
    public static void ChangeStatValue(StatPack statPack, float changeValue)
    {
        var routine = (IEnumerator)ReflectionUtils.ExecuteMethod<GameManager>("ChangeStatValue",
            BindingFlags.NonPublic | BindingFlags.Instance, EssentialInjector.GM,
            new object[] { statPack.Stat, changeValue, StatModification.Permanent });
        EssentialInjector.GM.StartCoroutineEx(routine, out _);
        
        StatPanel.Refresh();
    }
    
    public static void ChangeStatRate(StatPack statPack, float changeValue)
    {
        var routine = (IEnumerator)ReflectionUtils.ExecuteMethod<GameManager>("ChangeStatRate",
            BindingFlags.NonPublic | BindingFlags.Instance, EssentialInjector.GM,
            new object[] { statPack.Stat, changeValue, StatModification.Permanent });
        EssentialInjector.GM.StartCoroutineEx(routine, out _);
        
        StatPanel.Refresh();
    }

    private static void LogHandler(string message, LogType type)
    {
        EssentialMonitor.Instance.LogInfo(message);
    }

    private static void UiUpdate()
    {
        
    }
}