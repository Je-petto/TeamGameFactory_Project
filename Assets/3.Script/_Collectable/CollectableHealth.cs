using UnityEngine;

public class CollectableHealth : Collectable
{
    public int gainHealth = 10; //기본 회복량
    private Vector3 randomRotationAxis; //회전
    public override void Awake() //Collectable에서 상속받아 override해서 구현
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
}
