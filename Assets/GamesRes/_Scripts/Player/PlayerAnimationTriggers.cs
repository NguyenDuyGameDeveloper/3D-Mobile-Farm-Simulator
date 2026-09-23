using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimationTriggers : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ParticleSystem seedParticle;
    [SerializeField] private ParticleSystem waterParticle;

    [Header("Events")]
    [SerializeField] private UnityEvent StartHavesting;
    [SerializeField] private UnityEvent StopHavesting;

    private void PlaySeedParticle() => seedParticle.Play();
    private void PlayWaterParticle() => waterParticle.Play();
    private void StartHavestingCallback()
    {
        StartHavesting?.Invoke();
    }
    private void StopHavestingCallback()
    {
        StopHavesting?.Invoke();
    }
}
