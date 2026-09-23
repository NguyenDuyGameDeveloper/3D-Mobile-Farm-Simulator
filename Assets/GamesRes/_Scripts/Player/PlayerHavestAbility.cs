using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerHavestAbility : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform havestSphere;
    private PlayerAnimator playerAnimator;
    private PlayerToolSelector playerToolSelector;

    [Header("Settings")]
    private CropField currentCropField;
    private bool canHavest;

    private void Start()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
        playerToolSelector = GetComponent<PlayerToolSelector>();

        CropField.OnFullyHarvested += CropFieldFullyHarvestedCallback;
        PlayerToolSelector.OnToolSelected += ToolSelectedCallback;
    }
    private void OnDestroy()
    {
        CropField.OnFullyHarvested -= CropFieldFullyHarvestedCallback;
        PlayerToolSelector.OnToolSelected -= ToolSelectedCallback;
    }
    private void ToolSelectedCallback(Tool selectedTool)
    {
        if (!playerToolSelector.CanHavest())
            playerAnimator.StopHavestAnimation();
    }
    private void CropFieldFullyHarvestedCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopHavestAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsWatered())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsWatered())
            EnteredCropField(other.GetComponent<CropField>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopHavestAnimation();
            currentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanHavest())
        {
            if (currentCropField == null)
                currentCropField = cropField;

            playerAnimator.PlayHavestAnimation();

            if(canHavest)
                currentCropField.Havest(havestSphere);
        }
    }
    public void HavestingStartedCallback()
    {
        canHavest = true;
    }
    public void HavestingStoppedCallback()
    {
        canHavest = false;

    }
}
