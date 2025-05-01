using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public Transform ObstacleSpawnParent;
    [Tooltip("Z축 이동속도")] public float scrollSpeed = 5f; // Z축 이동 속도

    private float destroyItem; //아이템 삭제 위치조정

    private Vector3 scrollDirection = Vector3.back; // 카메라 방향으로 움직임 고정(-z축 방향)

    public float destroyOffsetPos = 5f;


    void Start()
    {
        if (Camera.main != null)
            destroyItem = Camera.main.transform.position.z - destroyOffsetPos; //카메라 Z축의 -5 이후 삭제
    }

    void Update()
    {
        MoveObstacles();
        DestroyObstacles();
    }

    private void MoveObstacles()
    {
        if (ObstacleSpawnParent == null) return;
        foreach (Transform items in ObstacleSpawnParent)
            if (items != null) items.position += scrollDirection * scrollSpeed * Time.deltaTime;
    }

    private void DestroyObstacles()
    {
        if (ObstacleSpawnParent == null) return;

        for (int i = ObstacleSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = ObstacleSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem) Destroy(item.gameObject);
        }
    }
}
