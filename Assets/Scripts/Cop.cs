using UnityEngine;

public class Cop : MonoBehaviour
{
  public float catchRadius = 2.0f; 

    void Update()
    {
        DetectRobbers();
    }

    void DetectRobbers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, catchRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("robber"))
            {
                Robber robber = hitCollider.GetComponent<Robber>();
                if (robber != null && !robber.isCaught)
                {
                    // Catch Rober
                    robber.isCaught = true;
                    LevelManager.Instance.RobberCaught();
                    Destroy(hitCollider.gameObject); 
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, catchRadius);
    }
}
