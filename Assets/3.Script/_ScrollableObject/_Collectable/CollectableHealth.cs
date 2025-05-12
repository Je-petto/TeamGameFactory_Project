using UnityEngine;

public class CollectableHealth : Collectable
{
    [Header("Collectable's Detailed Setup")]
    public int gainHealth = 10; // 기본 Health 회복량
    public int gainScore = 25;  // 기본 Score 획득량
}
