using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFeedback : Feedback
{
    private SpriteRenderer _sr;

    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public override void FeedBack()
    {
        if(_sr == null)
            _sr = GetComponent<SpriteRenderer>();

        Sequence seq = DOTween.Sequence();
        seq.Append(_sr.DOColor(new Color(1, 0, 0, 0.1f), 0.1f));
        seq.Append(_sr.DOColor(Color.white, 0.1f));
    }
}
