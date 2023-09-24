using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    [SerializeField]
    private Sprite[] _haveHeratSprite = new Sprite[2];

    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        Debug.Log(Convert.ToInt32(true));
        Debug.Log(Convert.ToInt32(false));
    }

    public void UpdateHeart(bool isHave)
    {
        if(_image == null)
            _image = GetComponent<Image>();
        _image.sprite = _haveHeratSprite[Convert.ToInt32(isHave)];
    }
}
