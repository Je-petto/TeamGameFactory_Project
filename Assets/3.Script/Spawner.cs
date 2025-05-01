using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    public List<GameObject> obstaclePrefabs; // 생성할 옵스타클 프리팹

    public Transform SpawnObstacle; //생성된 옵스타클 프리팹이 들어갈 부모
    public Camera mainCamera;
    public int spawnZ = 30; //카메라에서 소환될 거리
    [Tooltip("초(sec)")] public float spawnInterval = 1f; // 스폰 간격 (초)

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        StartCoroutine(SpawnItemRoutine());
    }

    private IEnumerator SpawnItemRoutine()
    {
        while (true)
        {
            SpwanObstacle();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpwanObstacle()
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x);
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit);
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);


        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        Instantiate(prefab, randomPos, Quaternion.identity, SpawnObstacle);
    }
}