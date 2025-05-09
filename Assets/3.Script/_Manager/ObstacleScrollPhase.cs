using UnityEngine;

public class ObstacleScrollPhase : MonoBehaviour
{
    [Header("난이도 상승 설정")]
    public float intervalTime = 10f; // 지정된 시간(초)마다 난이도 상승 기본값 10초
    public float increseSpeed = 0.1f; // 스크롤 속도 증가량
    public float spawnRate = 0.5f; // 스폰 빈도 증가량

    private float timer; //경과 시간 추적

    [Header("참조할 컴포넌트들")]
    public ScrollManager scrollManager;
    public ObstacleSpawner obstacleSpawner;

    void Update()
    {
        if (!GameManager.isLive) return; //살아있지 않다면(게임중이지 않다면) return

        timer += Time.deltaTime; // 매 시간마다 timer 증가 (경과시간 증가)
        if (timer >= intervalTime)  //타이머가 intervalTime보다 크거나 같아야됨
        {
            timer = 0f; // 초기화
            DifficultyLevel(); //난이도 상승 메소드 호출
        }
    }

    private void DifficultyLevel() //시간 지남에 따라서 난이도 증가 
    {
        if (scrollManager != null) //스크롤의 속도 증가
            scrollManager.ScrollIncreseSpeed += increseSpeed;

        if (obstacleSpawner != null) //생성되는 obstcale 증가
            obstacleSpawner.spawnRate += spawnRate;
    }
}