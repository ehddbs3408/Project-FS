using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIDialogue : UIBase
{
    public override void Init()
    {
        _root = UIManager.Instance.document.rootVisualElement.Q<VisualElement>("UI_Dialogue");
    }
}
