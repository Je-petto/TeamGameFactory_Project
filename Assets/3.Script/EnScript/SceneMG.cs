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
    public void StartGame()
    {
        Debug.Log("스타트 게임");
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
}
