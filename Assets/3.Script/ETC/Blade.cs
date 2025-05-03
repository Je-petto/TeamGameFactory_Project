using UnityEngine;

public class Blade : MonoBehaviour
{
    [Tooltip("Y축 회전 속도 (도/초)")]
    public float rotationSpeed = 120f; // 초당 회전할 각도 (Unity 에디터에서 조절 가능)

    void Update()
    {
        // 오브젝트의 Transform 컴포넌트를 가져옵니다. (this.transform 또는 transform 사용)
        // Y축 (Vector3.up)을 기준으로 rotationSpeed * Time.deltaTime 만큼 회전합니다.
        // Time.deltaTime을 곱해주면 프레임 속도에 상관없이 일정한 속도로 회전하게 됩니다.
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}