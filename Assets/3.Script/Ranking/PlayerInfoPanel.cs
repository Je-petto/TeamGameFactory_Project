using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [Header("TextMeshPro 표시 UI")]
    public TMP_Text nameText;
    public TMP_Text distanceText;
    public TMP_Text itemScoreText;
    public TMP_Text totalScoreText;
    public TMP_Text rankingNumText;

    [Header("랭킹 관리자 참조")]
    public RankingManager rankingManager;

    void Start()
    {
        // GameManager에서 점수 및 이름 받아오기
        string playerID = GameManager.playerName;
        int distanceScore = Mathf.RoundToInt(GameManager.distance);
        int itemScore = Mathf.RoundToInt(GameManager.itemScore);
        int totalScore = Mathf.RoundToInt(GameManager.totalScore);

        // UI에 포맷팅된 텍스트 표시
        nameText.text = $"Player ID : {playerID}";
        distanceText.text = $"Distance : {distanceScore}";
        itemScoreText.text = $"Item Score : {itemScore}";
        totalScoreText.text = $"Total Score : {totalScore}";

        // 랭킹 정보 설정 및 저장
        rankingManager.SetCurrentPlayerData(playerID, distanceScore, itemScore, totalScore);
        int playerRank = rankingManager.OnSaveButtonClicked();
        rankingNumText.text = playerRank > 0 ? $"Rank : {playerRank}" : "UNRANKED";
    }
}