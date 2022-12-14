using System;
using UnityEngine;
using UnityEngine.UI;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace CardEssential.Monitor.Stat;

public class StatCellDetailContent : UIModel
{
    public override GameObject UIRoot => m_UiRoot;
    private Action<string> m_ApplyButtonClickedHandler;
    private Action m_ResetButtonClickedHandler;
    
    private GameObject m_UiRoot;
    private Text m_TopLabel;
    
    private GameObject m_BaseContent;
    private Text m_Base;
    private InputFieldRef m_BaseInputField;
    private ButtonRef m_BaseApply;
    private ButtonRef m_BaseReset;
    
    private GameObject m_AtBaseContent;
    private Text m_AtBase;
    
    private GameObject m_ModifiersContent;
    private Text m_ModifiersLabel;
    private Text m_ModifiersText;

    public StatCellDetailContent(Action<string> applyButtonClickedHandler, Action resetButtonClickedHandler)
    {
        m_ApplyButtonClickedHandler = applyButtonClickedHandler;
        m_ResetButtonClickedHandler = resetButtonClickedHandler;
    }
    
    public override void ConstructUI(GameObject parent)
    {
        m_UiRoot = UIFactory.CreateUIObject("StatCellContent", parent);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(m_UiRoot, false, false, true, true, 0, 0, 0, 0, 0);
        
        m_TopLabel = UIFactory.CreateLabel(m_UiRoot, "TopLabel", "", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_TopLabel.gameObject, minHeight: 25, minWidth: 30);
        
        //Base Content
        m_BaseContent = UIFactory.CreateUIObject("BaseContent", m_UiRoot);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(m_BaseContent, false, false, true, true, 10, 0, 0, 20, 0);
        
        m_Base = UIFactory.CreateLabel(m_BaseContent, "BaseLabel", "Base", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_Base.gameObject, minHeight: 25, minWidth: 30);

        m_BaseInputField = UIFactory.CreateInputField(m_BaseContent, "BaseInputField", "");
        UIFactory.SetLayoutElement(m_BaseInputField.GameObject, minHeight: 25, minWidth: 30);
        m_BaseInputField.Text = "0";

        m_BaseApply = UIFactory.CreateButton(m_BaseContent, "BaseApply", "Apply");
        UIFactory.SetLayoutElement(m_BaseApply.GameObject, minHeight: 25, minWidth: 40);
        m_BaseApply.OnClick = () => m_ApplyButtonClickedHandler(m_BaseInputField.Text);
        
        m_BaseReset = UIFactory.CreateButton(m_BaseContent, "BaseReset", "Reset");
        UIFactory.SetLayoutElement(m_BaseReset.GameObject, minHeight: 25, minWidth: 40);
        m_BaseReset.OnClick = () => m_ResetButtonClickedHandler();
        
        //AtBase Content
        m_AtBaseContent = UIFactory.CreateUIObject("AtBaseContent", m_UiRoot);
        UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(m_AtBaseContent, false, false, true, true, 5, 0, 0, 20, 0);
        
        m_AtBase = UIFactory.CreateLabel(m_AtBaseContent, "AtBaseLabel", "AtBase", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_AtBase.gameObject, minHeight: 25, minWidth: 30);
        
        //Modifiers Content
        m_ModifiersContent = UIFactory.CreateUIObject("ModifiersContent", m_UiRoot);
        UIFactory.SetLayoutGroup<VerticalLayoutGroup>(m_ModifiersContent, false, false, true, true, 5, 0, 0, 20, 0);
        
        m_ModifiersLabel = UIFactory.CreateLabel(m_ModifiersContent, "ModifiersLabel", "Modifiers", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_ModifiersLabel.gameObject, minHeight: 25, minWidth: 30);
        
        m_ModifiersText = UIFactory.CreateLabel(m_ModifiersContent, "ModifiersText", "ModifiersText", TextAnchor.MiddleLeft, Color.white, true, 15);
        UIFactory.SetLayoutElement(m_ModifiersText.gameObject, minHeight: 25, minWidth: 30);
    }

    public void Refresh(string topLabel, string baseText, bool notAtBase, string atBaseText, string modifiersLabel,
        string modifiersText)
    {
        m_TopLabel.text = topLabel;
        m_Base.text = baseText;
        if (notAtBase || string.IsNullOrEmpty(atBaseText))
        {
            m_AtBase.gameObject.SetActive(false);
        }
        else
        {
            m_AtBase.gameObject.SetActive(true);
        }
        m_AtBase.text = atBaseText;
        m_ModifiersLabel.text = modifiersLabel;
        m_ModifiersText.text = modifiersText;
        m_ModifiersContent.gameObject.SetActive(!string.IsNullOrEmpty(modifiersText));
    }
}