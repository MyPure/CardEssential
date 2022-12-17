using System.Linq;
using CardEssential.Injector.Stat;
using CardEssential.Monitor.Utils;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Widgets.ScrollView;

namespace CardEssential.Monitor.Stat;

public class StatCell : ICell
{
    public bool Enabled => UIRoot.activeInHierarchy;
    public RectTransform Rect { get; set; }
    public GameObject UIRoot { get; set; }
    public float DefaultHeight { get => 20; }

    private StatPack m_StatPack;

    private bool m_Foldout = true;
    private ButtonRef m_FoldOutButton;

    private GameObject m_Like;
    private Toggle m_LikeToggle;
    
    private Text m_StatName;
    private Text m_StatValue;
    private Text m_StatStatus;

    private StatCellDetail m_StatCellDetail;
    
    private Color m_Value = new Color(0.11f, 0.11f, 0.11f);
    private Color m_Value2 = new Color(0.25f, 0.25f, 0.25f);
    private Color m_Value3 = new Color(0.05f, 0.05f, 0.05f);
    private Color m_Value4 = new Color(1f, 1f, 1f, 0f);

    public GameObject CreateContent(GameObject parent)
    {
        UIRoot = UIFactory.CreateUIObject("StatCell", parent, new Vector2(25, 25));
        Rect = UIRoot.GetComponent<RectTransform>();

        UIFactory.SetLayoutElement(UIRoot, minHeight: 25, flexibleWidth: 9999);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(UIRoot, false, false, true, true, 5, 2, 2, 2, 2,
            childAlignment: TextAnchor.UpperLeft);

        GameObject statBase = UIFactory.CreateUIObject("StatBase", UIRoot);
        UIFactory.SetLayoutElement(UIRoot, minHeight: 25, flexibleWidth: 9999);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(statBase, false, false, true, true, 5, 2, 2, 2, 2);

        m_FoldOutButton = UIFactory.CreateButton(statBase, "Foldout", "►");
        UIFactory.SetLayoutElement(m_FoldOutButton.GameObject, minHeight: 25, minWidth: 15, preferredWidth: 15);
        m_FoldOutButton.OnClick += OnFoldoutClicked;
        RuntimeHelper.SetColorBlock(m_FoldOutButton.Component, m_Value, m_Value2, m_Value3, m_Value4);
        SetButton();

        m_Like = UIFactory.CreateToggle(statBase, "Like", out m_LikeToggle, out var likeText);
        UIFactory.SetLayoutElement(m_Like, minHeight: 25, minWidth: 20);
        m_LikeToggle.onValueChanged.AddListener(HandleLickToggleValueChanged);
        likeText.text = "";

        m_StatName = UIFactory.CreateLabel(statBase, "StatName", "", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_StatName.gameObject, minHeight: 25, minWidth: 30);

        m_StatValue = UIFactory.CreateLabel(statBase, "StatValue", "", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_StatValue.gameObject, minHeight: 25, minWidth: 30);

        m_StatStatus = UIFactory.CreateLabel(statBase, "StatStatus", "", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_StatStatus.gameObject, minHeight: 25, minWidth: 30);

        // Foldout content
        m_StatCellDetail = new StatCellDetail(HandleValueApplyClick, HandleRateApplyClick, HandleValueResetClick,
            HandleRateResetClick, HandleValueLock, HandleRateLock);
        m_StatCellDetail.ConstructUI(UIRoot);
        m_StatCellDetail.SetActive(false);

        return UIRoot;
    }

    public void SetStat(StatPack statPack)
    {
        m_StatPack = statPack;
        m_StatName.text = TextUtils.RichText(statPack.Name, ColorConst.NORMAL_LABEL);

        var minMaxText = TextUtils.RichText($"[{statPack.Value.MinValue} - {statPack.Value.MaxValue}]", ColorConst.STAT_VALUE_MIN_MAX);
        var valueText = TextUtils.RichText($"{statPack.Value.CurrentValue:0.##}", ColorConst.STAT_VALUE);
        var rateLabel = statPack.Rate.CurrentRatePerTick > 0 ? TextUtils.RichText("▲", ColorConst.VALUE_GROW) : statPack.Rate.CurrentRatePerTick < 0 ? TextUtils.RichText("▼", ColorConst.VALUE_DOWN) : TextUtils.RichText("●", ColorConst.VALUE_EVEN);
        m_StatValue.text = $"{minMaxText} {valueText} {rateLabel}";

        var status = statPack.CurrentStatus;
        m_StatStatus.text = status == null ? "" : TextUtils.RichText($"({status.GameName}) {status.Description}", ColorConst.STAT_DESC);

        m_LikeToggle.isOn = StatMonitorManager.SavedData.favoriteStats.Contains(statPack.DefaultName);
        
        SetDetail();
    }

