using UnityEngine;

public class ScrollManager : MonoBehaviour
{
    [Header("증가시킬 투사체 부모 오브젝트")]
    public Transform ObstacleSpawnParent;
    public Transform CollectableSpawnParent;
    // [Tooltip("Z축 이동속도")] public float scrollSpeed = 5f; // Z축 이동 속도
    
    [Header("속도 증가량 및 기준 거리")]
    private float scrollIncreseSpeed = 1f;
    public float ScrollIncreseSpeed = 1.1f;
    public float increseDistance = 200f;

    private float destroyItem; // 아이템 삭제 위치조정

    private Vector3 scrollDirection = Vector3.back; // 카메라 방향으로 움직임 고정(-z축 방향)

    public float destroyOffsetPos = 10f;
    void Start()
    {
        if (Camera.main != null)
            destroyItem = Camera.main.transform.position.z - destroyOffsetPos; // 카메라 Z축의 -5 이후 삭제
    }

    void Update()
    {
        if (GameManager.isLive)
        {
            MoveObstacles();
            MoveCollectable();
        }
            
    }

    private void MoveObstacles()
    {
        if (ObstacleSpawnParent == null) return;
        Obstacle obs = null;
        float scrollSpeed = 0f;
        foreach (Transform tr in ObstacleSpawnParent)
            if (tr != null)
            {
                obs = tr.gameObject.GetComponent<Obstacle>();
                scrollSpeed = obs.data.scrollSpeed;
                tr.position += scrollDirection * scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }
            GameManager.distance += scrollSpeed * Time.deltaTime;
            int currentDistance = (int)(GameManager.distance / increseDistance);
            if (lastDistance != currentDistance)
            {
                lastDistance = currentDistance;
                scrollIncreseSpeed = ScrollIncreseSpeed;
            }                
        for (int i = ObstacleSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = ObstacleSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem)
                Destroy(item.gameObject);
        }
    }
    private void MoveCollectable()
    {
        if (CollectableSpawnParent == null) return;
        CollectableData colData = null;
        foreach (Transform tr in CollectableSpawnParent)
            if (tr != null)
            {
                colData = tr.gameObject.GetComponent<Collectable>().data;
                tr.position += scrollDirection * colData.scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }
            GameManager.distance += colData.scrollSpeed * Time.deltaTime;
            int currentDistance = (int)(GameManager.distance / increseDistance);
            if (lastDistance != currentDistance)
            {
                lastDistance = currentDistance;
                scrollIncreseSpeed = ScrollIncreseSpeed;
            }
        GameManager.totalScore = GameManager.distance + GameManager.itemScore;
        for (int i = CollectableSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = CollectableSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem)
                Destroy(item.gameObject);
        }
    }
    int lastDistance;
    void CheckDistance(object ob, bool isObs)
    {
        object obj;
        if (isObs)
            obj = (Obstacle)ob;
        else    
            obj = (Collectable)ob;
    }
}
