using UnityEngine;
using CustomInspector;

public class GameManager : MonoBehaviour
{
    public static float itemScore;
    public static float distance;
    public static float totalScore;
    public static bool isLive = true;
    public static bool isPause = false;
    public static int selectPlayer = 1;

    public static void GainScore(int addScore)
    {
        itemScore += addScore;
        totalScore += addScore;
    }
}
