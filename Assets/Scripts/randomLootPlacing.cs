using UnityEngine;
using System.Collections.Generic;

public class LootableSpawnManager : MonoBehaviour
{
    [System.Serializable]
    public class LootableObject
    {
        public GameObject prefab;
        [Range(0, 100)] public float spawnChance; // Objeler i�in kullan�lacak, y�zdelik spawn oran�
    }

    public List<Transform> spawnPoints; // spawn noktalar�
    public List<LootableObject> lootableObjects; // lootable prefablar�m�n listesi
    public int minLootCount = 3;
    public int maxLootCount = 5;

    void Start()
    {
        SpawnLootables();
    }

    void SpawnLootables()
    {
        // Spawn noktalar�n� kar��t�r
        List<Transform> shuffledPoints = new List<Transform>(spawnPoints);
        ShuffleList(shuffledPoints);

        int lootToSpawn = Random.Range(minLootCount, maxLootCount + 1);
        int spawned = 0;

        foreach (Transform point in shuffledPoints)
        {
            if (spawned >= lootToSpawn) break;

            GameObject chosen = GetRandomLoot();
            if (chosen != null)
            {
                Instantiate(chosen, point.position, Quaternion.identity);
                spawned++;
            }
        }
    }

    GameObject GetRandomLoot()
    {
        float totalChance = 0f;
        foreach (var loot in lootableObjects)
            totalChance += loot.spawnChance;

        float roll = Random.Range(0, totalChance);
        float sum = 0f;

        foreach (var loot in lootableObjects)
        {
            sum += loot.spawnChance;
            if (roll <= sum)
                return loot.prefab;
        }

        return null; // hi�biri gelmezse
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
