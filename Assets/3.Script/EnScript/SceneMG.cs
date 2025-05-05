using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMG : MonoBehaviour
{
    public void TapToStart()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
    public void BackToStart()
    {
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }
    public void BackToMainScene()
    {
        SceneManager.LoadScene(1,LoadSceneMode.Single);
    }
    public void LoadingScene()
    {
        SceneManager.LoadScene(2,LoadSceneMode.Single);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(3,LoadSceneMode.Single);
    }
    public void ToRankSystem()
    {
        SceneManager.LoadScene(4,LoadSceneMode.Single);
    }
    public void GoToLog()
    {
        SceneManager.LoadScene(5,LoadSceneMode.Single);
    }
    
}
