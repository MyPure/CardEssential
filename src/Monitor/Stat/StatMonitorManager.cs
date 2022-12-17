using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CardEssential.Injector;
using CardEssential.Injector.Stat;
using CardEssential.Monitor.FileUtility;
using CardEssential.Monitor.Stat.Data;
using CardEssential.Monitor.Utils;
using UnityEngine;
using UniverseLib.Input;
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
    
    public static StatFilter StatFilter { get; private set; }
    
    public static StatTabDataFile SavedData { get; private set; }

    public static UIBase UiBase { get; set; }
    public static StatPanel StatPanel { get; set; }

    public static Dictionary<string, (float realValue, float baseValue)> LockedStatValue { get; set; } = new();
    public static Dictionary<string, (float realRate, float baseRate)> LockedStatRate { get; set; } = new();

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

    public static void SaveData()
    {
        DataProxy<StatTabDataFile>.Save(SavedData);
    }

    public static void ToggleDisplay()
    {
        ShowUI = !ShowUI;
        StatPanel.SetActive(ShowUI);
    }

    private static void OnInitialized() 
    {
        UiBase = UniversalUI.RegisterUI(GUID, UiUpdate);

        SavedData = DataProxy<StatTabDataFile>.Read();
        StatFilter = StatFilter.Create(SavedData);
        
        var statPanel = new StatPanel(UiBase);
        StatPanel = statPanel;

        ShowUI = false;
        EssentialInjector.Instance.StatInjector.OnStatCollected += OnStatCollectedHandler;
        //EssentialInjector.Instance.StatInjector.OnStatListTabCollected += OnStatListTabCollectedHandler;
        EssentialInjector.Instance.StatInjector.OnQuitGame += OnQuitGameHandler;
    }

    private static void Reload()
    {
        SavedData = DataProxy<StatTabDataFile>.Read();
        StatFilter = StatFilter.Create(SavedData);
        
        StatPanel.Destroy();
        var statPanel = new StatPanel(UiBase);
        StatPanel = statPanel;

        if (StatPacks != null)
        {
            StatPanel.SetData(StatPacks);
        }
    }

    public static bool IsLockValue(StatPack statPack)
    {
        return LockedStatValue.ContainsKey(statPack.DefaultName);
    }
    
    public static bool IsLockRate(StatPack statPack)
    {
        return LockedStatRate.ContainsKey(statPack.DefaultName);
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

    public static void UpdateStatStatuses(StatPack statPack, float prevValue)
    {
        var routine = (IEnumerator)ReflectionUtils.ExecuteMethod<GameManager>("UpdateStatStatuses",
            BindingFlags.NonPublic | BindingFlags.Instance, EssentialInjector.GM,
            new object[] { statPack.Stat, prevValue, null });
        EssentialInjector.GM.StartCoroutineEx(routine, out _);

        StatPanel.Refresh();
    }

    private static void LogHandler(string message, LogType type)
    {
        EssentialMonitor.Instance.LogInfo(message);
    }

    private static void UiUpdate()
    {
        if (InputManager.GetKey(KeyCode.LeftControl) && InputManager.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }
}