using System;
using UnityEngine;

public class AppleTree : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject treeCam;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] private Renderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] private Transform appleHolder;
    private TreeManager treeManager;

    [Header("Settings")]
    [SerializeField] private float maxShakeMagnitude;
    [SerializeField] private float shakeIncrement;
    private float shakeSliderValue;
    private float shakeMagnitude;
    private bool isShaking;

    [Header("Actions")]
    public static Action<CropType> OnAppleHarvested;

    public void Shake()
    {
        isShaking = true;

        TweenShake(maxShakeMagnitude);
        UpdateShakeSlider();
    }
    private void UpdateShakeSlider()
    {
        shakeSliderValue += shakeIncrement;
        treeManager.UpdateShakeSlider(shakeSliderValue);

        for (int i = 0; i < appleHolder.childCount; i++)
        {
            float applePercent = (float)i / appleHolder.childCount;

            Apple currentApple = appleHolder.GetChild(i).GetComponent<Apple>();

            if (shakeSliderValue > applePercent && !currentApple.IsDropped())
                DropApple(currentApple);
        }

        if (shakeSliderValue >= 1)
            ExitTreeMode();
    }
    private void ExitTreeMode()
    {
        treeManager.EndTreeMode();

        DisableTreeCam();
        TweenShake(0);

        ResetApples();
    }
    private void ResetApples()
    {
        for (int i = 0; i < appleHolder.childCount; i++)
        {
            appleHolder.GetChild(i).GetComponent<Apple>().Reset();
        }
    }
    private void DropApple(Apple apple)
    {
        apple.DropApple();
        OnAppleHarvested?.Invoke(CropType.Apple);
    }
    public void Initialize(TreeManager treeManager)
    {
        EnableTreeCam();
        shakeSliderValue = 0;
        this.treeManager = treeManager;
    }
    public void StopShaking()
    {
        if (!isShaking)
            return;
        isShaking = false;

        Debug.Log("Stopped");

        TweenShake(0);
    }
    public bool IsReady()
    {
        for (int i = 0; i < appleHolder.childCount; i++)
            if (!appleHolder.GetChild(i).GetComponent<Apple>().IsReady())
                return false;

        return true;
    }
    private void TweenShake(float targetMagnitude)
    {
        LeanTween.cancel(renderer.gameObject);
        LeanTween.value(renderer.gameObject, UpdateShakeMagnitude, shakeMagnitude, targetMagnitude, 1f);
    }
    private void UpdateShakeMagnitude(float value)
    {
        shakeMagnitude = value;
        UpdateMaterials();
    }
    private void UpdateMaterials()
    {
        foreach (Material material in renderer.materials)
        {
            material.SetFloat("_Magnitude", shakeMagnitude);
        }

        foreach (Transform transform in appleHolder.transform)
        {
            Apple apple = transform.GetComponent<Apple>();

            if (apple.IsDropped())
                continue;
            apple.Shake(shakeMagnitude);
        }
    }
    public void EnableTreeCam() => treeCam.SetActive(true);
    public void DisableTreeCam() => treeCam.SetActive(false);
}
