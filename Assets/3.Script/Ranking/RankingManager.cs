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
        public int score;
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

    public void SetCurrentPlayerData(string name, int totalScore)
    {
        currentPlayer = new PlayerRankData
        {
            playerID = name,
            score = totalScore
        };
    }

    public int OnSaveButtonClicked()
    {
        if (currentPlayer == null || string.IsNullOrWhiteSpace(currentPlayer.playerID))
        {
            Debug.LogWarning("플레이어 정보가 비어 있습니다.");
            return -1;
        }

        // 중복 이름 있으면 기존 데이터 덮어쓰기
        var existing = rankingData.rankings.Find(p => p.playerID == currentPlayer.playerID);
        if (existing != null)
        {
            existing.score = currentPlayer.score;
        }
        else
        {
            rankingData.rankings.Add(currentPlayer);
        }

        // 정렬 및 상위 5개 자르기
        rankingData.rankings.Sort((a, b) => b.score.CompareTo(a.score));
        if (rankingData.rankings.Count > 5)
            rankingData.rankings = rankingData.rankings.GetRange(0, 5);

        SaveRankingData();
        UpdateRankingUI();

        // 현재 플레이어 순위 계산
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
            text.text = $"{i + 1}위: {rankingData.rankings[i].playerID} - {rankingData.rankings[i].score}";
        }
    }
}
