using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템을 먹고 나서 화면의 모든 장애물을 삭제
public class CollectableDobs : Collectable
{
    public override void ClearObstacles()
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        foreach (GameObject obstacle in obstacles)
        {
            Destroy(obstacle);
        }
    }
}