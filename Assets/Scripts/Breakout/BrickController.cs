using UnityEngine;

public class BrickController : MonoBehaviour {
    public int points = 10;
    private BreakoutGameManager gameManager;

    [Header("Effects")]
    public ParticleSystem destructionEffect;
    public AudioClip breakSound;
    public float soundVolume = 0.7f;
    private AudioSource audioSource;

    private void Start()
    {
        //gameManager = GetComponent<BreakoutGameManager>();
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ball")) {
            //gameManager.AddScore(points);
            PlayDestructionEffects();
            Destroy(gameObject);
        }
    }

    void PlayDestructionEffects() {
         AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position, soundVolume);
         ParticleSystem particles = Instantiate(
                destructionEffect, 
                transform.position, 
                Quaternion.identity
            );
        Destroy(particles.gameObject, particles.main.duration);
    }
}
