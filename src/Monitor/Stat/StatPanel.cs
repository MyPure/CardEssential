using System.Collections.Generic;
using System.Linq;
using CardEssential.Injector.Stat;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Panels;

namespace CardEssential.Monitor.Stat;

public class StatPanel : PanelBase
{
    public StatPanel(UIBase owner) : base(owner)
    {
    }

    private static Vector2 s_Position = new Vector2(200, -200);
    
    public override string Name => "CardEssential.Monitor.Stats (Ctrl + F)";
    public override int MinWidth => 900;
    public override int MinHeight => 600;
    public override Vector2 DefaultAnchorMin => new(0, 1);
    public override Vector2 DefaultAnchorMax => new(0, 1);

    public override bool CanDragAndResize => true;
    
    private List<StatPack> m_StatPacks = new();

    private int m_CurrentTab = 0;
    private List<ButtonRef> m_TabButtons = new ();
    private List<StatTabModel> m_StatTabModels = new ();
    private HashSet<string> m_FilteredData = new ();

    public override void SetActive(bool active)
    {
        base.SetActive(active);

        Refresh();
    }

    public void Refresh()
    {
        if (m_StatTabModels.Count > 0)
        {
            m_StatTabModels[m_CurrentTab].Refresh();
        }
    }
    
    public void SetData(List<StatPack> statPacks)
    {
        m_FilteredData.Clear();
        m_StatPacks = statPacks;
        foreach (var statTabModel in m_StatTabModels)
        {
            List<StatPack> filteredData;
            if (statTabModel.Classification != "Other")
            {
                filteredData = StatMonitorManager.StatFilter.Filter(statTabModel.Classification, statPacks);
                filteredData.ForEach(pack =>
                {
                    m_FilteredData.Add(pack.DefaultName);
                });
            }
            else
            {
                filteredData = statPacks.Where(pack => !m_FilteredData.Contains(pack.DefaultName)).ToList();
            }
            
            statTabModel.SetData(filteredData);
            statTabModel.Refresh();
        }
    }

    public void Clear()
    {
        m_StatPacks.Clear();
        m_FilteredData.Clear();
        foreach (var model in m_StatTabModels)
        {
            model.Clear();
            model.Refresh();
        }
    }
    
    public override void SetDefaultSizeAndPosition()
    {
        base.SetDefaultSizeAndPosition();

        Rect.pivot = new Vector2(0, 1);
        Rect.anchoredPosition = s_Position;
    }
    
    protected override void OnClosePanelClicked()
    {
        StatMonitorManager.ToggleDisplay();
    }

    protected override void ConstructPanelContent()
    {
        var buttonHeader = UIFactory.CreateUIObject("ButtonHeader", ContentRoot);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(buttonHeader, true, false, true, true, 2, 2, 2, 2, 2);

        int index = 0;
        foreach (var classification in StatMonitorManager.StatFilter.AllClassifications.Concat(new[] { "Other" }))
        {
            var tabButton = UIFactory.CreateButton(buttonHeader, classification, classification);
            UIFactory.SetLayoutElement(tabButton.GameObject, minWidth: 40, minHeight: 25);
            var indexCapture = index;
            tabButton.OnClick = () => SetTab(indexCapture);
            m_TabButtons.Add(tabButton);

            var model = new StatTabModel(classification);
            model.ConstructUI(ContentRoot);
            model.SetActive(false);
            m_StatTabModels.Add(model);

            index++;
        }

        SetTab(0);
    }

    public override void Destroy()
    {
        m_StatTabModels.ForEach(model => model.Destroy());
        s_Position = Rect.anchoredPosition;
        base.Destroy();
    }

    private void SetTab(int index)
    {
        m_StatTabModels[m_CurrentTab].SetActive(false);
        m_CurrentTab = index;
        m_StatTabModels[index].SetActive(true);
        Refresh();
    }
}