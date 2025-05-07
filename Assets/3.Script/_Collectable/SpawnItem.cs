using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{
    [SerializeField] PlayerBehaviour player;
    public List<GameObject> itemPrefabs; //생성할 아이템 프리팹

    public Transform SpawnItems; //생성된 아이템 프리팹이 들어갈 부모 

    public Camera mainCamera; //메인 카메라
    public int spawnZ = 30; //카메라에서 소환될 거리


    public void SpwanItem()
    {
        float randomXAxis = Random.Range(player.movementLimits.x, player.movementLimits.width + player.movementLimits.x);
        float randomYAxis = Random.Range(-player.yAxisLimit, player.yAxisLimit);

        //Player 이동 반경만큼의 소환 좌표 좌표 (화면 내)
        Vector3 randomPos = new Vector3(randomXAxis, randomYAxis, spawnZ);

        //Viewport 좌표를 월드 좌표로 변환 (z는 카메라에서의 거리)
        //Vector3 worldPos = mainCamera.ViewportToWorldPoint(new Vector3(randomViewportPos.x, randomViewportPos.y, spawnZ));

        //아이템 생성 프리팹을 부모 오브젝트 밑에 생성
        GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
        Instantiate(prefab, randomPos, Quaternion.identity, SpawnItems);

    }
}
