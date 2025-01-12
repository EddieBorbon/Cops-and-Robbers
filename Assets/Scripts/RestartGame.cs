using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    public void restartGame()
    {
        // Reiniciar la escena principal
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainScene"); // Cambia "MainScene" por el nombre de tu escena principal
    }
}
