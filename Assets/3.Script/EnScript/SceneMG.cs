using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMG : MonoBehaviour
{
    public void TapToStart()
    {
        Debug.Log("탭 투 스타트");

        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
    public void BackToStart()
    {
        Debug.Log("백 투 스타트");
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }
    public void BackToMenu()
    {
        Debug.Log("백 투 메뉴뉴");

        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
    public void StartGame()
    {
        Debug.Log("스타트 게임");
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
    public void ToRankSystem()
    {
        Debug.Log("랭킹 UI로 이동");
        SceneManager.LoadScene(3,LoadSceneMode.Single);
    }
    public void GoToLog()
    {
        Debug.Log("미션로그그 UI로 이동");
        SceneManager.LoadScene(4,LoadSceneMode.Single);
    }
    
}
