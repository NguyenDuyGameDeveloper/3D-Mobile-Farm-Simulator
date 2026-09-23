using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ChunkWalls))]
public class Chunk : MonoBehaviour
{
    [Header("Actions")]
    public static Action OnUnlocked;
    public static Action OnPriceChanged;

    [Header("Elements")]
    [SerializeField] private GameObject unlockedElements;
    [SerializeField] private GameObject lockedElements;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private MeshFilter chunkFilter;
    private ChunkWalls chunkWalls;

    [Header("Settings")]
    [SerializeField] private int initialPrice;
    private int currentPrice;
    private bool unlocked;
    private int configuration;

    private void Awake()
    {
        chunkWalls = GetComponent<ChunkWalls>();
    }
    public void Initialize(int loadedPrice)
    {
        currentPrice = loadedPrice;
        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock(false);
    }
    public void TryUnlock()
    {
        if (CashManager.Instance.GetCoins() <= 0) return;

        currentPrice--;
        CashManager.Instance.UseCoin(1);

        OnPriceChanged?.Invoke();

        priceText.text = currentPrice.ToString();

        if (currentPrice <= 0)
            Unlock();
    }
    private void Unlock(bool triggerAction = true)
    {
        unlockedElements.SetActive(true);
        lockedElements.SetActive(false);

        unlocked = true;

        if (triggerAction == true)
            OnUnlocked?.Invoke();
    }
    public void SetRenderer(Mesh chunkMesh)
    {
        chunkFilter.mesh = chunkMesh;
    }
    public void DisplayLockedElements() => lockedElements.SetActive(true);
    public void UpdateWalls(int configuration)
    {
        this.configuration = configuration;
        chunkWalls.Configuration(configuration);
    }
    public bool IsUnlocked() => unlocked;
    public int GetCurrentPrice() => currentPrice;
    public int GetInitialPrice() => initialPrice;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, Vector3.one * 5);

        Gizmos.color = new Color(0, 0, 0, 0);
        Gizmos.DrawCube(transform.position, Vector3.one * 5);
    }
}
