using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Collectable 생성용 스크립트
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
        //카메라가 연결되지 않으면 메인카메라를 연결
        //코루틴으로 호출하여 아이템을 일정 간격으로 생성함
    }

    //아이템을 일정 간격으로 생성
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpwanCollectable();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    //아이템 소환
    private void SpwanCollectable()
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x);
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit);
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);
        //플레이어 이동 제한 범위 내에서 랜덤한 위치에 생성

        GameObject spawnObj = CalculateWeight(); //가중치에 따른 아이템 소환
        
        Instantiate(spawnObj, randomPos, Quaternion.identity, SpawnObstacle);
    }

    //아이템 소환 가중치 설정
    // 각 장애물 프리팹이 가진 가중치를 이용해 장애물을 생성
    GameObject CalculateWeight() 
    {
        //maxtWeight = 각각의 장애물 프리팹의 가중치 총합을 구하기 위한 변수
        //curWeight = 각 프리팝의 weight를 누적해서 비교하기 위해 사용 
        float maxWeight = 0f, curWeight = 0f;
        GameObject spawnObj = null;  //spawnObj = 조건에 맞는 오브젝트가 선택되었을떄 반환하기 위해 사용한 변수
        foreach (var obj in collectablePrefabs) //모든 프리팹의 wieght 값을 더해 maxWeight(전체 확률범위)를 구함
            maxWeight += obj.GetComponent<Collectable>().data.weight;
        float selectWeight = Random.Range(0, maxWeight); //0부터 maxWeight사이에서 무작위 수 선택
        foreach (var obj in collectablePrefabs) //obstaclePrefabs를 돌면서 가중치를  누적 가중치가 selectWeight를 넘거나 같아지는 시점의 오브젝트를 선택 *룰렛 휠 알고리즘(Roulette Wheel Selection)
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
/*
룰렛 휠 알고리즘의 이란?

확률에 따라 룰렛 바퀴를 돌려 당첨 영역에 걸리는 원리

정해진 전체 범위 내에서 오브젝트별 할당된 범위만큼 당첨 확률이 높아짐

예시
오브젝트      Weight      누적 가중치
A               1            1
B               3            4
C               6            10
selectWeight 가 2.5면 A는 weight가 1이라서 2.5를 넘지 못하지만 B가 4 안쪽에 있으므로 B를 선택


*/