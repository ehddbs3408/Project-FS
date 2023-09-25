using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShakeFeedback : Feedback
{
    private CinemachineVirtualCamera _cm;
    private CinemachineBasicMultiChannelPerlin _noizeChannel;

    [SerializeField]
    private float _amplitudeGainTime = 0.1f;
    [SerializeField]
    private float _unAmplitudeGainTime = 0.1f;
    [SerializeField]
    private float _amplitudeGainValue = 1;

    private void Start()
    {
        _cm = GetComponent<CinemachineVirtualCamera>();
        _noizeChannel = _cm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public override void FeedBack()
    {
        DOTween.To(() => _noizeChannel.m_AmplitudeGain, x => _noizeChannel.m_AmplitudeGain = x, _amplitudeGainValue, _amplitudeGainTime)
            .OnComplete(() => DOTween.To(() => _noizeChannel.m_AmplitudeGain, x => _noizeChannel.m_AmplitudeGain = x, 0, _unAmplitudeGainTime));
    }
}
