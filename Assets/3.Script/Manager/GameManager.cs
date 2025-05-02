using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float score;
    public static bool isLive = true;

    public static void GainScore(int addScore)
    {
        score += addScore;
    }
}
