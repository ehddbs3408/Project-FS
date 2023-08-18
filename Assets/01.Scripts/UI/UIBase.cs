using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBase
{
    protected VisualElement _root;

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        _root.style.display = DisplayStyle.Flex;
    }

    public virtual void Hide()
    {
        _root.style.display = DisplayStyle.None;
    }
}
