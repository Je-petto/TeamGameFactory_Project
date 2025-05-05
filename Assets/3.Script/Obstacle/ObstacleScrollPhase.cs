using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScrollPhase : MonoBehaviour
{

    [Header("난이도 상승 설정")]
    public float intervalTime = 10f; // 몇 초마다 난이도 상승
    public float scrollSpeedMultiplier = 0.1f; // 스크롤 속도 증가량
    public float spawnRateMultiplierIncrement = 0.5f; // 스폰 빈도 증가량

    private float timer;

    [Header("참조할 컴포넌트들")]
    public ScrollManager scrollManager;
    public ObstacleSpawner obstacleSpawner;


      void Update()
    {
        if (!GameManager.isLive) return;

        timer += Time.deltaTime;
        if (timer >= intervalTime)
        {
            timer = 0f;
            IncreaseDifficulty();
        }
    }

    private void IncreaseDifficulty() //난이도 증가
    {
        if (scrollManager != null)
        {
            scrollManager.ScrollIncreseSpeed += scrollSpeedMultiplier;
        }

        if (obstacleSpawner != null)
        {
            obstacleSpawner.spawnRate += spawnRateMultiplierIncrement;
        }
    }


}
