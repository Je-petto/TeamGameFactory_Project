using UnityEngine;

// ScriptableObject를 상속 유니티 에디터에서 우클릭으로 Create->ScriptableObject->Obstacle
[CreateAssetMenu(menuName = "ScriptableObject/Scrollable/Obstacle", fileName = "Obstacle")]
public class ObstacleData : ScrollableObjectData
{
    [Header("Obstacle Setup")]
    public int damage = 10; // 해당 오브젝트가 플레이어에게 입히는 데미지
}