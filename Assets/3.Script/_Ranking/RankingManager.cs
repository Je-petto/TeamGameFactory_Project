using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

// 해당 스크립트는 플레이어들의 점수 저장,
// 상위 5명을 "랭킹" 형태로 화면에 보여주는 역할 제공
public class RankingManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Transform rankingListParent;       // 랭킹 목록이 붙을 부모 오브젝트 (UI 안쪽 박스)
    public GameObject rankingTextTemplate;    // 한 줄의 랭킹 텍스트 템플릿 (비활성화된 상태)

    private string filePath; // 점수를 저장할 JSON 파일 경로

    // 개별 플레이어 정보를 담는 클래스
    //Unity에서는 JsonUtility.ToJson()이나 FromJson()을 사용할 때,
    //해당 클래스가 [System.Serializable]로 표시되어 있어야 정상적으로 작동
    [System.Serializable]
    public class PlayerRankData
    {
        public string playerID;     // 플레이어 이름
        public float distance;      // 거리 점수
        public float itemScore;     // 아이템 점수
        public float totalScore;    // 총 점수
    }

    // 모든 플레이어 랭킹 정보를 담는 리스트 형태 클래스
    [System.Serializable]
    public class RankingData
    {
        public List<PlayerRankData> rankings = new List<PlayerRankData>(); // 여러 플레이어 정보 저장
    }

    private RankingData rankingData;           // 전체 랭킹 데이터 (리스트)
    private PlayerRankData currentPlayer;      // 현재 점수를 기록할 플레이어

    private void Awake()
    {
        // 게임이 시작되면 저장된 JSON 파일을 불러온다
        filePath = Path.Combine(Application.persistentDataPath, "ranking_data.json");

        LoadRankingData();     // JSON 파일에서 이전 랭킹 불러오기
        UpdateRankingUI();     // 불러온 랭킹을 화면에 보여주기
    }

    // 게임 종료 후 현재 플레이어의 점수 정보 입력 (이름, 점수 등)
    public void SetCurrentPlayerData(string name, float distance, float itemScore, float totalScore)
    {
        currentPlayer = new PlayerRankData
        {
            playerID = name,
            distance = distance,
            itemScore = itemScore,
            totalScore = totalScore
        };
    }

    // 점수를 저장하고 랭킹을 새로 정리하는 함수 (버튼으로 실행 가능)
    // <returns>플레이어의 랭킹 (1~5위), 없으면 -1</returns>
    public int OnSaveButtonClicked()
    {
        // 이름이 비어 있으면 저장하지 않음
        if (currentPlayer == null || string.IsNullOrWhiteSpace(currentPlayer.playerID))
        {
            Debug.LogWarning("플레이어 정보가 비어 있습니다.");
            return -1;
        }

        // 이미 저장된 이름이 있다면 점수만 새로 입력 (덮어쓰기)
        var existing = rankingData.rankings.Find(p => p.playerID == currentPlayer.playerID);
        if (existing != null)
        {
            existing.distance = currentPlayer.distance;
            existing.itemScore = currentPlayer.itemScore;
            existing.totalScore = currentPlayer.totalScore;
        }
        else
        {
            rankingData.rankings.Add(currentPlayer); // 새 플레이어면 추가
        }

        // 총점 기준으로 내림차순 정렬 (높은 점수 → 위쪽)
        rankingData.rankings.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

        // 상위 5명만 남기고 나머지는 삭제
        if (rankingData.rankings.Count > 5)
            rankingData.rankings = rankingData.rankings.GetRange(0, 5);

        SaveRankingData();   // ▶ 정리된 랭킹을 JSON 파일로 저장
        UpdateRankingUI();   // ▶ 랭킹을 화면에 다시 그리기

        // 현재 플레이어의 순위 확인
        int rank = rankingData.rankings.FindIndex(p => p.playerID == currentPlayer.playerID);
        return (rank >= 0) ? rank + 1 : -1; // 0부터 시작이므로 +1
    }

    // JSON 파일에서 랭킹 데이터 불러오기
    private void LoadRankingData()
    {
        if (File.Exists(filePath))
        {
            // 파일이 존재하면 내용을 읽어서 랭킹 리스트로 변환
            string json = File.ReadAllText(filePath);
            rankingData = JsonUtility.FromJson<RankingData>(json);
        }
        else
        {
            // 파일이 없다면 새로운 랭킹 리스트를 생성
            rankingData = new RankingData();
        }
    }

    // 현재 랭킹 데이터를 JSON 파일로 저장
    private void SaveRankingData()
    {
        // 보기 좋게 정렬된 JSON 형식으로 저장
        string json = JsonUtility.ToJson(rankingData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("랭킹 데이터 저장 완료: " + filePath);
    }

    // 화면에 랭킹 정보를 출력 (TextMeshPro UI 사용)
    private void UpdateRankingUI()
    {
        // 기존에 있던 랭킹 항목 삭제 (템플릿 제외)
        foreach (Transform child in rankingListParent)
        {
            if (child != rankingTextTemplate.transform)
                Destroy(child.gameObject);
        }

        // 현재 랭킹 정보를 하나씩 UI 항목으로 생성
        for (int i = 0; i < rankingData.rankings.Count; i++)
        {
            // 템플릿을 복제하여 새로운 랭킹 줄 생성
            GameObject entry = Instantiate(rankingTextTemplate, rankingListParent);
            entry.SetActive(true); // 비활성화된 템플릿을 복제했으니 활성화 필요

            // 복제된 항목에 텍스트 설정 (예: 1위: Alice - 150점)
            TMP_Text text = entry.GetComponent<TMP_Text>();
            var data = rankingData.rankings[i];
            text.text = $"{i + 1}위: {data.playerID} - {(int)data.totalScore}점";
        }
    }
}
