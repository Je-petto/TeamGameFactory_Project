using UnityEngine;

//이동반전 아이템
public class CollectableRevers : Collectable
{
    public float reverseDuration = 3f; // 리버스 시간 3초 유지

    public override void Reverse(PlayerBehaviour player) //플레이어를 받아서 플레이어에 적용시킴
    {
        player.StartCoroutine(player.ReverseMovement(reverseDuration));
        //코루틴으로 지속시간만큼 적용시켜서 지정된 시간이 지나면 다시 원래대로 돌아가게 함
    }
}