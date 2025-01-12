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
    }



    public void RobberCaught()
    {
        if (playerRole == PlayerRole.Cop)
        {
            robbersCaught++;
            if (robbersCaught >= LevelManager.Instance.robbersPerLevel)
            {
                LevelComplete();
            }
        }
    }

    public void PlayerCaught()
    {
        if (playerRole == PlayerRole.Robber)
        {
            UIManager.Instance.ShowGameOverMenu();
        }
    }

   public void LevelComplete()
    {
        currentState = GameState.LevelComplete;
       // Debug.Log("¡Nivel completado!");
        currentLevel++;
        UIManager.Instance.UpdateLevel(currentLevel);
        LevelManager.Instance.InitializeLevel(currentLevel); // Iniciar el siguiente nivel
        LevelManager.Instance.currentTime = 60f;
    }

    public void PauseGame()
    {
        currentState = GameState.Paused;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        currentState = GameState.Playing;
        Time.timeScale = 1;
    }
}