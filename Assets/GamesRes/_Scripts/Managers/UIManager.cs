using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject treePanel;
    
    [SerializeField] private GameObject treeButton;
    [SerializeField] private GameObject toolButtonsContainer;

    private void Awake()
    {
        SetGameMode();

        PlayerDetection.OnEnteredTreeZone += EnteredTreeZoneCallback;
        PlayerDetection.OnExittedTreeZone += ExittedTreeZoneCallback;

        TreeManager.OnTreeModeStarted += SetTreeMode;
        TreeManager.OnTreeModeEnded += SetGameMode;
    }
    private void Start()
    {
        SetGameMode();
    }
    private void OnDestroy()
    {
        PlayerDetection.OnEnteredTreeZone -= EnteredTreeZoneCallback;
        PlayerDetection.OnExittedTreeZone -= ExittedTreeZoneCallback;

        TreeManager.OnTreeModeStarted -= SetTreeMode;
        TreeManager.OnTreeModeEnded -= SetGameMode;
    }
    private void EnteredTreeZoneCallback(AppleTree appleTree)
    {
        treeButton.SetActive(true);
        toolButtonsContainer.SetActive(false);
    }
    private void ExittedTreeZoneCallback(AppleTree appleTree)
    {
        treeButton.SetActive(false);
        toolButtonsContainer.SetActive(true);
    }
    private void SetGameMode()
    {
        gamePanel.SetActive(true);
        treePanel.SetActive(false);
    }
    private void SetTreeMode(AppleTree appleTree)
    {
        gamePanel.SetActive(false);
        treePanel.SetActive(true);
    }
}
