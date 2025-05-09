using UnityEngine;

public class Obstacle : ScrollableObject
{
    public ObstacleData data; // 장애물의 속성을 담은 ObstacleData 외부에서 연결
    void Update()
    {
        transform.Rotate(randomRotationAxis, data.rotationSpeed * Time.deltaTime);
        // data.rotationSpeed값에 따라 매 프레임마다 회전시킴
    }
}