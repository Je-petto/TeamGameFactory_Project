using UnityEngine;

//점수 증가 아이템 클래스
public class CollectableScore : Collectable
{
    public int gainScore = 10;  //기본 점수 추가

    private Vector3 randomRotationAxis; //아이템 회전축
    public override void Awake()
    {
        base.Awake();
        randomRotationAxis = Random.insideUnitSphere.normalized;
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up;
            //무작위 회전축 설정하여 y축으로 설정
    }
    public override void Update()
    {
        base.Update();
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
        //data.rotationSpeed에 맞춰 아이템이 회전하도록 설정
        //Time.deltaTime 곱해서 일정한 회전속도 유지
    }
}
