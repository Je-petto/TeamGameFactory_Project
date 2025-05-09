using UnityEngine;

public abstract class ScrollableObject : MonoBehaviour
{
    protected Vector3 randomRotationAxis; // 장애물 회전 방향
    void Awake() // 장애물이 생성되면 실행 
    {
        randomRotationAxis = Random.insideUnitSphere.normalized; // 무작위 방향 벡털르 뽑아 normalized
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up; // 회전이 0이되면 안되니 zero는 예외처리 하고 y축(Vector3.up으로 설정)

        // Debug.Log($"장애물 생성! 회전 축: {randomRotationAxis.x:F2}, {randomRotationAxis.y:F2}, {randomRotationAxis.z:F2}");
    }
}
