using UnityEngine;

public class Robber : MonoBehaviour
{
    public bool isCaught = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("cop") && !isCaught)
        {
            isCaught = true;
            LevelManager.Instance.RobberCaught();
            Destroy(gameObject); 
        }
    }
}
