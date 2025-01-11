using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public enum BotRole { Cop, Robber }
    public BotRole botRole;

    private NavMeshAgent agent;
    private Transform player;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Busca al jugador al inicio
        FindPlayer();

        // Inicia el comportamiento correspondiente
        if (botRole == BotRole.Robber)
        {
            InvokeRepeating("FindHidingSpot", 0f, 5f);
        }
        else if (botRole == BotRole.Cop)
        {
            InvokeRepeating("ChasePlayer", 0f, 1f);
        }
    }

    void FindPlayer()
    {
        // Busca al jugador por su script
        Player playerScript = FindFirstObjectByType<Player>();
        if (playerScript != null)
        {
            player = playerScript.transform;
        }
        else
        {
            //Debug.LogWarning("Player not found! Make sure there is a GameObject with the Player script in the scene.");
        }
    }

    void FindHidingSpot()
    {
        GameObject[] hidingSpots = World.Instance.GetHidingSpots();
        GameObject closestSpot = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject spot in hidingSpots)
        {
            float distance = Vector3.Distance(transform.position, spot.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestSpot = spot;
            }
        }

        if (closestSpot != null)
        {
            agent.SetDestination(closestSpot.transform.position);
        }
    }

    void ChasePlayer()
    {
        if (player != null) // Verifica que el jugador esté asignado
        {
            agent.SetDestination(player.position);
        }
        else
        {
            //Debug.LogWarning("Player reference is null. Trying to find player again...");
            FindPlayer(); // Intenta encontrar al jugador nuevamente
        }
    }
}