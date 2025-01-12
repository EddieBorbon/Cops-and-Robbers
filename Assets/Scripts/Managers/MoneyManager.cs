using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public GameObject moneyPrefab;
    public Vector3 spawnAreaCenter = Vector3.zero;
    public Vector2 spawnAreaSize = new Vector2(50, 50);

    public void SpawnMoney(int moneyPerLevel)
    {
        for (int i = 0; i < moneyPerLevel; i++)
        {
            Instantiate(moneyPrefab, GetRandomPositionOnPlane(spawnAreaCenter, spawnAreaSize), Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionOnPlane(Vector3 center, Vector2 size)
    {
        float randomX = Random.Range(-size.x / 2, size.x / 2);
        float randomZ = Random.Range(-size.y / 2, size.y / 2);
        return new Vector3(randomX, 0, randomZ) + center;
    }
}