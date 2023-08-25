using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBase
{
    protected GameObject _parent;

    public virtual void Init()
    {

    }

    public virtual void Show()
    {
        _parent.gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        _parent.gameObject.SetActive(false);
    }
}
