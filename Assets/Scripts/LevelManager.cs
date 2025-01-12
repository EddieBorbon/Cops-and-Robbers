using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject copPrefab;
    public GameObject robberPrefab;
    public GameObject moneyPrefab;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector2 spawnAreaSize = new Vector2(50, 50);

    // Variable para la cámara de tercera persona
    public Camera thirdPersonCamera;

    public int robbersPerLevel = 3;
    public int moneyPerLevel = 5;
    public int copsPerLevel = 2;

    private int robbersLeft; // Ladrones restantes
    private int moneyLeft;   // Dinero recolectado

    private GameObject player; // Referencia al jugador

    // Referencia al Panel de Game Over
    public GameObject gameOverPanel;

    // Variables para el timer
    public float timerDuration = 60f; // Duración del timer en segundos
    public float currentTime; // Tiempo restante
    private bool isGameStarted = false; // Estado del juego

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Spawnear al jugador solo una vez al inicio del juego
        SpawnPlayer();
        // Asegurarse de que la pantalla de Game Over esté desactivada al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        currentTime = timerDuration;
        UpdateTimerUI(); // Actualizar el texto del timer al inicio
        InitializeLevel(GameManager.Instance.currentLevel);
    }

    public void InitializeLevel(int level)
    {
        // Configurar la dificultad según el nivel
        robbersPerLevel = 3 + level;
        moneyPerLevel = 5 + level;
        copsPerLevel = 2 + level;

        // Inicializar ladrones restantes y dinero recolectado
        robbersLeft = robbersPerLevel;
        moneyLeft = 0;

        // Mover al jugador a la posición de spawn del nuevo nivel
        MovePlayerToSpawn();

        // Inicializar el nivel según el rol del jugador
        if (GameSettings.Instance.PlayerRole == PlayerRole.Cop)
        {
            InitializeCopMode();
        }
        else
        {
            InitializeRobberMode();
        }

        // Reasignar la cámara al jugador
        if (player != null)
        {
            SetupCameraAsChild(player.transform);
        }
        else
        {
            Debug.LogWarning("No se encontró un jugador para asignar la cámara.");
        }
        // Comenzar el juego y el timer
        StartGame();
    }

    void SpawnPlayer()
    {
        // Spawnear al jugador solo si no existe
        if (player == null)
        {
            if (GameSettings.Instance.PlayerRole == PlayerRole.Cop)
            {
                player = Instantiate(copPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            }
            else
            {
                player = Instantiate(robberPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            }

            // Asignar el script Player
            AssignPlayerScript(player);

            // Configurar la cámara como hija del jugador con el offset
            SetupCameraAsChild(player.transform);
        }
    }

    void MovePlayerToSpawn()
    {
        if (player != null)
        {
            // Mover al jugador a una posición aleatoria en el área de spawn
            player.transform.position = GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize);
        }
        else
        {
            Debug.LogWarning("No se encontró un jugador para mover.");
        }
    }

    void InitializeCopMode()
    {
        // Spawnear robbers bots
        for (int i = 0; i < robbersPerLevel; i++)
        {
            GameObject robber = Instantiate(robberPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            AssignBotScript(robber, Bot.BotRole.Robber); // Assign bot script to robbers
        }
    }

    void InitializeRobberMode()
    {
        // Spawnear dinero
        for (int i = 0; i < moneyPerLevel; i++)
        {
            Instantiate(moneyPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
        }

        // Spawnear cops bots
        for (int i = 0; i < copsPerLevel; i++)
        {
            GameObject cop = Instantiate(copPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            AssignBotScript(cop, Bot.BotRole.Cop); // Assign bot script to cops
        }
    }

    void AssignPlayerScript(GameObject character)
    {
        // Asignar el script Player
        Player playerScript = character.AddComponent<Player>();

        // Configurar el rol del jugador
        playerScript.role = (GameSettings.Instance.PlayerRole == PlayerRole.Cop) ? Player.PlayerRole.Cop : Player.PlayerRole.Robber;
    }

    void AssignBotScript(GameObject character, Bot.BotRole role)
    {
        // Asignar el script Bot y configurar el NavMeshAgent
        Bot botScript = character.AddComponent<Bot>();
        UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            agent = character.AddComponent<UnityEngine.AI.NavMeshAgent>();
        }

        // Configurar el rol del bot
        botScript.botRole = role;
    }

    Vector3 GetRandomPositionOnPlane(Vector3 spawnAreaCenter, Vector2 spawnAreaSize)
    {
        if (spawnAreaSize.x <= 0 || spawnAreaSize.y <= 0)
        {
            Debug.LogError("El tamaño del área de spawn no puede ser negativo o cero.");
            return Vector3.zero;
        }

        Vector3 randomPosition;
        bool positionIsValid = false;
        int attempts = 0;

        do
        {
            float randomX = Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2);
            float randomZ = Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2);
            randomPosition = new Vector3(randomX, 0, randomZ) + spawnAreaCenter;

            positionIsValid = !Physics.CheckSphere(randomPosition, 1.0f);
            attempts++;
        } while (!positionIsValid && attempts < 10);

        if (!positionIsValid)
        {
            Debug.LogWarning("No se encontró una posición válida después de 10 intentos.");
        }

        return randomPosition;
    }

    private void OnDrawGizmos()
    {
        // Define el color del Gizmo
        Gizmos.color = Color.green;

        // Dibuja un wireframe (marco) del área de spawn
        Gizmos.DrawWireCube(spawnAreaCenter, new Vector3(spawnAreaSize.x, 0.1f, spawnAreaSize.y));
    }

    public void RobberCaught()
    {
        robbersLeft--; // Restar un ladrón
        Debug.Log("Robbers Left: " + robbersLeft);

        // Actualizar el Canvas (si tienes un UIManager)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateRobbersLeft(robbersLeft);
            SoundManager.Instance.PlayRobberCaughtSound();
        }

        // Verificar si todos los ladrones han sido atrapados
        if (robbersLeft <= 0)
        {
            SoundManager.Instance.PlayNextLevelSound();
            GameManager.Instance.LevelComplete(); // Subir de nivel
            robbersLeft = robbersPerLevel;
            UIManager.Instance.UpdateMoneyUI(moneyLeft);
        }
    }

    public void MoneyCaught()
    {
        moneyLeft++; // Incrementar el dinero recolectado
        Debug.Log("Dinero recolectado: " + moneyLeft);

        // Actualizar el Canvas (si tienes un UIManager)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateMoneyUI(moneyLeft);
            SoundManager.Instance.PlayMoneyCollectedSound(); // Reproducir sonido al recolectar dinero
        }

        // Verificar si se ha recolectado todo el dinero
        if (moneyLeft >= moneyPerLevel)
        {
            SoundManager.Instance.PlayNextLevelSound();
            GameManager.Instance.LevelComplete(); // Subir de nivel
            moneyLeft = 0;
            UIManager.Instance.UpdateMoneyUI(moneyLeft);
        }
    }

    void SetupCameraAsChild(Transform playerTransform)
    {
        if (thirdPersonCamera != null)
        {
            // Asegurarse de que la cámara esté activa
            if (!thirdPersonCamera.gameObject.activeSelf)
            {
                thirdPersonCamera.gameObject.SetActive(true);
            }

            // Hacer que la cámara sea hija del jugador
            thirdPersonCamera.transform.SetParent(playerTransform);

            // Aplicar el offset de posición (0, 2, -3)
            thirdPersonCamera.transform.localPosition = new Vector3(0, 2, -3);

            // Aplicar el offset de rotación (-25, 0, 0)
            thirdPersonCamera.transform.localRotation = Quaternion.Euler(25, 0, 0);
        }
        else
        {
            Debug.LogWarning("No se ha asignado una cámara de tercera persona.");
        }
    }
    public void GameOver()
    {
        // Activar la pantalla de Game Over
        if (gameOverPanel != null)
        {
            SoundManager.Instance.PlayGameOverSound();
            gameOverPanel.SetActive(true);
        }

        // Pausar el juego (opcional)
        Time.timeScale = 0;
    }
    public void StartGame()
    {
        // Activar el estado de juego y comenzar el timer
        isGameStarted = true;
    }
    public void RestartGame()
    {
        // Reiniciar la escena principal
        Time.timeScale = 1; // Reanudar el tiempo
        SceneManager.LoadScene("MainScene"); // Cambia "MainScene" por el nombre de tu escena principal
    }
    void UpdateTimerUI()
    {
        // Usar el UIManager para actualizar el texto del timer
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateTimerUI(currentTime);
        }
    }
    void Update()
    {
        // Actualizar el timer solo si el juego ha comenzado
        if (isGameStarted && currentTime > 0)
        {
            currentTime -= Time.deltaTime; // Reducir el tiempo
            UpdateTimerUI(); // Actualizar el texto del timer

            // Si el tiempo llega a 0, activar el Game Over
            if (currentTime <= 0)
            {
                currentTime = 0; // Asegurarse de que no sea negativo
                GameOver();
            }
        }
    }

}