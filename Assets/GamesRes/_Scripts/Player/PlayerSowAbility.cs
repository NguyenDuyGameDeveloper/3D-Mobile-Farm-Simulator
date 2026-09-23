using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerToolSelector))]
public class PlayerSowAbility : MonoBehaviour
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

        SeedParticles.OnSeedsCollided += SeedsCollidedCallback;
        CropField.OnFullySown += CropFieldFullySownCallback;
        PlayerToolSelector.OnToolSelected += ToolSelectedCallback;
    }
    private void OnDestroy()
    {
        SeedParticles.OnSeedsCollided -= SeedsCollidedCallback;
        CropField.OnFullySown -= CropFieldFullySownCallback;
        PlayerToolSelector.OnToolSelected -= ToolSelectedCallback;
    }
    private void ToolSelectedCallback(Tool selectedTool)
    {
        if (!playerToolSelector.CanSow())
            playerAnimator.StopSowAnimation();
    }
    private void SeedsCollidedCallback(Vector3[] seedPositions)
    {
        if (currentCropField == null) return;

        currentCropField.SeedsCollidedCallback(seedPositions);
    }
    private void CropFieldFullySownCallback(CropField cropField)
    {
        if (cropField == currentCropField)
            playerAnimator.StopSowAnimation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsEmpty())
        {
            currentCropField = other.GetComponent<CropField>();
            EnteredCropField(currentCropField);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("CropField") &&
            other.GetComponent<CropField>().IsEmpty())
            EnteredCropField(other.GetComponent<CropField>());
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CropField"))
        {
            playerAnimator.StopSowAnimation();
            currentCropField = null;
        }
    }
    private void EnteredCropField(CropField cropField)
    {
        if (playerToolSelector.CanSow())
            playerAnimator.PlaySowAnimation();
    }
}
