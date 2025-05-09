using UnityEngine;

// 아이템을 먹고 나서 화면의 모든 장애물을 삭제
public class CollectableDobs : Collectable
{
    public override void ClearObstacles() //Collectable에서 상속받아 override한걸 여기서 구현
    {
        GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle"); //태그가 obstacle인지 확인

        foreach (GameObject obstacle in obstacles) //찾아낸 list의 obstacle을 모두 순회
        {
            Destroy(obstacle); //찾아낸 모든 obstacle 삭제
        }
    }
}