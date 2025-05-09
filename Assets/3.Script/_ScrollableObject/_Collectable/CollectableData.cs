using UnityEngine;

//collectable 속성을 정의
public enum CollectableType {SCORE, HEALTH, DOBS, REVERS}

[CreateAssetMenu(menuName = "ScriptableObject/Scrollable/Collectable", fileName = "Collectable")]
public class CollectableData : ScrollableObjectData
{
    [Header("Collectable Setup")]
    public CollectableType collectableType;
}