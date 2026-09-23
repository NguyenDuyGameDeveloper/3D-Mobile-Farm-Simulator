using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private ParticleSystem waterParticle;
    private Animator anim;

    [Header("Settings")]
    [SerializeField] private float moveSpeedMultiplier;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void ManageAnimation(Vector3 moveVector)
    {
        if (moveVector.magnitude > 0)
        {
            anim.SetFloat("moveSpeed", moveVector.magnitude * moveSpeedMultiplier);
            PlayRunAnimation();

            anim.transform.forward = moveVector.normalized;
        }
        else
            PlayIdleAnimation();
    }
    private void PlayRunAnimation() => anim.Play("PlayerRun");
    private void PlayIdleAnimation() => anim.Play("PlayerIdle");
    public void PlaySowAnimation() => anim.SetLayerWeight(1, 1);
    public void StopSowAnimation() => anim.SetLayerWeight(1, 0);

    public void PlayWaterAnimation() => anim.SetLayerWeight(2, 1);
    public void StopWaterAnimation()
    {
        anim.SetLayerWeight(2, 0);
        waterParticle.Stop();
    }
    public void PlayHavestAnimation() => anim.SetLayerWeight(3, 1);
    public void StopHavestAnimation() => anim.SetLayerWeight(3, 0);
    public void PlayShakeTreeAnimation()
    {
        anim.SetLayerWeight(4, 1);
        anim.Play("Shake Tree");
    }
    public void StopShakeTreeAnimation() => anim.SetLayerWeight(4, 0);
}
