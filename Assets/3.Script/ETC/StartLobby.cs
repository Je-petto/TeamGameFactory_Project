using UnityEngine;

public class StartLobby : MonoBehaviour
{
    void Start()
    {
        GameManager.selectPlayer = 0;
        Time.timeScale = 1f;
    }
}
