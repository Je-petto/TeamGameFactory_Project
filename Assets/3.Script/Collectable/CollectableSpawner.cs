using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    public List<GameObject> collectablePrefabs; // 생성할 Collectable 프리팹

    public Transform SpawnObstacle; //생성된 옵스타클 프리팹이 들어갈 부모
    public Camera mainCamera;
    public int spawnZ = 30; //카메라에서 소환될 거리
    [Tooltip("초(sec)")] public float spawnInterval = 1f; // 스폰 간격 (초)

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
            SpwanCollectable();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpwanCollectable()
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x);
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit);
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);

        GameObject spawnObj = CalculateWeight();
        
        Instantiate(spawnObj, randomPos, Quaternion.identity, SpawnObstacle);
    }
    GameObject CalculateWeight()
    {
        float maxWeight = 0f, curWeight = 0f;
        GameObject spawnObj = null;
        foreach (var obj in collectablePrefabs)
            maxWeight += obj.GetComponent<Collectable>().data.weight;
        float selectWeight = Random.Range(0, maxWeight);
        foreach (var obj in collectablePrefabs)
        {
            curWeight += obj.GetComponent<Collectable>().data.weight;
            if (selectWeight <= curWeight)
            {
                spawnObj = obj;
                return spawnObj;
            } 
        }
        return null;
    }
}