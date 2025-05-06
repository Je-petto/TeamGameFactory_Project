using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButton : MonoBehaviour
{
    // 이 메서드를 버튼의 OnClick 이벤트에 연결하세요.
    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 실행 중지
        #else
        Application.Quit(); // 빌드된 게임 종료
        #endif
    }
}

