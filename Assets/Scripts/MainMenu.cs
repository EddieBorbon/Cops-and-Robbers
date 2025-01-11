using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayAsCop()
    {
        GameSettings.Instance.SetPlayerRole(PlayerRole.Cop); // Guardar la elección
        SceneManager.LoadScene("GameScene"); // Cargar la escena de juego
    }

    public void PlayAsRobber()
    {
        GameSettings.Instance.SetPlayerRole(PlayerRole.Robber); // Guardar la elección
        SceneManager.LoadScene("GameScene"); // Cargar la escena de juego
    }
}