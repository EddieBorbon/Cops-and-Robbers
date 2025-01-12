using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver,
        LevelComplete
    }

    public GameState currentState = GameState.MainMenu;
    public PlayerRole playerRole; // Rol seleccionado por el jugador

    public int currentLevel = 1;
    public int robbersCaught = 0;
    public int moneyCaught = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartGame();
    }

    void StartGame()
    {
        currentState = GameState.Playing;
        LevelManager.Instance.InitializeLevel(currentLevel); // Iniciar el primer nivel
    }

    public void RobberCaught()
    {
        if (playerRole == PlayerRole.Cop)
        {
            robbersCaught++;
            UIManager.Instance.UpdateRobbersLeft(robbersCaught);
            if (robbersCaught >= LevelManager.Instance.robbersPerLevel)
            {
                LevelComplete();
            }
        }
    }

    public void MoneyCaught(){
        if(playerRole == PlayerRole.Robber){
            moneyCaught++;
            Debug.Log(moneyCaught);
            UIManager.Instance.UpdateMoneyUI(moneyCaught);
            if(moneyCaught >= LevelManager.Instance.moneyPerLevel){
                LevelComplete();
            }
        }
    }

    public void PlayerCaught()
    {
        if (playerRole == PlayerRole.Robber)
        {
            GameOver();
        }
    }

    public void LevelComplete()
    {
        currentState = GameState.LevelComplete;
        currentLevel++;
        UIManager.Instance.UpdateLevel(currentLevel); // Actualizar la UI del nivel
        LevelManager.Instance.InitializeLevel(currentLevel); // Iniciar el siguiente nivel
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        UIManager.Instance.ShowGameOverMenu(); // Mostrar el menú de Game Over
        Time.timeScale = 0; // Pausar el juego
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0; // Pausar el juego
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1; // Reanudar el juego
    }

    public void RestartGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainScene"); // Cambia "MainScene" por el nombre de tu escena principal
    }

    public void QuitGame()
    {
        Application.Quit(); // Salir del juego
    }
}