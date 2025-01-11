using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance; // Singleton instance

    public TextMeshProUGUI messageText; // Texto para mensajes (ej. "¡Nivel completado!")
    public TextMeshProUGUI moneyRobbersText; // Texto para mostrar el dinero recolectado
    public GameObject pauseMenu; // Menú de pausa
    public GameObject gameOverMenu; // Menú de fin de juego

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
        HideMessage();
        HidePauseMenu();
        HideGameOverMenu();
    }

    void Update()
    {
        // Mostrar/ocultar el menú de pausa con la tecla "Esc"
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    // Mostrar un mensaje en la pantalla
    public void ShowMessage(string message, float duration = 2.0f)
    {
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        Invoke("HideMessage", duration); // Ocultar el mensaje después de un tiempo
    }

    // Ocultar el mensaje
    public void HideMessage()
    {
        messageText.gameObject.SetActive(false);
    }

    // Actualizar el contador de dinero
    public void UpdateMoneyUI(int money)
    {
        moneyRobbersText.text = "Dinero: " + money;
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

    // Botones del menú de pausa
    public void OnResumeButtonClicked()
    {
        TogglePauseMenu();
    }

    public void OnRestartButtonClicked()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reiniciar la escena
    }

    public void OnMainMenuButtonClicked()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainMenu"); // Cargar el menú principal
    }

    // Botones del menú de fin de juego
    public void OnGameOverRestartButtonClicked()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reiniciar la escena
    }

    public void OnGameOverMainMenuButtonClicked()
    {
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainMenu"); // Cargar el menú principal
    }
}