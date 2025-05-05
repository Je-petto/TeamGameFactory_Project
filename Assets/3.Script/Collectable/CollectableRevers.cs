using UnityEngine;

public class CollectableRevers : Collectable
{
    public float reverseDuration = 3f; // 3초 유지

    public void Reverse(PlayerBehaviour player)
    {
        player.StartCoroutine(player.ReverseMovement(reverseDuration));
    }
}