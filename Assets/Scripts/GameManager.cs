using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 60.0f; 
    private float currentTime;
    public TextMeshProUGUI messageText; // Para mensajes como "¡Atrapa todos los ladrones!"
    public GameObject messagePanel; // Panel que contiene el mensaje
    public TextMeshProUGUI timerText; // Para mostrar el tiempo restante

    void Start()
    {
        currentTime = timeLimit;

        // Mostrar el mensaje inicial
        ShowMessage("¡Atrapa todos los ladrones!", 2.0f); // Muestra el mensaje durante 2 segundos

        // Inicializar el texto del temporizador
        UpdateTimerText();
    }

    void Update()
    {
        // Actualizar el tiempo restante
        currentTime -= Time.deltaTime;

        // Actualizar el texto del temporizador en cada frame
        UpdateTimerText();

        // Verificar si el tiempo se ha agotado
        if (currentTime <= 0)
        {
            currentTime = 0; // Asegurarse de que no sea negativo
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("¡Tiempo agotado! Game Over.");
        ShowMessage("¡Tiempo agotado! Game Over.", 3.0f); // Muestra el mensaje de Game Over durante 3 segundos
    }

    // Método para mostrar un mensaje durante un tiempo específico
    void ShowMessage(string message, float duration)
    {
        // Activar el panel y establecer el mensaje
        messagePanel.SetActive(true);
        messageText.text = message;

        // Desactivar el panel después de "duration" segundos
        Invoke("HideMessage", duration);
    }

    // Método para ocultar el panel
    void HideMessage()
    {
        messagePanel.SetActive(false);
    }

    // Método para actualizar el texto del temporizador
    void UpdateTimerText()
    {
        // Formatear el tiempo en minutos y segundos
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Actualizar el texto del temporizador
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}