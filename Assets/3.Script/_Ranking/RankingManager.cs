using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Transform rankingListParent;
    public GameObject rankingTextTemplate;

    private string filePath;

    [System.Serializable]
    public class PlayerRankData
    {
        public string playerID;
        public float distance;
        public float itemScore;
        public float totalScore;
    }

    [System.Serializable]
    public class RankingData
    {
        public List<PlayerRankData> rankings = new List<PlayerRankData>();
    }

    private RankingData rankingData;
    private PlayerRankData currentPlayer;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "ranking.json");
        LoadRankingData();
        UpdateRankingUI();
    }

    // 👉 이름 + 거리 점수 + 아이템 점수 + 총합점 입력
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

    public int OnSaveButtonClicked()
    {
        if (currentPlayer == null || string.IsNullOrWhiteSpace(currentPlayer.playerID))
        {
            Debug.LogWarning("플레이어 정보가 비어 있습니다.");
            return -1;
        }

        // 중복 이름이면 덮어쓰기
        var existing = rankingData.rankings.Find(p => p.playerID == currentPlayer.playerID);
        if (existing != null)
        {
            existing.distance = currentPlayer.distance;
            existing.itemScore = currentPlayer.itemScore;
            existing.totalScore = currentPlayer.totalScore;
        }
        else
        {
            rankingData.rankings.Add(currentPlayer);
        }

        // 총점 기준 정렬 후 상위 5개 유지
        rankingData.rankings.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));
        if (rankingData.rankings.Count > 5)
            rankingData.rankings = rankingData.rankings.GetRange(0, 5);

        SaveRankingData();
        UpdateRankingUI();

        int rank = rankingData.rankings.FindIndex(p => p.playerID == currentPlayer.playerID);
        return (rank >= 0) ? rank + 1 : -1;
    }

    private void LoadRankingData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            rankingData = JsonUtility.FromJson<RankingData>(json);
        }
        else
        {
            rankingData = new RankingData();
        }
    }

    private void SaveRankingData()
    {
        string json = JsonUtility.ToJson(rankingData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("랭킹 데이터 저장 완료: " + filePath);
    }

    private void UpdateRankingUI()
    {
        foreach (Transform child in rankingListParent)
        {
            if (child != rankingTextTemplate.transform)
                Destroy(child.gameObject);
        }

        for (int i = 0; i < rankingData.rankings.Count; i++)
        {
            GameObject entry = Instantiate(rankingTextTemplate, rankingListParent);
            entry.SetActive(true);

            TMP_Text text = entry.GetComponent<TMP_Text>();
            var data = rankingData.rankings[i];
            text.text = $"{i + 1}위: {data.playerID} - {(int)data.totalScore}점";
        }
    }

}
