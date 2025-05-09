using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Scrollable", fileName = "Scrollable")]
public class ScrollableObejctData : ScriptableObject
{
    [Header("Scrollable Object Setup")]
    public float scrollSpeed = 30f; // 장애물 이동(스크롤) 속도
    public float rotationSpeed = 50f; // 장애물 회전 속도
    public float weight = 1f; // 각각 할당된 장애물의 소환가중치(소환빈도)
}