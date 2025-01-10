using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int currentLevel = 1; // Nivel actual
    public int robbersPerLevel = 3; // Cantidad de robbers por nivel
    public GameObject robberPrefab; // Prefab del robber
    public Transform spawnArea; // Referencia al plano que define el área de spawn

    public int robbersCaught = 0; // Contador de robbers atrapados

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
        StartLevel();
    }

    void StartLevel()
    {
        robbersCaught = 0;
        SpawnRobbers();
    }

    void SpawnRobbers()
    {
        for (int i = 0; i < robbersPerLevel; i++)
        {
            Vector3 randomPosition = GetRandomPositionOnPlane();
            Instantiate(robberPrefab, randomPosition, Quaternion.identity);
        }
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
            // Calcular una posición aleatoria dentro del área del plano
            float randomX = Random.Range(-planeSize.x / 2, planeSize.x / 2);
            float randomZ = Random.Range(-planeSize.z / 2, planeSize.z / 2);
            randomPosition = new Vector3(randomX, 0, randomZ) + spawnArea.position;

            // Verificar si la posición está libre
            positionIsValid = !Physics.CheckSphere(randomPosition, 1.0f); // 1.0f es el radio de verificación
            attempts++;
        } while (!positionIsValid && attempts < 10); // Intentar hasta 10 veces

        if (!positionIsValid)
        {
            Debug.LogWarning("No se encontró una posición válida después de 10 intentos.");
        }

        return randomPosition;
    }

    public void RobberCaught()
    {
        robbersCaught++;
        if (robbersCaught >= robbersPerLevel)
        {
            Debug.Log("¡Nivel completado!");
            NextLevel();
        }
    }

    void NextLevel()
    {
        currentLevel++;
        robbersPerLevel++; // Más robbers

        // Buscar todos los Robbers en la escena sin un orden específico
        Robber[] robbers = FindObjectsByType<Robber>(FindObjectsSortMode.None);

        // Recorrer la lista de Robbers y aumentar su velocidad
        foreach (var robber in robbers)
        {
            UnityEngine.AI.NavMeshAgent agent = robber.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (agent != null)
            {
                agent.speed += 1.0f; // Aumentar velocidad
            }
        }

        StartLevel();
    }
}