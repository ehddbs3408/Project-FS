using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimeFeedback : Feedback
{
    [SerializeField]
    private float _timeFreezeTime = 0.2f;
    [SerializeField]
    private float _timeUnFreezeTime = 0.2f;
    [SerializeField]
    private float _timeFreezeValue = 0.2f;

    public override void FeedBack()
    {
        DOTween.To(() => Time.timeScale, x => Time.timeScale = x, _timeFreezeValue, _timeFreezeTime)
            .OnComplete(() => DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1, _timeUnFreezeTime));
    }
}
