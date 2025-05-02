using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData data;

    [Tooltip("회전 속도 (도/초)")]
    public float rotationSpeed = 50f; // 초당 50도 회전 (인스펙터에서 조절 가능)

    private Vector3 randomRotationAxis; // 무작위 회전 축을 저장할 변수

    void Awake()
    {
        // 스크립트가 처음 로드될 때 (장애물 오브젝트가 생성될 때)
        // 무작위 회전 축을 생성해.
        // Random.insideUnitSphere는 반지름 1인 구 내부의 무작위 점을 반환해.
        // .normalized를 붙여서 방향만 가지고 크기는 1인 벡터로 만들어 축으로 사용해.
        randomRotationAxis = Random.insideUnitSphere.normalized;

        // 혹시라도 아주 낮은 확률로 Vector3.zero가 나오면 회전 축이 안 되니 기본 축(예: Vector3.up)으로 설정
        if (randomRotationAxis == Vector3.zero)
        {
            randomRotationAxis = Vector3.up;
        }

        // Debug.Log($"장애물 생성! 회전 축: {randomRotationAxis.x:F2}, {randomRotationAxis.y:F2}, {randomRotationAxis.z:F2}"); // 어떤 축으로 회전하는지 확인하고 싶으면 이 줄을 활용해봐.
    }
    void Update()
    {
        // 매 프레임마다 설정된 무작위 축을 기준으로 회전시켜.
        // Time.deltaTime을 곱해서 프레임 속도와 상관없이 일정한 속도로 회전하게 만들어.
        transform.Rotate(randomRotationAxis, rotationSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerBehaviour player = col.transform.parent.parent.GetComponent<PlayerBehaviour>();

            if (player.health - data.damage < 0) player.health = 0;
            else player.health -= data.damage;

            Destroy(gameObject);
        }
    }
}
