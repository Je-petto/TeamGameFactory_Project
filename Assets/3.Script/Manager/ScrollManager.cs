using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    public Transform ObstacleSpawnParent;
    [Tooltip("Z축 이동속도")] public float scrollSpeed = 5f; // Z축 이동 속도
    public float scrollIncreseSpeed = 1.2f;

    private float destroyItem; //아이템 삭제 위치조정

    private Vector3 scrollDirection = Vector3.back; // 카메라 방향으로 움직임 고정(-z축 방향)

    public float destroyOffsetPos = 10f;

    public float distance;
    void Start()
    {
        if (Camera.main != null)
            destroyItem = Camera.main.transform.position.z - destroyOffsetPos; //카메라 Z축의 -5 이후 삭제
    }

    void Update()
    {
        if (GameManager.isLive)
        {
            CheckDistance();
            MoveObstacles();
        }
            
    }

    private void MoveObstacles()
    {
        if (ObstacleSpawnParent == null) return;
        foreach (Transform tr in ObstacleSpawnParent)
            if (tr != null)
                tr.position += scrollDirection * scrollSpeed * Time.deltaTime;
        for (int i = ObstacleSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = ObstacleSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem)
                Destroy(item.gameObject);
        }
    }
    int lastDistance;
    void CheckDistance()
    {   
        distance += scrollSpeed * Time.deltaTime;
        int currentDistance = (int)(distance / 200);
        if (lastDistance != currentDistance)
        {
            lastDistance = currentDistance;
            scrollSpeed *= scrollIncreseSpeed;
        }
        GameManager.score = distance;
    }
}
