using UnityEngine;

//ScriptableObject를 상속 유니티 에디터에서 우클릭으로 Create->ScriptableObject->Obstacle
[CreateAssetMenu(menuName = "ScriptableObject/Obstacles", fileName = "Obstacles")]
public class ObstacleData : ScriptableObject
{
    public int damage = 10; // 해당 오브젝트가 플레이어에게 입히는 데미지
    public float scrollSpeed = 30f; //장애물 이동(스크롤) 속도
    public float rotationSpeed = 50f; //장애물 회전 속도
    public float weight = 1f; //각각 할당된 장애물의 소환가중치(소환빈도)
}