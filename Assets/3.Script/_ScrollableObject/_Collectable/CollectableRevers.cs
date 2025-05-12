using UnityEngine;

public class CollectableRevers : Collectable
{
    [Header("Collectable's Detailed Setup")]
    public float reverseDuration = 3f; // 리버스 시간 3초 유지
    public int gainScore = -100;  // 기본 점수 추가

    public override void Reverse(PlayerBehaviour player) //플레이어를 받아서 플레이어에 적용시킴
    {
        player.StartCoroutine(player.ReverseMovement(reverseDuration));
        //코루틴으로 지속시간만큼 적용시켜서 지정된 시간이 지나면 다시 원래대로 돌아가게 함
    }
}