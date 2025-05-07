using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 랭킹 시스템에 들어갈 데이터들
    public static string playerName = "Player"; 
    public static float itemScore;
    public static float distance;
    public static float totalScore;

    // 플레이어 생존 여부와 게임의 일시정지 여부 판단
    public static bool isLive = true;
    public static bool isPause = false;

    // 어떤 캐릭터를 선택했는 지 판단하는 데이터
    public static int selectPlayer = 0;

    // Ability에 필요한 데이터
    public static bool isInvincible = false;
    public static float collectableIncresePersent = 1f;

    // Score 획득
    public static void GainScore(int addScore)
    {
        itemScore += addScore;
        totalScore += addScore;
    }

    // 게임 재시작할 때 사용되는 메서드
    public static void ResetGame()
    {
        totalScore = 0;
        itemScore = 0;
        distance = 0;
        isLive = true;
        isPause = false;
    }
}
