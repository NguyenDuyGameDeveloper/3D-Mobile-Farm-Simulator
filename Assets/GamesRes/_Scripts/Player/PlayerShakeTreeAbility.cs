using UnityEngine;

[RequireComponent(typeof(PlayerAnimator))]
public class PlayerShakeTreeAbility : MonoBehaviour
{
    [Header("Components")]
    private PlayerAnimator playerAnimator;

    [Header("Settings")]
    [SerializeField] private float distanceToTree;
    [Range(0f, 1f)]
    [SerializeField] private float shakeThreshold;
    private bool isActive;
    private Vector2 previousMousePosition;
    private bool isShaking;

    [Header("Elements")]
    private AppleTree currentTree;

    private void Awake()
    {
        playerAnimator = GetComponent<PlayerAnimator>();

        TreeManager.OnTreeModeStarted += TreeModeStartedCallback;
        TreeManager.OnTreeModeEnded += TreeModeEndedCallback;
    }
    private void Update()
    {
        if (isActive && !isShaking)
            ManageTreeShaking();
    }
    private void OnDestroy()
    {
        TreeManager.OnTreeModeStarted -= TreeModeStartedCallback;
        TreeManager.OnTreeModeEnded -= TreeModeEndedCallback;
    }
    private void TreeModeStartedCallback(AppleTree appleTree)
    {
        currentTree = appleTree;
        isActive = true;

        MoveTowardsTree();
    }
    private void TreeModeEndedCallback()
    {
        currentTree = null;
        isActive = false;
        isShaking = false;

        LeanTween.delayedCall(.1f, () => playerAnimator.StopShakeTreeAnimation());
    }
    private void MoveTowardsTree()
    {
        Vector3 treePos = currentTree.transform.position;
        Vector3 dir = transform.position - treePos;

        Vector3 flatDir = dir;
        flatDir.y = 0;

        Vector3 targetPos = treePos + flatDir.normalized * distanceToTree;
        playerAnimator.ManageAnimation(-flatDir);

        LeanTween.move(gameObject, targetPos, .5f);
    }
    private void ManageTreeShaking()
    {
        if (!Input.GetMouseButton(0))
        {
            currentTree.StopShaking();
            return;
        }

        float shakeMagnitude = Vector2.Distance(Input.mousePosition, previousMousePosition);

        if (ShouldShake(shakeMagnitude))
            Shake();
        else
            currentTree.StopShaking();

        previousMousePosition = Input.mousePosition;
    }
    private void Shake()
    {
        isShaking = true;

        currentTree.Shake();
        playerAnimator.PlayShakeTreeAnimation();

        LeanTween.delayedCall(.2f, () => isShaking = false);
    }
    private bool ShouldShake(float shakeMagnitude)
    {
        float screenPercent = shakeMagnitude / Screen.width;
        return screenPercent >= shakeThreshold;
    }
}
