using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    public List<GameObject> obstaclePrefabs; // 생성할 옵스타클 프리팹

    public Transform SpawnObstacle; //생성된 옵스타클 프리팹이 들어갈 부모
    public Camera mainCamera;
    public int spawnZ = 30; //카메라에서 소환될 거리
    private float spawnInterval = 1f; // 스폰 간격 (초)
    [Header("스폰 간격-1초당 1프리펩씩)")]public float spawnRate = 1f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpwanObstacle();
             float interval = spawnInterval / spawnRate;
            yield return new WaitForSeconds(interval);
        }
    }

    private void SpwanObstacle()
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x);
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit);
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);

        GameObject spawnObj = CalculateWeight();

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        Instantiate(prefab, randomPos, Quaternion.identity, SpawnObstacle);
    }

    GameObject CalculateWeight()
    {
        float maxWeight = 0f, curWeight = 0f;
        GameObject spawnObj = null;
        foreach (var obj in obstaclePrefabs)
            maxWeight += obj.GetComponent<Obstacle>().data.weight;
        float selectWeight = Random.Range(0, maxWeight);
        foreach (var obj in obstaclePrefabs)
        {
            curWeight += obj.GetComponent<Obstacle>().data.weight;
            if (selectWeight <= curWeight)
            {
                spawnObj = obj;
                return spawnObj;
            } 
        }
        return null;
    }
}