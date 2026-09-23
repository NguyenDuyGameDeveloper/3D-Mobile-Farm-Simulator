using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropContainersParent;
    [SerializeField] private UICropContainer uiCropContainerPrefab;

    public void Configure(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for (int i = 0; i < items.Length; i++)
        {
            UICropContainer cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);

            Sprite cropIcon = DataManager.Instance.GetCropSpriteFromCropType(items[i].cropType);
            int cropAmount = items[i].cropAmount;

            cropContainerInstance.Configure(cropIcon, cropAmount);
        }
    }           
    public void UpdateDisplay(Inventory inventory)
    {
        InventoryItem[] items = inventory.GetInventoryItems();

        for(int i = 0;i < items.Length;i++)
        {
            UICropContainer cropContainerInstance;
            if (i < cropContainersParent.childCount)
            {
                cropContainerInstance = cropContainersParent.GetChild(i).GetComponent<UICropContainer>();
                cropContainerInstance.gameObject.SetActive(true);
            }
            else
            {
                cropContainerInstance = Instantiate(uiCropContainerPrefab, cropContainersParent);
            }

            Sprite cropIcon = DataManager.Instance.GetCropSpriteFromCropType(items[i].cropType);
            int cropAmout = items[i].cropAmount;

            cropContainerInstance.Configure(cropIcon, cropAmout);
        }

        int remainingContainers = cropContainersParent.childCount - items.Length;

        if(remainingContainers <= 0)
            return;

        for(int i = 0; i < remainingContainers; i++)
        {
            cropContainersParent.GetChild(items.Length + i).gameObject.SetActive(false);
        }
    }
}
