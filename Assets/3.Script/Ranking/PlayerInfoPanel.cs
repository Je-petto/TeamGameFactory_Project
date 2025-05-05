using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [Header("입력 데이터 (Inspector에서 설정)")]
    public string playerID;
    public int distanceScore;
    public int itemScore;

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
        int totalScore = distanceScore + itemScore;

        // UI에 포맷팅된 텍스트 표시
        nameText.text = $"Player ID : {playerID}";
        distanceText.text = $"Distance : {distanceScore}";
        itemScoreText.text = $"Item Score : {itemScore}";
        totalScoreText.text = $"Total Score : {totalScore}";

        // 랭킹 정보 설정 및 저장
        rankingManager.SetCurrentPlayerData(playerID, totalScore);
        int playerRank = rankingManager.OnSaveButtonClicked();
        rankingNumText.text = playerRank > 0 ? $"Rank : {playerRank}" : "UNRANKED";
    }
}
