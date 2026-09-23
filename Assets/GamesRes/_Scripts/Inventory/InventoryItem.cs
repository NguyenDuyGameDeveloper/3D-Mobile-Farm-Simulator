[System.Serializable]
public class InventoryItem
{
    public CropType cropType;
    public int cropAmount;

    public InventoryItem(CropType cropType, int cropAmount)
    {
        this.cropType = cropType;
        this.cropAmount = cropAmount;
    }
}
