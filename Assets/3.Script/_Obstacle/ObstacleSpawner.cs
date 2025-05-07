using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player; //플레이어
    public List<GameObject> obstaclePrefabs; // 생성할 옵스타클 프리팹

    public Transform SpawnObstacle; //생성된 옵스타클 프리팹이 들어갈 부모
    public Camera mainCamera; //메인 카메라
    public int spawnZ = 30; //카메라에서 소환될 거리 -> Z축(카메라의 전방)
    private float spawnInterval = 1f; // 기본 스폰 간격 (초)
    [Header("스폰 간격 숫자가 커지면 많이 소환됨)")] public float spawnRate = 1f; // 소환 빈도 조절 spawnRate가 높을수록 소환이 더 자주됨

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main; //카메라를 지정하지 않았을 경우, 메인 카메라를 찾아 설정

        StartCoroutine(SpawnRoutine()); //코루틴실행
        
    }

    private IEnumerator SpawnRoutine() //시간차별 소환 코루틴 메소드
    {
        while (true)
        {
            SpwanObstacle();
            float interval = spawnInterval / spawnRate;
            /*
            장애물이 많이 나오려면  인스펙터에서 숫자가 낮아져야 되니
            인스펙터에서 숫자를 키우면 많이 나오게끔 변수를 설정함 -> 편의성
            */
            yield return new WaitForSeconds(interval); //interval마다 소환
        }
    }

    private void SpwanObstacle() //장애물 일정한 간격으로 랜덤위치에 소환
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x); //x축 랜덤설정
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit); //y축 랜덤설정
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);

        GameObject spawnObj = CalculateWeight(); //가중치를 불러와 소환

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Count)];
        Instantiate(prefab, randomPos, Quaternion.identity, SpawnObstacle);
    }

    GameObject CalculateWeight() // 각 장애물 프리팹이 가진 가중치를 이용해 장애물을 생성
    {
        //maxtWeight = 각각의 장애물 프리팹의 가중치 총합을 구하기 위한 변수
        //curWeight = 각 프리팝의 weight를 누적해서 비교하기 위해 사용 
        float maxWeight = 0f, curWeight = 0f;
        //spawnObj = 조건에 맞는 오브젝트가 선택되었을떄 반환하기 위해 사용한 변수
        GameObject spawnObj = null;
        
        foreach (var obj in obstaclePrefabs) //모든 프리팹의 wieght 값을 더해 maxWeight(전체 확률범위)를 구함
            maxWeight += obj.GetComponent<Obstacle>().data.weight;
        float selectWeight = Random.Range(0, maxWeight); //0부터 maxWeight사이에서 무작위 수 선택
        foreach (var obj in obstaclePrefabs) //obstaclePrefabs를 돌면서 가중치를  누적 가중치가 selectWeight를 넘거나 같아지는 시점의 오브젝트를 선택 *룰렛 휠 알고리즘(Roulette Wheel Selection)
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