using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleData data; //장애물의 속성을 담은 ObstacleData 외부에서 연결
    private Vector3 randomRotationAxis; //장애물 회전 방향
    void Awake() //장애물이 생성되면 실행 
    {
        randomRotationAxis = Random.insideUnitSphere.normalized; //무작위 방향 벡털르 뽑아 normalized
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up; //회전이 0이되면 안되니 zero는 예외처리 하고 y축(Vector3.up으로 설정)

        // Debug.Log($"장애물 생성! 회전 축: {randomRotationAxis.x:F2}, {randomRotationAxis.y:F2}, {randomRotationAxis.z:F2}");
    }
    void Update()
    {
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
        //data.rotationSpeed값에 따라 매 프레임마다 회전시킴
    }
}