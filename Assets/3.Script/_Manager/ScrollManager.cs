using UnityEngine;

//아이템 및 장애물 이동용 스크립트
public class ScrollManager : MonoBehaviour
{
    [Header("증가시킬 투사체 부모 오브젝트")]
    public Transform ObstacleSpawnParent;
    public Transform CollectableSpawnParent;

    [Header("속도 증가량 및 기준 거리")]
    private float scrollIncreseSpeed = 1f; // 실제 적용될 스크롤 속도 배율
    public float ScrollIncreseSpeed = 1.1f; // 난이도 증가 시 스크롤 속도 증가량
    public float incresePhase = 200f; // 일정 거리마다 난이도를 증가시키는 기준 거리

    private float destroyPos; // 아이템 삭제 위치 조정

    private Vector3 scrollDirection = Vector3.back; // 카메라 방향으로 움직임 고정(-z축 방향)

    public float destroyOffsetPos = -10f; // 삭제 위치 설정(카메라 뒤쪽 어디인지)
    private int lastPhase; // 마지막으로 확인한 거리 단계 (난이도 증가 체크용)

    void Start()
    {
        if (Camera.main != null)
            destroyPos = Camera.main.transform.position.z + destroyOffsetPos;
        // 메인 카메라 위치 기준으로 오브젝트 삭제 위치 설정
    }

    void Update()
    {
        if (GameManager.isLive) // 플레이어가 생존 중일떄 스폰되는 오브젝트(장애물, 아이템)들이 이동하는 동작 실행
        {
            MoveObstacles();   // 장애물 이동
            MoveCollectable(); // 아이템 이동
        }
    }

    // 장애물 움직임
    private void MoveObstacles()
    {
        if (ObstacleSpawnParent == null) return;
        Obstacle obs = null;
        // 장애물에 들어있는 데이터 중 scrollSpeed 값를 받을 데이터 정의
        float scrollSpeed = 0f;

        // 부모 안에 있는 모든 장애물을 이동
        foreach (Transform obsTr in ObstacleSpawnParent)
            if (obsTr != null)
            {
                obs = obsTr.gameObject.GetComponent<Obstacle>();
                scrollSpeed = obs.data.scrollSpeed;
                obsTr.position += scrollDirection * scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }

        // 장애물 이동에 따른 총 이동 거리 누적
        GameManager.distance += scrollSpeed * Time.deltaTime;

        // 일정 거리마다 스크롤 배율을 증가(난이도 증가)
        // 현재 Phase = 현재까지 진행된 거리 / Phase가 증가하기 위한 기준 거리
        // 6.5 = 1250 / 200 -> 현재 Phase(6단계)
        int currentPhase = (int)(GameManager.distance / incresePhase);
        if (lastPhase != currentPhase)
        {
            lastPhase = currentPhase;
            scrollIncreseSpeed = ScrollIncreseSpeed;
        }

        // 삭제 위치를 넘었을 경우 삭제
        for (int i = ObstacleSpawnParent.childCount - 1; i >= 0; i--)
        {
            // 장애물 친구들 가지고 오기
            Transform item = ObstacleSpawnParent.GetChild(i);

            // 현재 장애물이 있는지 && 장애물의 z축 위치가 파괴되려는 위치에 도달했을 경우
            if (item != null && item.position.z <= destroyPos)
                Destroy(item.gameObject);
        }
    }

    // 아이템 움직임
    private void MoveCollectable()
    {
        if (CollectableSpawnParent == null) return;

        // Collectable은 추상 클래스이므로 가져올 수 없음.
        // 따라서 data를 직접 가져와서 scrollSpeed 값을 받아옴.
        CollectableData colData = null;

        // 부모 안에 있는 모든 아이템을 이동
        foreach (Transform tr in CollectableSpawnParent)
            if (tr != null)
            {
                colData = tr.gameObject.GetComponent<Collectable>().data;
                tr.position += scrollDirection * colData.scrollSpeed * scrollIncreseSpeed * Time.deltaTime;
            }
        // 일정 거리마다 난이도 증가
        int currentPhase = (int)(GameManager.distance / incresePhase);
        if (lastPhase != currentPhase)
        {
            lastPhase = currentPhase;
            scrollIncreseSpeed = ScrollIncreseSpeed;
        }
        // 총 점수 계산(거리+아이템 획득 점수)
        GameManager.totalScore = GameManager.distance + GameManager.itemScore;
        // 삭제 위치를 넘었을 경우 삭제
        for (int i = CollectableSpawnParent.childCount - 1; i >= 0; i--)
        {
            Transform item = CollectableSpawnParent.GetChild(i);
            if (item != null && item.position.z <= destroyPos)
                Destroy(item.gameObject);
        }
    }
}
