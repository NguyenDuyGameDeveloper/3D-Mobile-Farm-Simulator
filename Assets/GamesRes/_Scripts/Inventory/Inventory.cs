using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] private List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public void CropHarvestedCallback(CropType cropType)
    {
        bool cropFound = false;

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem inventoryItem = inventoryItems[i];

            if (inventoryItem.cropType == cropType)
            {
                inventoryItem.cropAmount++;
                cropFound = true;
                break;
            }
        }
        //DebugInventory();

        if (cropFound)
            return;

        inventoryItems.Add(new InventoryItem(cropType, 1));

    }
    public InventoryItem[] GetInventoryItems() => inventoryItems.ToArray();
    public void Clear() => inventoryItems.Clear();
    public void DebugInventory()
    {
        foreach (InventoryItem inventoryItem in inventoryItems)
        {
            Debug.Log("We have " + inventoryItem.cropAmount + " " + inventoryItem.cropType + " in our Inventory!");
        }
    }
}
