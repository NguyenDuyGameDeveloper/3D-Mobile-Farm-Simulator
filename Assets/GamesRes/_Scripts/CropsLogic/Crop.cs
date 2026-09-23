using UnityEngine;

public class Crop : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform cropRenderer;
    [SerializeField] private ParticleSystem harvestedParticle;

    public void ScaleUp() => cropRenderer.gameObject.LeanScale(Vector3.one, 1).setEase(LeanTweenType.easeInCubic);
    public void ScaleDown()
    {
        cropRenderer.gameObject.LeanScale(Vector3.zero, .1f).setEase(LeanTweenType.easeInCubic).
        setOnComplete(() => Destroy(gameObject));

        harvestedParticle.gameObject.SetActive(true);
        harvestedParticle.transform.parent = null;
        harvestedParticle.Play();
    }

    //private IEnumerator ScaleCoroutine()
    //{
    //    Vector3 startScale = cropRenderer.localScale;
    //    Vector3 targetScale = new(1, 1, 1);

    //    float time = 0f;
    //    float duration = 1f; // thời gian scale (1 giây)

    //    while (time < duration)
    //    {
    //        time += Time.deltaTime;
    //        float t = time / duration;

    //        cropRenderer.localScale = Vector3.Lerp(startScale, targetScale, t);

    //        yield return null;
    //    }

    //    cropRenderer.localScale = targetScale;
    //}
}
