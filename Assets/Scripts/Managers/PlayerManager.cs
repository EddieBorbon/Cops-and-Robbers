using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject copPrefab;
    public GameObject robberPrefab;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector2 spawnAreaSize = new Vector2(50, 50);

    private GameObject player;

    public void SpawnPlayer()
    {
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

            AssignPlayerScript(player);
        }
    }

    public void MovePlayerToSpawn()
    {
        if (player != null)
        {
            player.transform.position = GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize);
        }
        else
        {
            Debug.LogWarning("No se encontró un jugador para mover.");
        }
    }

    // Método para obtener la transformación del jugador
    public Transform GetPlayerTransform()
    {
        if (player != null)
        {
            return player.transform;
        }
        else
        {
            Debug.LogWarning("No se encontró un jugador para obtener su transformación.");
            return null;
        }
    }

    private void AssignPlayerScript(GameObject character)
    {
        Player playerScript = character.AddComponent<Player>();
        playerScript.role = (GameSettings.Instance.PlayerRole == PlayerRole.Cop) ? Player.PlayerRole.Cop : Player.PlayerRole.Robber;
    }

    private Vector3 GetRandomPositionOnPlane(Vector3 center, Vector2 size)
    {
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomZ = Random.Range(-size.y / 2, size.y / 2);
        return new Vector3(randomX, 0, randomZ) + center;
    }
}