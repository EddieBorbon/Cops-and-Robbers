using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    public enum BotRole { Cop, Robber }
    public BotRole botRole;

    private NavMeshAgent agent;
    private Transform player;
    private Player playerScript; // Referencia al script del jugador

    private Vector3 wanderTarget = Vector3.zero;
    private bool coolDown = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Busca al jugador al inicio
        FindPlayer();

        // Inicia el comportamiento correspondiente según el rol
        if (botRole == BotRole.Robber)
        {
            InvokeRepeating("RobberBehaviour", 0f, 1f); // Comportamiento del ladrón
        }
        else if (botRole == BotRole.Cop)
        {
            InvokeRepeating("CopBehaviour", 0f, 1f); // Comportamiento del policía
        }
    }

    void FindPlayer()
    {
        playerScript = FindFirstObjectByType<Player>();
        if (playerScript != null)
        {
            player = playerScript.transform;
        }
        else
        {
            Debug.LogWarning("Player not found! Make sure there is a GameObject with the Player script in the scene.");
        }
    }

    void RobberBehaviour()
    {
        if (player == null) return;

        if (CanSeeTarget() && TargetCanSeeMe())
        {
            CleverHide(); // Si el jugador ve al ladrón, este se esconde
            coolDown = true;
            Invoke("ResetCoolDown", 5); // Espera 5 segundos antes de hacer otra acción
        }
        else if (TargetInRange())
        {
            Evade(); // Si el jugador está cerca, el ladrón huye
        }
        else
        {
            Wander(); // Si no hay amenaza, el ladrón deambula
        }
    }

    void CopBehaviour()
    {
        if (player == null) return;

        if (CanSeeTarget())
        {
            Pursue(); // Si el policía ve al jugador, lo persigue
        }
        else if (TargetInRange())
        {
            Seek(player.position); // Si el jugador está cerca pero no visible, se mueve hacia su última posición conocida
        }
        else
        {
            Wander(); // Si no hay rastro del jugador, el policía deambula
        }
    }

    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location)
    {
        Vector3 fleeVector = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeVector);
    }

    void Pursue()
    {
        if (player == null) return;

        Vector3 targetDir = player.position - this.transform.position;
        Seek(player.position + targetDir.normalized * 2); // Persigue al jugador con un pequeño margen de predicción
    }

    void Evade()
    {
        if (player == null) return;

        Vector3 targetDir = player.position - this.transform.position;
        Flee(player.position + targetDir.normalized * 2); // Huye del jugador con un pequeño margen de predicción
    }

    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1.0f, 1.0f) * wanderJitter, 0, Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
    }

    void Hide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            Vector3 hideDir = spot.transform.position - player.position;
            Vector3 hidePos = spot.transform.position + hideDir.normalized * 10;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Seek(chosenSpot);
    }

    void CleverHide()
    {
        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        foreach (GameObject spot in World.Instance.GetHidingSpots())
        {
            Vector3 hideDir = spot.transform.position - player.position;
            Vector3 hidePos = spot.transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = spot;
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized);
    }

    bool CanSeeTarget()
    {
        if (player == null) return false;

        RaycastHit raycastInfo;
        Vector3 rayToTarget = player.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            if (raycastInfo.transform.gameObject.tag == "Player")
                return true;
        }
        return false;
    }

    bool TargetCanSeeMe()
    {
        if (player == null) return false;

        Vector3 toAgent = this.transform.position - player.position;
        float lookingAngle = Vector3.Angle(player.forward, toAgent);

        if (lookingAngle < 60)
            return true;
        return false;
    }

    void ResetCoolDown()
    {
        coolDown = false;
    }

    bool TargetInRange()
    {
        if (player == null) return false;

        if (Vector3.Distance(this.transform.position, player.position) < playerScript.detectionRange)
            return true;
        return false;
    }
}