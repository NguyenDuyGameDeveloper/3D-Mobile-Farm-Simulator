using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerWaterAbility : MonoBehaviour
{
    [Header("Elements")]
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header("Settings")]
     private CropField currentCropField;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        WaterParticles.OnWaterCollided += WaterCollidedCallback;
        CropField.OnFullyWatered += CropFieldFullyWateredCallback;
        PlayerToolSelector.OnToolSelected += ToolSelectedCallback;
    }
    private void OnDestroy()
    {
        WaterParticles.OnWaterCollided -= WaterCollidedCallback;
        CropField.OnFullyWatered -= CropFieldFullyWateredCallback;
        PlayerToolSelector.OnToolSelected -= ToolSelectedCallback;
    }
    private void ToolSelectedCallback(Tool selectedTool)
    {
        if (!playerToolSelector.CanWater())
            playerAnimator.StopWaterAnimation();
    }
    private void WaterCollidedCallback(Vector3[] waterPositions)
    {
        if (currentCropField == null) return;

        currentCropField.WaterCollidedCallback(waterPositions);
    }
    private void CropFieldFullyWateredCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopWaterAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsSown())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsSown())
            EnteredCropField(other.GetComponent<CropField>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopWaterAnimation();
            currentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanWater())
        {
            if(currentCropField == null)
                currentCropField = cropField;

            playerAnimator.PlayWaterAnimation();
        }
    }
}
