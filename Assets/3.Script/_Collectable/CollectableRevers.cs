using UnityEngine;

//이동반전 아이템
public class CollectableRevers : Collectable
{
    private Vector3 randomRotationAxis; //회전
    public float reverseDuration = 3f; // 리버스 시간 3초 유지

    public override void Awake()
    {
        base.Awake();
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

    public override void Reverse(PlayerBehaviour player) //플레이어를 받아서 플레이어에 적용시킴
    {
        player.StartCoroutine(player.ReverseMovement(reverseDuration));
        //코루틴으로 지속시간만큼 적용시켜서 지정된 시간이 지나면 다시 원래대로 돌아가게 함
    }
}