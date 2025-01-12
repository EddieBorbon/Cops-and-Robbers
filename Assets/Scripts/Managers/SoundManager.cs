using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioClip moneyCollectedSound; // Sonido al recolectar dinero
    public AudioClip robberCaughtSound;   // Sonido al atrapar a un ladrón
    public AudioClip gameOverSound;       // Sonido al perder el juego
    public AudioClip nextLevelSound;       // Sonido al perder el juego

    private AudioSource audioSource;

    void Awake()
    {
        // Implementar el patrón Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }

        // Obtener o añadir el componente AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Método para reproducir el sonido de recolectar dinero
    public void PlayMoneyCollectedSound()
    {
        PlaySound(moneyCollectedSound);
    }

    // Método para reproducir el sonido de atrapar a un ladrón
    public void PlayRobberCaughtSound()
    {
        PlaySound(robberCaughtSound);
    }

    // Método para reproducir el sonido de Game Over
    public void PlayGameOverSound()
    {
        PlaySound(gameOverSound);
    }

    // Método para reproducir el sonido de Game Over
    public void PlayNextLevelSound()
    {
        PlaySound(nextLevelSound);
    }

    // Método genérico para reproducir un sonido
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Reproducir el sonido sin interrumpir la música de fondo
        }
    }
}