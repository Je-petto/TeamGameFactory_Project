using UnityEngine;

//collectable 속성을 정의
public enum CollectableType {SCORE, HEALTH, DOBS, REVERS}

[CreateAssetMenu(menuName = "ScriptableObject/Collectable", fileName = "Collectable")]
public class CollectableData : ScriptableObject
{
    public CollectableType type;
    public float scrollSpeed = 30f; //아이템 이동속도(스크롤속도)
    public float rotationSpeed = 60f; // 회전속도
    public float weight = 1f; //가중치
}
