using System.Collections.Generic;
using CardEssential.Injector.Stat;
using UnityEngine;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using UniverseLib.UI.Widgets.ScrollView;

namespace CardEssential.Monitor.Stat;

public class StatTabModel : UIModel, ICellPoolDataSource<StatCell>
{
    public override GameObject UIRoot => m_ScrollPool.UIRoot;
    public string Classification { get; set; }
    public int ItemCount { get => m_StatPacks == null ? 0 : m_StatPacks.Count; }
    
    private ScrollPool<StatCell> m_ScrollPool;
    private List<StatPack> m_StatPacks;

    public StatTabModel(string classification)
    {
        Classification = classification;
    }
    
    public void Refresh()
    {
        m_ScrollPool.Refresh(true, false);
    }

    public void SetData(List<StatPack> statPacks)
    {
        m_StatPacks = statPacks;
    }

    public void Clear()
    {
        m_StatPacks?.Clear();
    }
    
    public void OnCellBorrowed(StatCell cell)
    {
        
    }

    public void SetCell(StatCell cell, int index)
    {
        if (index >= ItemCount)
        {
            cell.Disable();
            return;
        }
        cell.SetStat(m_StatPacks[index]);
    }
    
    public override void ConstructUI(GameObject parent)
    {
        m_ScrollPool = UIFactory.CreateScrollPool<StatCell>(parent, "StatScrollPool", out GameObject scrollRoot, out GameObject scrollContent, Color.gray);
        UIFactory.SetLayoutElement(scrollRoot, flexibleWidth: 9999, flexibleHeight: 9999);
        m_ScrollPool.Initialize(this);
    }
}