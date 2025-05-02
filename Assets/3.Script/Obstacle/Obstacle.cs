using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstacleData data;
    private Vector3 randomRotationAxis;
    void Awake()
    {
        randomRotationAxis = Random.insideUnitSphere.normalized;
        if (randomRotationAxis == Vector3.zero)
            randomRotationAxis = Vector3.up;

        // Debug.Log($"장애물 생성! 회전 축: {randomRotationAxis.x:F2}, {randomRotationAxis.y:F2}, {randomRotationAxis.z:F2}");
    }
    void Update()
    {
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            PlayerBehaviour player = col.transform.parent.parent.GetComponent<PlayerBehaviour>();

            player.OnDamage(data.damage);

            Destroy(gameObject);
        }
    }
}
