using System;
using System.Linq;
using CardEssential.Injector.Stat;
using CardEssential.Monitor.Utils;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace CardEssential.Monitor.Stat;

public class StatCellDetail : UIModel
{
    public override GameObject UIRoot => m_UiRoot;
    
    private GameObject m_UiRoot;
    private StatCellDetailContent m_ValueContent;
    private StatCellDetailContent m_RateContent;
    
    private Action<string> m_ValueApplyButtonClickedHandler;
    private Action<string> m_RateApplyButtonClickedHandler;
    private Action m_ValueResetButtonClickedHandler;
    private Action m_RateResetButtonClickedHandler;

    public StatCellDetail(Action<string> valueApplyButtonClickedHandler, Action<string> rateApplyButtonClickedHandler, Action valueResetButtonClickedHandler, Action rateResetButtonClickedHandler)
    {
        m_ValueApplyButtonClickedHandler = valueApplyButtonClickedHandler;
        m_RateApplyButtonClickedHandler = rateApplyButtonClickedHandler;
        m_ValueResetButtonClickedHandler = valueResetButtonClickedHandler;
        m_RateResetButtonClickedHandler = rateResetButtonClickedHandler;
    }
    
    public override void ConstructUI(GameObject parent)
    {
        m_UiRoot = UIFactory.CreateUIObject("StatCellDetail", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(m_UiRoot, false, false, true, true, 5, 0, 0, 20, 0);

        m_ValueContent = new StatCellDetailContent(m_ValueApplyButtonClickedHandler, m_ValueResetButtonClickedHandler);
        m_ValueContent.ConstructUI(m_UiRoot);
        
        m_RateContent = new StatCellDetailContent(m_RateApplyButtonClickedHandler, m_RateResetButtonClickedHandler);
        m_RateContent.ConstructUI(m_UiRoot);
    }
    
    private void AppendLine(Text textUi, string text, int indent)
    {
        textUi.text += "\n";
        for (int i = 0; i < indent; ++i)
        {
            textUi.text += "  ";
        }
        textUi.text += text;
    }

    public void Refresh(StatPack statPack)
    {
        RefreshValue(statPack);
        RefreshRate(statPack);
    }

    private void RefreshValue(StatPack statPack)
    {
        var valueLabel = TextUtils.RichText("Value", ColorConst.SPECIAL_LABEL_HEAD);
        var valueText = TextUtils.RichText($"{statPack.Value.CurrentValue:0.##}", ColorConst.STAT_VALUE);
        var rateLabel = statPack.Rate.CurrentRatePerTick > 0 ? TextUtils.RichText("▲", ColorConst.VALUE_GROW) : statPack.Rate.CurrentRatePerTick < 0 ? TextUtils.RichText("▼", ColorConst.VALUE_DOWN) : TextUtils.RichText("●", ColorConst.VALUE_EVEN);
        var valueChange = statPack.Rate.CurrentRatePerTick >= 0
            ? TextUtils.RichText($"+{statPack.Rate.CurrentRatePerTick:0.##} / tick", ColorConst.VALUE_GROW)
            : TextUtils.RichText($"{statPack.Rate.CurrentRatePerTick:0.##} / tick", ColorConst.VALUE_DOWN);
        var topLabel = $"{valueLabel} {valueText} {rateLabel} {valueChange}";

        var baseLabel = TextUtils.RichText("Base", ColorConst.SPECIAL_LABEL);
        var baseValue = TextUtils.RichText($"{statPack.Value.CurrentBaseValue:0.##}", ColorConst.STAT_VALUE);
        string baseText = $"{baseLabel} {baseValue}";
        
        bool notAtBase = statPack.NotAtBase;
        var atBaseLabel = TextUtils.RichText("AtBase", ColorConst.SPECIAL_LABEL);
        var atBseValue = statPack.Value.AtBaseModifiedValue >= 0
            ? TextUtils.RichText($"+{statPack.Value.AtBaseModifiedValue:0.##}", ColorConst.VALUE_GROW)
            : TextUtils.RichText($"{statPack.Value.AtBaseModifiedValue:0.##}", ColorConst.VALUE_DOWN);
        string atBaseText = statPack.Value.AtBaseModifiedValue == 0 ? "" : $"{atBaseLabel} {atBseValue}";
        
        string modifiersName = TextUtils.RichText("Modifiers", ColorConst.SPECIAL_LABEL);
        string modifiersValue = statPack.Value.GlobalModifiedValue >= 0
            ? TextUtils.RichText($"+{statPack.Value.GlobalModifiedValue:0.##}", ColorConst.VALUE_GROW)
            : TextUtils.RichText($"{statPack.Value.GlobalModifiedValue:0.##}", ColorConst.VALUE_DOWN);
        string modifiersLabel = $"{modifiersName} {modifiersValue}";
        string modifiersContentText = "";

        for (int i = 0; i < statPack.ModifierSources.Count; ++i)
        {
            var modifier = statPack.ModifierSources[i];
            if (modifier.Value == 0)
            {
                continue;
            }
            var modifierName = TextUtils.RichText(GetModifierSourceName(modifier), ColorConst.NORMAL_LABEL);
            var modifierStatStatusName = GetModifierSourceStatusIfStat(modifier);
            var modifierStatStatusLabel =
                string.IsNullOrEmpty(modifierStatStatusName) ? "" : TextUtils.RichText($"({modifierStatStatusName})", ColorConst.NORMAL_LABEL);
            var modifierValue = modifier.Value >= 0
                ? TextUtils.RichText($"+{modifier.Value:0.##}", ColorConst.VALUE_GROW)
                : TextUtils.RichText($"{modifier.Value:0.##}", ColorConst.VALUE_DOWN);
            string modifierText = $"\t{modifierName}{modifierStatStatusLabel} {modifierValue}";
            
            modifiersContentText += modifierText;
            modifiersContentText += "  ";
        }
        
        m_ValueContent.Refresh(topLabel, baseText, notAtBase, atBaseText, modifiersLabel, modifiersContentText);
    }
    
    private void RefreshRate(StatPack statPack)
    {
        var rateLabel = TextUtils.RichText("Rate", ColorConst.SPECIAL_LABEL_HEAD);
        var rateText = TextUtils.RichText($"{statPack.Rate.CurrentRatePerTick:0.##}", ColorConst.STAT_VALUE);
        var topLabel = $"{rateLabel} {rateText}";

        var baseLabel = TextUtils.RichText("Base", ColorConst.SPECIAL_LABEL);
        var baseRate = TextUtils.RichText($"{statPack.Rate.CurrentBaseRate:0.##}", ColorConst.STAT_VALUE);
        string baseText = $"{baseLabel} {baseRate}";
        
        bool notAtBase = statPack.NotAtBase;
        var atBaseLabel = TextUtils.RichText("AtBase", ColorConst.SPECIAL_LABEL);
        var atBaseRate = statPack.Rate.AtBaseModifiedRate >= 0
            ? TextUtils.RichText($"+{statPack.Rate.AtBaseModifiedRate:0.##}", ColorConst.VALUE_GROW)
            : TextUtils.RichText($"{statPack.Rate.AtBaseModifiedRate:0.##}", ColorConst.VALUE_DOWN);
        string atBaseText = statPack.Rate.AtBaseModifiedRate == 0 ? "" : $"{atBaseLabel} {atBaseRate}";
        
        string modifiersName = TextUtils.RichText("Modifiers", ColorConst.SPECIAL_LABEL);
        string modifiersRate = statPack.Rate.GlobalModifiedRate >= 0
            ? TextUtils.RichText($"+{statPack.Rate.GlobalModifiedRate:0.##}", ColorConst.VALUE_GROW)
            : TextUtils.RichText($"{statPack.Rate.GlobalModifiedRate:0.##}", ColorConst.VALUE_DOWN);
        string modifiersLabel = $"{modifiersName} {modifiersRate}";
        string modifiersContentText = "";
        
        for (int i = 0; i < statPack.ModifierSources.Count; ++i)
        {
            var modifier = statPack.ModifierSources[i];
            if (modifier.Rate == 0)
            {
                continue;
            }
            var modifierName = TextUtils.RichText(GetModifierSourceName(modifier), ColorConst.NORMAL_LABEL);
            var modifierStatStatusName = GetModifierSourceStatusIfStat(modifier);
            var modifierStatStatusLabel =
                string.IsNullOrEmpty(modifierStatStatusName) ? "" : TextUtils.RichText($"({modifierStatStatusName})", ColorConst.NORMAL_LABEL);
            var modifierRate = modifier.Rate >= 0
                ? TextUtils.RichText($"+{modifier.Rate:0.##}", ColorConst.VALUE_GROW)
                : TextUtils.RichText($"{modifier.Rate:0.##}", ColorConst.VALUE_DOWN);
            string modifierText = $"\t{modifierName}{modifierStatStatusLabel} {modifierRate}";
            modifiersContentText += modifierText;
            modifiersContentText += "  ";
        }
        
        m_RateContent.Refresh(topLabel, baseText, notAtBase, atBaseText, modifiersLabel, modifiersContentText);
    }
    
    private string GetModifierSourceName(StatModifierSource modifierSource)
    {
        string sourceName = "";
        if (modifierSource.Stat != null)
        {
            sourceName = modifierSource.Stat.StatModel.GameName;
        }
        else if (modifierSource.Card != null)
        {
            sourceName = modifierSource.Card.CardModel.CardName;
        }
        else if (modifierSource.Character != null)
        {
            sourceName = modifierSource.Character.CharacterName;
        }
        else if (modifierSource.Perk != null)
        {
            sourceName = modifierSource.Perk.PerkName;
        }
        else if (modifierSource.TimeOfDay != null)
        {
            sourceName = $"Time {modifierSource.TimeOfDay.EffectStartingTime}~{modifierSource.TimeOfDay.EffectEndTime}";
        }
        return sourceName;
    }
    
    private string GetModifierSourceStatusIfStat(StatModifierSource modifierSource)
    {
        string sourceName = "";
        if (modifierSource.Stat != null)
        {
            var status = modifierSource.Stat.CurrentStatuses.FirstOrDefault();
            if (status != null)
            {
                sourceName = status.GameName;
            }
        }
        return sourceName;
    }
}