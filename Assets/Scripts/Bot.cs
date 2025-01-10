using UnityEngine;
using UnityEngine.AI;

public class Bot : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target; // El objetivo (jugador con tag "Cop")
    Drive ds;
    public LayerMask ignoreLayers; // Asigna los layers que quieres ignorar en el Inspector

    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();

        // Buscar automáticamente al objeto con el tag "Cop" y asignarlo como target
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("cop");
        }

        if (target != null)
        {
            ds = target.GetComponent<Drive>();
        }
        else
        {
            Debug.LogError("No se encontró un objeto con el tag 'Cop'.");
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

    Vector3 wanderTarget = Vector3.zero;

    void Wander()
    {
        float wanderRadius = 10;
        float wanderDistance = 20;
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
        if (target == null) return;

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 10;

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
        if (target == null) return;

        float dist = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGO = World.Instance.GetHidingSpots()[0];

        for (int i = 0; i < World.Instance.GetHidingSpots().Length; i++)
        {
            Vector3 hideDir = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePos = World.Instance.GetHidingSpots()[i].transform.position + hideDir.normalized * 100;

            if (Vector3.Distance(this.transform.position, hidePos) < dist)
            {
                chosenSpot = hidePos;
                chosenDir = hideDir;
                chosenGO = World.Instance.GetHidingSpots()[i];
                dist = Vector3.Distance(this.transform.position, hidePos);
            }
        }

        Collider hideCol = chosenGO.GetComponent<Collider>();
        Ray backRay = new Ray(chosenSpot, -chosenDir.normalized);
        RaycastHit info;
        float distance = 250.0f;
        hideCol.Raycast(backRay, out info, distance);

        Seek(info.point + chosenDir.normalized * 2);
    }

    bool CanSeeTarget()
    {
        if (target == null) return false;

        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        float lookAngle = Vector3.Angle(this.transform.forward, rayToTarget);

        if (lookAngle < 60)
        {
            Debug.DrawRay(this.transform.position, rayToTarget, Color.red, 1.0f); // Dibuja el raycast en el Editor
            if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
            {
                if (raycastInfo.transform.gameObject.tag == "cop")
                {
                    Debug.Log("Te estoy viendo");
                    return true;
                }
            }
        }
        Debug.Log("No te estoy viendo");
        return false;
    }

    bool CanSeeMe()
    {
        if (target == null) return false;

        Vector3 rayToBot = this.transform.position - target.transform.position; // Dirección desde el jugador hacia el bot
        float lookAngle = Vector3.Angle(target.transform.forward, rayToBot);

        if (lookAngle < 60)
        {
            return true;
        }
        return false;
    }

    bool coolDown = false;

    void BehaviourCoolDown()
    {
        coolDown = false;
        Debug.Log("Cooldown terminado");
    }

    bool TargetInRange()
    {
        if (target == null) return false;

        if (Vector3.Distance(this.transform.position, target.transform.position) < 10)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (target == null)
        {
            // Si no hay target, buscar al jugador con el tag "Cop"
            target = GameObject.FindGameObjectWithTag("cop");
            if (target != null)
            {
                ds = target.GetComponent<Drive>();
            }
            return;
        }

        if (!coolDown)
        {
            if (!TargetInRange())
            {
                Wander();
            }
            else if (CanSeeTarget() && CanSeeMe())
            {
                Debug.Log("Escondiéndome...");
                CleverHide();
                coolDown = true;
                Invoke("BehaviourCoolDown", 5); // Reinicia el cooldown después de 5 segundos
            }
            else
            {
                Debug.Log("Buscando un lugar para esconderme...");
                Hide();
            }
        }
    }
}