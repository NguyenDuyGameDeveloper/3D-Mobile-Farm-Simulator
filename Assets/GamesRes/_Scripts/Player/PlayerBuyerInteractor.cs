using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuyerInteractor : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private InventoryManager inventoryManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Buyer"))
            SellCrops();
    }
    private void SellCrops()
    {
        Inventory inventory = inventoryManager.GetInventory();
        InventoryItem[] items = inventory.GetInventoryItems();

        int coinEarned = 0;

        for (int i = 0; i < items.Length; i++)
        {
            //Calculate the amout of Gold Player will earn tho Crop
            int itemPrice = DataManager.Instance.GetCropPriceFromCropType(items[i].cropType);
            coinEarned += itemPrice * items[i].cropAmount;
        }

        TransactionEffectManager.Instance.PlayCoinParticles(coinEarned);
        //CashManager.Instance.AddCoins(coinEarned);

        inventoryManager.ClearInventory();
    }
}
