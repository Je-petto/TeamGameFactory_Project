using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagers : MonoBehaviour
{
    public void BackToStart() // 메인화면 씬.
    {
        Debug.Log("메인화면 씬 전환 완료");
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    public void LobbyScene() // 로비화면 씬.
    {
        Debug.Log("로비 씬 전환 완료");
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }
    public void StartGame() //InGame 씬
    {
        Time.timeScale = 1;
        Debug.Log("인게임 씬 전환 완료");
        SceneManager.LoadScene(2, LoadSceneMode.Single);
        //로비->로딩->인게임 데이터 넘겨주기
    }
    public void ToRankSystem() //랭크 씬
    {
        Debug.Log("랭크 씬 전환 완료");
        SceneManager.LoadScene(3, LoadSceneMode.Single);

    }
    public void GoToLog() //로그 씬
    {
        Debug.Log("미션 로그 씬 전환 완료");
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }
}
