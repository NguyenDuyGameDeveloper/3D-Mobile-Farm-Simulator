using System.IO;
using UnityEngine;

[RequireComponent(typeof(InventoryDisplay))]
public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private Inventory inventory;
    private InventoryDisplay inventoryDisplay;
    private string dataPath;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        inventoryDisplay = GetComponent<InventoryDisplay>();

        dataPath = Application.persistentDataPath + "/InventoryData.txt";

        LoadInventory();
        ConfigureInventoryDisplay();

        CropTile.OnCropHarvested += AddCrop;
        AppleTree.OnAppleHarvested += AddCrop;
    }
    private void OnDestroy()
    {
        CropTile.OnCropHarvested -= AddCrop;
        AppleTree.OnAppleHarvested -= AddCrop;
    }
    public void AddCrop(CropType cropType)
    {
        inventory.CropHarvestedCallback(cropType);

        inventoryDisplay.UpdateDisplay(inventory);

        SaveInventory();
    }
    public Inventory GetInventory() { return inventory; }
    [NaughtyAttributes.Button]
    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDisplay.UpdateDisplay(inventory);
        SaveInventory();
    }
    private void LoadInventory()
    {
        string data = "";
        if (File.Exists(dataPath))
        {
            data = File.ReadAllText(dataPath);
            inventory = JsonUtility.FromJson<Inventory>(data);

            inventory ??= new Inventory();
        }
        else
        {
            File.Create(dataPath);
            inventory = new Inventory();
        }
    }
    private void SaveInventory()
    {
        string data = JsonUtility.ToJson(inventory, true);
        File.WriteAllText(dataPath, data);
    }
    private void ConfigureInventoryDisplay() => inventoryDisplay.Configure(inventory);
}