    private void HandleLickToggleValueChanged(bool value)
    {
        if (value)
        {
            StatMonitorManager.SavedData.favoriteStats.Add(m_StatPack.DefaultName);
        }
        else
        {
            StatMonitorManager.SavedData.favoriteStats.Remove(m_StatPack.DefaultName);
        }
        StatMonitorManager.SaveData();
        StatMonitorManager.StatPanel.RefreshFavorite();
    }

    private void HandleValueApplyClick(string inputText)
    {
        if (m_StatPack == null)
        {
            return;
        }

        if (!float.TryParse(inputText, out var value))
        {
            return;
        }

        StatMonitorManager.ChangeStatValue(m_StatPack, value - m_StatPack.Value.CurrentBaseValue);
    }
    
    private void HandleRateApplyClick(string inputText)
    {
        if (m_StatPack == null)
        {
            return;
        }

        if (!float.TryParse(inputText, out var value))
        {
            return;
        }

        StatMonitorManager.ChangeStatRate(m_StatPack, value - m_StatPack.Rate.CurrentBaseRate);
    }

    private void HandleValueResetClick()
    {
        if (m_StatPack == null)
        {
            return;
        }
        
        StatMonitorManager.ChangeStatValue(m_StatPack, m_StatPack.Value.BaseValue - m_StatPack.Value.CurrentBaseValue);
    }
    
    private void HandleRateResetClick()
    {
        if (m_StatPack == null)
        {
            return;
        }
        
        StatMonitorManager.ChangeStatRate(m_StatPack, m_StatPack.Rate.BaseRatePerTick - m_StatPack.Rate.CurrentBaseRate);
    }

    private void HandleValueLock(bool locked)
    {
        if (locked)
        {
            StatMonitorManager.LockedStatValue[m_StatPack.DefaultName] = (m_StatPack.Value.CurrentValue, m_StatPack.Value.CurrentBaseValue);
        }
        else
        {
            if (StatMonitorManager.LockedStatValue.ContainsKey(m_StatPack.DefaultName))
            {
                var prevValue = StatMonitorManager.LockedStatValue[m_StatPack.DefaultName].realValue;
                StatMonitorManager.LockedStatValue.Remove(m_StatPack.DefaultName);
                StatMonitorManager.UpdateStatStatuses(m_StatPack, prevValue);
            }
        }
        
        m_StatCellDetail.Refresh(m_StatPack);
    }
    
    private void HandleRateLock(bool locked)
    {
        if (locked)
        {
            StatMonitorManager.LockedStatRate[m_StatPack.DefaultName] = (m_StatPack.Rate.CurrentRatePerTick, m_StatPack.Rate.CurrentBaseRate);
        }
        else
        {
            if (StatMonitorManager.IsLockRate(m_StatPack))
            {
                var prevValue = StatMonitorManager.LockedStatRate[m_StatPack.DefaultName].realRate;
                StatMonitorManager.LockedStatRate.Remove(m_StatPack.DefaultName);
                StatMonitorManager.UpdateStatStatuses(m_StatPack, prevValue);
            }
        }
            
        m_StatCellDetail.Refresh(m_StatPack);
    }

    private void SetButton()
    {
        m_FoldOutButton.ButtonText.text = m_Foldout ? "►" : "▼";
        m_FoldOutButton.ButtonText.color = m_Foldout ? new Color(0.3f, 0.3f, 0.3f) : new Color(0.5f, 0.5f, 0.5f);
    }
    
    private void OnFoldoutClicked()
    {
        m_Foldout = !m_Foldout;
        SetButton();
        SetDetail();
    }

    private void SetDetail()
    {
        var show = !m_Foldout;
        
        m_StatCellDetail.SetActive(show);
        if (show)
        {
            m_StatCellDetail.Refresh(m_StatPack);
        }
    }
    
    public void Enable()
    {
        UIRoot.SetActive(true);
    }

    public void Disable()
    {
        UIRoot.SetActive(false);
    }
}