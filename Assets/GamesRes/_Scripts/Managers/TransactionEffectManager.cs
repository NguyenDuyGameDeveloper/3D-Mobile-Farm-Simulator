using System.Collections;
using UnityEngine;

public class TransactionEffectManager : MonoBehaviour
{
    public static TransactionEffectManager Instance;

    [Header("Elements")]
    [SerializeField] private ParticleSystem coinParticle;
    [SerializeField] private RectTransform coinRectTransform;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private int coinAmount;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    private Camera camera;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        camera = Camera.main;
    }
    [NaughtyAttributes.Button]
    public void PlayCoinParticleTest()
    {
        PlayCoinParticles(100);
    }
    public void PlayCoinParticles(int amount)
    {
        if (coinParticle.isPlaying) return;

        ParticleSystem.Burst burst = coinParticle.emission.GetBurst(0);
        burst.count = amount;
        coinParticle.emission.SetBurst(0, burst);

        ParticleSystem.MainModule main = coinParticle.main;
        main.gravityModifier = 2;

        coinParticle.Play();
        coinAmount = amount;

        StartCoroutine(PlayCoinParticlesCoroutine());
    }
    private IEnumerator PlayCoinParticlesCoroutine()
    {
        yield return new WaitForSeconds(1.2f);

        ParticleSystem.MainModule main = coinParticle.main;
        main.gravityModifier = 0;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[coinAmount];
        coinParticle.GetParticles(particles);

        Vector3 direction = (coinRectTransform.transform.position - camera.transform.position).normalized;

        while (coinParticle.isPlaying)
        {
            coinParticle.GetParticles(particles);

            Vector3 targetPosition = camera.transform.position + direction * Vector3.Distance(camera.transform.position,
                coinParticle.transform.position);

            for (int i = 0; i < particles.Length; i++)
            {
                if (particles[i].remainingLifetime <= 0) continue;

                particles[i].position = Vector3.MoveTowards(particles[i].position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(particles[i].position, targetPosition) < 1f)
                {
                    particles[i].position += Vector3.up * 100000;
                    //particles[i].remainingLifetime = 0;
                    CashManager.Instance.AddCoins(1);
                }
            }

            coinParticle.SetParticles(particles);

            yield return null;
        }
    }
}
