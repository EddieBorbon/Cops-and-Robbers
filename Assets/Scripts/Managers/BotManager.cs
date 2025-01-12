using UnityEngine;

public class BotManager : MonoBehaviour
{
    public GameObject copPrefab;
    public GameObject robberPrefab;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector2 spawnAreaSize = new Vector2(50, 50);

    public void SpawnBots(int robbersPerLevel, int copsPerLevel)
    {
        for (int i = 0; i < robbersPerLevel; i++)
        {
            GameObject robber = Instantiate(robberPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            AssignBotScript(robber, Bot.BotRole.Robber);
        }

        for (int i = 0; i < copsPerLevel; i++)
        {
            GameObject cop = Instantiate(copPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
            AssignBotScript(cop, Bot.BotRole.Cop);
        }
    }

    private void AssignBotScript(GameObject character, Bot.BotRole role)
    {
        Bot botScript = character.AddComponent<Bot>();
        UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();

        if (agent == null)
        {
            agent = character.AddComponent<UnityEngine.AI.NavMeshAgent>();
        }

        botScript.botRole = role;
    }

    private Vector3 GetRandomPositionOnPlane(Vector3 center, Vector2 size)
    {
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomZ = Random.Range(-size.y / 2, size.y / 2);
        return new Vector3(randomX, 0, randomZ) + center;
    }
}