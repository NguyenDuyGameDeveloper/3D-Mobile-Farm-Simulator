using System;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Slider shakeSlider;

    [Header("Settings")]
    private AppleTree lastTriggerTree;

    [Header("Actions")]
    public static Action<AppleTree> OnTreeModeStarted;
    public static Action OnTreeModeEnded;

    private void Start()
    {
        PlayerDetection.OnEnteredTreeZone += EnteredTreeZoneCallback;
    }
    private void OnDestroy()
    {
        PlayerDetection.OnEnteredTreeZone -= EnteredTreeZoneCallback;
    }

    private void EnteredTreeZoneCallback(AppleTree appleTree)
    {
        this.lastTriggerTree = appleTree;
    }
    public void TreeButtonCallback()
    {
        if(!lastTriggerTree.IsReady())
        {
            Debug.Log("Tree is not ready!");
            return;
        }

        StartTreeMode();
    }
    private void StartTreeMode()
    {
        lastTriggerTree.Initialize(this);

        OnTreeModeStarted?.Invoke(lastTriggerTree);

        // Initialize the Slider
        UpdateShakeSlider(0);
    }
    public void UpdateShakeSlider(float value)
    {
        shakeSlider.value = value;
    }
    public void EndTreeMode()
    {
        OnTreeModeEnded?.Invoke();
    }
}
