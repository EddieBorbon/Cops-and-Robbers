using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerRole
    {
        Cop,
        Robber
    }

    public PlayerRole role; // Rol del jugador (Cop o Robber)
    public float moveSpeed = 5.0f; // Velocidad de movimiento del jugador
    public float rotationSpeed = 10.0f; // Velocidad de rotación del jugador
    public float detectionRange = 10.0f; // Rango de detección del objetivo
    public float catchRange = 2.0f; // Rango para atrapar o interactuar
    public LayerMask targetLayer; // Capa para detectar objetivos

    private GameObject target;
    private bool isChasing = false;
    private bool coolDown = false;
    private bool canPickupMoney = true; // Cooldown para recoger dinero
    private bool canCatchRobber = true; // Cooldown para capturar Robbers

    void Start()
    {
        // Asignar el objetivo según el rol del jugador
        if (role == PlayerRole.Cop)
        {
            target = GameObject.FindGameObjectWithTag("Robber"); // Buscar un robber
        }
        else if (role == PlayerRole.Robber)
        {
            target = GameObject.FindGameObjectWithTag("Cop"); // Buscar un cop
        }

        if (target == null)
        {
           // Debug.LogWarning("No se encontró un objetivo para el jugador.");
        }
    }

    void Update()
    {
        HandleMovement();

        if (target == null) return;

        // Calcular la distancia al objetivo
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // Comportamiento según el rol del jugador
        if (role == PlayerRole.Cop)
        {
            HandleCopBehavior(distanceToTarget);
        }
        else if (role == PlayerRole.Robber)
        {
            HandleRobberBehavior(distanceToTarget);
        }
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized;

        if (movement != Vector3.zero)
        {
            // Rotar hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Mover al jugador
            transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        }
    }

    void HandleCopBehavior(float distanceToTarget)
    {
        if (distanceToTarget <= detectionRange)
        {
            // Perseguir al objetivo
            isChasing = true;

            // Intentar atrapar al objetivo
            if (distanceToTarget <= catchRange && canCatchRobber)
            {
                CatchTarget();
            }
        }
        else
        {
            // Dejar de perseguir
            if (isChasing)
            {
                isChasing = false;
            }
        }
    }

    void HandleRobberBehavior(float distanceToTarget)
    {
        if (!coolDown)
        {
            if (distanceToTarget <= detectionRange)
            {
                if (CanSeeTarget() && CanSeeMe())
                {
                    //Debug.Log("Escondiéndome...");
                    coolDown = true;
                    Invoke("BehaviourCoolDown", 5); // Reiniciar el cooldown después de 5 segundos
                }
            }
        }
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
                if (raycastInfo.transform.gameObject.tag == "Cop" && role == PlayerRole.Robber)
                {
                    //Debug.Log("Te estoy viendo");
                    return true;
                }
            }
        }
      //  Debug.Log("No te estoy viendo");
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

    void BehaviourCoolDown()
    {
        coolDown = false;
       // Debug.Log("Cooldown terminado");
    }

    void CatchTarget()
    {
        if (role == PlayerRole.Cop && canCatchRobber)
        {
            canCatchRobber = false; // Activar cooldown
          //  Debug.Log("¡Robber atrapado!");
            LevelManager.Instance.RobberCaught(); // Notificar al LevelManager
            Destroy(target); // Eliminar al robber
            Invoke("ResetRobberCatch", 1.0f); // Reactivar la captura después de 1 segundo
        }
    }

    void ResetRobberCatch()
    {
        canCatchRobber = true; // Reactivar la capacidad de capturar Robbers
    }

    void OnDrawGizmosSelected()
    {
        // Dibujar el rango de detección y atrapar en el Editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, catchRange);
    }

    void OnTriggerEnter(Collider other)
    {
        // Verificar si el jugador entró en un trigger con el tag "Money" o "Robber"
        if (other.CompareTag("Money") && canPickupMoney && role == PlayerRole.Robber)
        {
            canPickupMoney = false; // Activar cooldown
            LevelManager.Instance.MoneyCaught(); // Notificar al LevelManager
            Destroy(other.gameObject); // Destruir el objeto de dinero
            Invoke("ResetMoneyPickup", 1.0f); // Reactivar la recolección después de 1 segundo
        }
        else if (other.CompareTag("Robber") && role == PlayerRole.Cop && canCatchRobber)
        {
            canCatchRobber = false; // Activar cooldown
            LevelManager.Instance.RobberCaught(); // Notificar al LevelManager
            Destroy(other.gameObject); // Destruir al robber
            Invoke("ResetRobberCatch", 1.0f); // Reactivar la captura después de 1 segundo
        }
    }

    void ResetMoneyPickup()
    {
        canPickupMoney = true; // Reactivar la capacidad de recoger dinero
    }
}