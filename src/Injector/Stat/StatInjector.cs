using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;

namespace CardEssential.Injector.Stat;

public class StatInjector
{
    public List<StatPack> StatPacks { get; set; }

    public event Action<List<StatPack>> OnStatCollected;
    public event Action OnQuitGame;
    public event Action<StatListTab[]> OnStatListTabCollected;

    private static StatInjector Instance => EssentialInjector.Instance.StatInjector;
    
    internal StatInjector()
    {
        StatPacks = new List<StatPack>();
    }
    
    private void OnGetStats(GameManager gm)
    {
        StatPacks.Clear();
        if (gm)
        {
            foreach (var stat in gm.AllStats)
            {
                StatPacks.Add(new StatPack(stat));
            }
        }
        OnStatCollected?.Invoke(StatPacks);
    }

    private void OnGetStatTabs(StatListTab[] statListTabs)
    {
        if (statListTabs.Length > 0)
        {
            OnStatListTabCollected?.Invoke(statListTabs);
        }
    }

    private void Release()
    {
        StatPacks.Clear();
        OnQuitGame?.Invoke();
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "InitializeStatsAndActions")]
    private static void GameManagerInitializeStatPatch(GameManager __instance)
    {
        Instance.OnGetStats(__instance);
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "QuitGame")]
    private static void GameManagerQuitGamePatch()
    {
        Instance.Release();
    }

    [HarmonyPostfix, HarmonyPatch(typeof(DetailedStatList), "Awake")]
    private static void DetailedStatListAwakePatch(DetailedStatList __instance)
    {
        FieldInfo tabsField = typeof(DetailedStatList).GetField("Tabs", BindingFlags.NonPublic | BindingFlags.Instance);
        if (tabsField != null)
        {
            if (tabsField.GetValue(__instance) is StatListTab[] tabs)
            {
                Instance.OnGetStatTabs(tabs);
            }
        }
    }

    [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "ActionRoutine")]
    private static void ActionRoutinePatch(CardAction _Action)
    {
        EssentialInjector.Instance.LogInfo("Action " + _Action.ActionName + " " + _Action.ActionDescription);
    }
}