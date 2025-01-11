using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject copPrefab;
    public GameObject robberPrefab;
    public GameObject moneyPrefab;
    public Transform spawnArea;

    // Nueva variable para la posición de spawn del jugador
    public Transform playerSpawnPosition;

    public int robbersPerLevel = 3;
    public int moneyPerLevel = 5;
    public int copsPerLevel = 2;

    private int robbersLeft; // Ladrones restantes
    private int moneyLeft;   // Dinero recolectado

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

    public void InitializeLevel(int level)
    {
        // Configurar la dificultad según el nivel
        robbersPerLevel = 3 + level;
        moneyPerLevel = 5 + level;
        copsPerLevel = 2 + level;

        // Inicializar ladrones restantes y dinero recolectado
        robbersLeft = robbersPerLevel;
        moneyLeft = 0;

        // Limpiar el nivel anterior (opcional)
        ClearLevel();

        // Inicializar el nivel según el rol del jugador
        if (GameSettings.Instance.PlayerRole == PlayerRole.Cop)
        {
            InitializeCopMode();
        }
        else
        {
            InitializeRobberMode();
        }
    }

    void InitializeCopMode()
    {
        // Spawnear al jugador como policía
        GameObject player = Instantiate(copPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
        AssignPlayerScript(player);

        // Spawnear robbers bots
        for (int i = 0; i < robbersPerLevel; i++)
        {
            GameObject robber = Instantiate(robberPrefab, GetRandomPositionOnPlane(), Quaternion.identity);
            AssignBotScript(robber, Bot.BotRole.Robber); // Assign bot script to robbers
        }
    }

    void InitializeRobberMode()
    {
        // Spawnear al jugador como ladrón en la posición de spawn
        GameObject player = Instantiate(robberPrefab, playerSpawnPosition.position, playerSpawnPosition.rotation);
        AssignPlayerScript(player);

        // Spawnear dinero
        for (int i = 0; i < moneyPerLevel; i++)
        {
            Instantiate(moneyPrefab, GetRandomPositionOnPlane(), Quaternion.identity);
        }

        // Spawnear cops bots
        for (int i = 0; i < copsPerLevel; i++)
        {
            GameObject cop = Instantiate(copPrefab, GetRandomPositionOnPlane(), Quaternion.identity);
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

    Vector3 GetRandomPositionOnPlane()
    {
        if (spawnArea == null)
        {
            Debug.LogError("No se ha asignado un área de spawn.");
            return Vector3.zero;
        }

        Vector3 planeSize = spawnArea.localScale * 10;
        Vector3 randomPosition;
        bool positionIsValid = false;
        int attempts = 0;

        do
        {
            float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2);
            randomPosition = new Vector3(randomX, 0, randomZ) + spawnArea.position;

            positionIsValid = !Physics.CheckSphere(randomPosition, 1.0f);
            attempts++;
        } while (!positionIsValid && attempts < 10);

        if (!positionIsValid)
        {
            Debug.LogWarning("No se encontró una posición válida después de 10 intentos.");
        }

        return randomPosition;
    }

    void ClearLevel()
    {
        // Destruir todos los objetos del nivel anterior (opcional)
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Robber"))
        {
            Destroy(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Cop"))
        {
            Destroy(obj);
        }
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Money"))
        {
            Destroy(obj);
        }
    }

    public void RobberCaught()
    {
        robbersLeft--; // Restar un ladrón
        Debug.Log("Robbers Left: " + robbersLeft);

        // Actualizar el Canvas (si tienes un UIManager)
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateRobbersLeft(robbersLeft);
        }

        // Verificar si todos los ladrones han sido atrapados
        if (robbersLeft <= 0)
        {
            UIManager.Instance.ShowMessage("¡Todos los ladrones han sido atrapados!", 2);
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
        }

        // Verificar si se ha recolectado todo el dinero
        if (moneyLeft >= moneyPerLevel)
        {
            UIManager.Instance.ShowMessage("¡Has recolectado todo el dinero!", 2);
            GameManager.Instance.LevelComplete(); // Subir de nivel
            moneyLeft = 0;
            UIManager.Instance.UpdateMoneyUI(moneyLeft);
        }
    }
}