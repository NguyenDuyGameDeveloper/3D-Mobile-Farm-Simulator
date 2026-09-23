using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerToolSelector : MonoBehaviour
{
    private Tool activeTool;

    [Header("Actions")]
    public static Action<Tool> OnToolSelected;

    [Header("Elements")]
    [SerializeField] private Image[] toolImages;

    [Header("Settings")]
    [SerializeField] private Color selectedButtonColor;

    private void Start()
    {
        SelectTool(0);
    }

    public void SelectTool(int toolIndex)
    {
        activeTool = (Tool)toolIndex;

        for (int i = 0; i < toolImages.Length; i++)
        {
            toolImages[i].color = i == toolIndex ? selectedButtonColor : Color.white;
        }

        OnToolSelected?.Invoke(activeTool);
    }
    public bool CanSow() => activeTool == Tool.Sow;
    public bool CanWater() => activeTool == Tool.Water;
    public bool CanHavest() => activeTool == Tool.Havest;
}
