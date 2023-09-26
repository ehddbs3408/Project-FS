using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RedPanelFeedback : Feedback
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public override void FeedBack()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() => _image.enabled = true);
        seq.Append(_image.DOFade(0.1f, 0.1f));
        seq.Append(_image.DOFade(0f, 0.1f));
        seq.AppendCallback(() => _image.enabled = false);
    }
}
