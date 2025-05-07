using UnityEngine;

//아이템 및 장애물 이동용 스크립트
public class ScrollManager : MonoBehaviour
{
    [Header("증가시킬 투사체 부모 오브젝트")]
    public Transform ObstacleSpawnParent;
    public Transform CollectableSpawnParent;
    // [Tooltip("Z축 이동속도")] public float scrollSpeed = 5f; // Z축 이동 속도

    [Header("속도 증가량 및 기준 거리")]
    private float scrollIncreseSpeed = 1f; // 실제 적용될 스크롤 속도 배율
    public float ScrollIncreseSpeed = 1.1f; // 난이도 증가 시 스클로 속도 증가량
    public float increseDistance = 200f; //일정거리마다 난이도를 증가시키는 기준 거리

    private float destroyItem; // 아이템 삭제 위치조정

    private Vector3 scrollDirection = Vector3.back; // 카메라 방향으로 움직임 고정(-z축 방향)

    public float destroyOffsetPos = 10f; //삭제 위치 설정(카메라 뒤쪽 어디인지)
    private int lastDistance; //마지막으로 확인한 거리 단계 (난이도 증가 체크용)
    void Start()
    {
        if (Camera.main != null)
            destroyItem = Camera.main.transform.position.z - destroyOffsetPos;
        // 메인 카메라 위치 기준으로 오브젝트 삭제 위치 설정
    }

    void Update()
    {
        if (GameManager.isLive) //게임 실행중일떄 스크롤 동작 실행
        {
            MoveObstacles();   //장애물 이동
            MoveCollectable(); //아이템 이동
        }

    }

    //장애물 움직임
    private void MoveObstacles()
    {
        if (ObstacleSpawnParent == null) return;
        Obstacle obs = null;
        float scrollSpeed = 0f;
        //부모 안에 있는 모든 장애물을 이동
        foreach (Transform tr in ObstacleSpawnParent)
            if (tr != null)
            {
                obs = tr.gameObject.GetComponent<Obstacle>();
                scrollSpeed = obs.data.scrollSpeed;
                tr.position += scrollDirection * scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }

        //장애물 이동에 따른 총 이동 거리 누적
        GameManager.distance += scrollSpeed * Time.deltaTime;
        //일정 거리마다 스크롤 배율을 증가(난이도 증가)
        int currentDistance = (int)(GameManager.distance / increseDistance);
        if (lastDistance != currentDistance)
        {
            lastDistance = currentDistance;
            scrollIncreseSpeed = ScrollIncreseSpeed;
        }
        //삭제 위치를 넘었을 경우 삭제
        for (int i = ObstacleSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = ObstacleSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem)
                Destroy(item.gameObject);
        }
    }
    //아이템 움직임
    private void MoveCollectable()
    {
        if (CollectableSpawnParent == null) return;
        CollectableData colData = null;
        //부모 안에 있는 모든 아이템을 이동
        foreach (Transform tr in CollectableSpawnParent)
            if (tr != null)
            {
                colData = tr.gameObject.GetComponent<Collectable>().data;
                tr.position += scrollDirection * colData.scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }
        //아이템 이동에 따른 총 이동 거리 누적
        GameManager.distance += colData.scrollSpeed * Time.deltaTime;
        //일정 거리마다 난이도 증가
        int currentDistance = (int)(GameManager.distance / increseDistance);
        if (lastDistance != currentDistance)
        {
            lastDistance = currentDistance;
            scrollIncreseSpeed = ScrollIncreseSpeed;
        }
        //총 점수 계산(거리+아이템 획득 점수)
        GameManager.totalScore = GameManager.distance + GameManager.itemScore;
        //삭제 위치를 넘었을 경우 삭제
        for (int i = CollectableSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = CollectableSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyItem)
                Destroy(item.gameObject);
        }
    }


    //형변환용 메소드 추후 필요시 사용
    // void CheckDistance(object ob, bool isObs)
    // {
    //     object obj;
    //     if (isObs)
    //         obj = (Obstacle)ob;
    //     else
    //         obj = (Collectable)ob;
    // }
}
