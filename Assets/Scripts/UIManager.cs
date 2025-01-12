using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance

    public TextMeshProUGUI moneyRobbersText; // Texto para mostrar el dinero recolectado
    public GameObject pauseMenu; // Menú de pausa
    public GameObject gameOverMenu; // Menú de fin de juego
    public TextMeshProUGUI timerText; // Referencia al Text de UI para mostrar el tiempo
    public TextMeshProUGUI levelText; // Referencia al Text de UI para mostrar el tiempo

    void Awake()
    {
        // Configurar el singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evitar duplicados
        }
    }

    void Start()
    {
        // Inicializar la UI
        UpdateMoneyUI(0);
        HidePauseMenu();
        UpdateLevel(1);
    }

    void Update()
    {
        // Mostrar/ocultar el menú de pausa con la tecla "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

   
    // Actualizar el contador de dinero
    public void UpdateMoneyUI(int money)
    {
        moneyRobbersText.text = "Money: " + money;
    }

    public void UpdateRobbersLeft(int robbersLeft)
    {
        moneyRobbersText.text = "Robbers Left: " + robbersLeft;
    }

    // Mostrar/ocultar el menú de pausa
    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            HidePauseMenu();
            Time.timeScale = 1; // Reanudar el juego
        }
        else
        {
            ShowPauseMenu();
            Time.timeScale = 0; // Pausar el juego
        }
    }

    public void ShowPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    // Mostrar el menú de fin de juego
    public void ShowGameOverMenu()
    {
        gameOverMenu.SetActive(true);
    }

    public void HideGameOverMenu()
    {
        gameOverMenu.SetActive(false);
    }


    public void OnGameOverMainMenuButtonClicked()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainMenu"); // Cargar el menú principal
    }

    public void UpdateTimerUI(float currentTime)
    {
        if (timerText != null)
        {
            // Mostrar el tiempo en formato MM:SS
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void UpdateLevel(int level){
        levelText.text = "Level: " + level;
    }
}