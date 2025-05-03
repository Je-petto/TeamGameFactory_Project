using UnityEngine;
using CustomInspector;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static float score;
    public static bool isLive = true;
    public static int selectPlayer = 0;

    public static void GainScore(int addScore)
    {
        score += addScore;
    }

    private void Awake()
    {
        // 만약 Instance가 아직 null이라면 (= 첫 번째 인스턴스라면)
        if (Instance == null)
        {
            // 이 인스턴스를 싱글톤 Instance로 설정합니다.
            Instance = this;

            // 씬이 바뀌어도 파괴되지 않도록 설정합니다.
            DontDestroyOnLoad(gameObject);
        }
        else // 만약 Instance가 이미 존재한다면 (= 중복 생성되었다면)
        {
            // 새로 생성된 이 게임 오브젝트를 파괴합니다.
            // Debug.LogWarning("GameManager 인스턴스가 이미 존재합니다. 새로 생성된 인스턴스를 파괴합니다.");
            Destroy(gameObject);
        }
    }
}
