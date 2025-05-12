
using CustomInspector;

public class GameManager : SingletonBehaviour<GameManager>
{
    // Scene이 바뀌어도 파괴되지 않게 하기
    protected override bool IsDontDestroy() => true;
    
    // 랭킹 시스템에 들어갈 데이터들
    [ReadOnly] public string playerName = "Player"; 
    [ReadOnly] public float itemScore;
    [ReadOnly] public float distance;
    [ReadOnly] public float totalScore;

    // 플레이어 생존 여부와 게임의 일시정지 여부 판단
    [ReadOnly] public bool isLive = true;
    [ReadOnly] public bool isPause = false;

    // 어떤 캐릭터를 선택했는 지 판단하는 데이터
    [ReadOnly] public int selectPlayer = 0;

    // Ability에 필요한 데이터
    [ReadOnly] public bool isInvincible = false;
    [ReadOnly] public float collectableIncresePersent = 1f;

    // Score 획득
    public void GainScore(int addScore)
    {
        itemScore += addScore;
        totalScore += addScore;
    }

    // 게임 재시작할 때 사용되는 메서드
    public void ResetGame()
    {
        totalScore = 0;
        itemScore = 0;
        distance = 0;

        isLive = true;
        isPause = false;
        
        isInvincible = false;
        collectableIncresePersent = 1f;
    }
}
