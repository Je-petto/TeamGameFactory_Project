using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class GameOverManager : MonoBehaviour
{
    // 게임 오버 UI와 플레이어 이름 입력창을 연결 (Unity Inspector에서 지정)
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private PlayerBehaviour player;// Inspector에 할당

    // 게임이 시작될 때 게임 오버 UI는 꺼져 있어야 한다
    private void Start()
    {
        gameOverUI.SetActive(false);
    }
    // 게임 오버가 되었을 때 실행되는 함수
    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true); // 게임 오버 UI 활성화
        GameManager.selectPlayer = 0; // selectplayer 값을 0으로 전환하여 캐릭터 선택값 초기화
        playerNameInput.text = ""; // 이름 입력칸 초기화 
        Time.timeScale = 0f; // 게임 정지
    }

    // 플레이어가 이름을 입력하고 확인 버튼을 눌렀을 때 실행
    public void OnSubmitName()
    {
        Debug.Log("입력한 이름: " + playerNameInput.text); // 디버깅 용도로 이름 출력

        // GameManager에 있는 이름과 점수를 가져와 저장
        GameManager.playerName = playerNameInput.text;
        GameManager.totalScore = GameManager.itemScore + GameManager.distance;
        // GameManager.totalScore: 게임 전체에서 사용하는 점수. 다른 클래스에서도 이 값을 참고할 수 있다

        // 랭킹 정보를 저장하는 함수 호출
        SavePlayerRecord(GameManager.playerName, (int)GameManager.totalScore);

        // 결과 화면으로 전환
        SceneManager.LoadScene("3.GameResult");
    }

    // 플레이어의 기록을 파일에 저장하는 함수예요
    private void SavePlayerRecord(string name, int totalScore)//int totalScore: SavePlayerRecord 함수에 전달된 지역 변수
    {
        // 파일 저장 위치를 정해요 (게임 저장용 특별 폴더)
        string path = Application.persistentDataPath + "/ranking.json";

        // 새로운 랭킹 데이터 하나를 생성
        RankingManager.PlayerRankData newRecord = new RankingManager.PlayerRankData
        {
            playerID = name,           // 플레이어 이름
            totalScore = totalScore    // 총 점수
        };

        // 랭킹 데이터를 담는 틀(리스트)을 준비
        RankingManager.RankingData pdatafile = new RankingManager.RankingData();

        // 기존에 저장된 랭킹 파일이 있다면 소환
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            pdatafile = JsonUtility.FromJson<RankingManager.RankingData>(json);
        }

        // 이름이 이미 있는 경우, 기존 점수를 새로운 점수로 수정정
        var existing = pdatafile.rankings.Find(p => p.playerID == name);
        if (existing != null)
            existing.totalScore = newRecord.totalScore;//newRecord.totalScore: PlayerRankData라는 객체에 있는 변수 이름
        else
            pdatafile.rankings.Add(newRecord); // 처음 등록된 이름이면 추가

        // 점수 높은 순서로 정렬
        pdatafile.rankings.Sort((a, b) => b.totalScore.CompareTo(a.totalScore));

        // 랭킹은 상위 5명까지만 기록
        if (pdatafile.rankings.Count > 5)
            pdatafile.rankings = pdatafile.rankings.GetRange(0, 5);

        // JSON 형식으로 다시 저장
        string newJson = JsonUtility.ToJson(pdatafile, true);
        File.WriteAllText(path, newJson);
    }
}

