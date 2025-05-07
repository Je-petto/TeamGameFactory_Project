using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템을 먹고 나서 화면의 모든 장애물을 삭제
public class CollectableDobs : Collectable
{
    private Vector3 randomRotationAxis; //회전


    public override void Awake()
    {
        base.Awake();//Collectable에 있는 Awake 소환환
        randomRotationAxis = Random.insideUnitSphere.normalized; //무작위 회전축 설정
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up; //회전이 0이되면 안되므로 y축으로 설정
    }
    public override void Update()
    {
        base.Update();
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
        //data.rotationSpeed에 맞춰 아이템이 회전하도록 설정
        //Time.deltaTime 곱해서 일정한 회전속도 유지
    }
    public override void ClearObstacles() //Collectable에서 상속받아 override한걸 여기서 구현
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); //태그가 obstacle인지 확인

        foreach (GameObject obstacle in obstacles) //찾아낸 list의 obstacle을 모두 순회
        {
            Destroy(obstacle); //찾아낸 모든 obstacle 삭제
        }
    }
}