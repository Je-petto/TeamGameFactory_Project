using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string playerName = "Player"; 
    public static float itemScore;
    public static float distance;
    public static float totalScore;
    public static bool isLive = true;
    public static bool isPause = false;
    public static int selectPlayer = 0;
    public static bool isInvincible = false;
    public static float collectableIncresePersent = 1f;

    public static void GainScore(int addScore)
    {
        itemScore += addScore;
        totalScore += addScore;
    }

    public static void ResetGame()
    {
        totalScore = 0;
        itemScore = 0;
        distance = 0;
        isLive = true;
        isPause = false;
    }
}
