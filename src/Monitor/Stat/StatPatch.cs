using HarmonyLib;

namespace CardEssential.Monitor.Stat;

public static class StatPatch
{
    [HarmonyPostfix, HarmonyPatch(typeof(InGameStat), "CurrentValue")]
    public static void InGameStatCurrentValuePostfix(InGameStat __instance, ref float __result)
    {
        if (StatMonitorManager.LockedStatValue.ContainsKey(__instance.StatModel.GameName.DefaultText))
        {
            __result = StatMonitorManager.LockedStatValue[__instance.StatModel.GameName.DefaultText].realValue;
            __instance.CurrentBaseValue =
                StatMonitorManager.LockedStatValue[__instance.StatModel.GameName.DefaultText].baseValue;
        }
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(InGameStat), "CurrentRatePerTick")]
    public static void InGameStatCurrentRatePostfix(InGameStat __instance, ref float __result)
    {
        if (StatMonitorManager.LockedStatRate.ContainsKey(__instance.StatModel.GameName.DefaultText))
        {
            __result = StatMonitorManager.LockedStatRate[__instance.StatModel.GameName.DefaultText].realRate;
            __instance.CurrentBaseRate = StatMonitorManager.LockedStatRate[__instance.StatModel.GameName.DefaultText].baseRate;
        }
    }
    
    [HarmonyPostfix, HarmonyPatch(typeof(GameManager), "ApplyRates")]
    public static void ApplyRatesPostfix()
    {
        StatMonitorManager.StatPanel.Refresh();
    }
}