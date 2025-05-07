using UnityEngine;

public class CursorManager : MonoBehaviour
{
    void Start()
    {
        // 게임이 시작될 때 마우스 커서를 게임 창 안에 가둡니다.
        // CursorLockMode.Confined: 커서를 보이게 유지하며 게임 창 안에 제한합니다.
        Cursor.lockState = CursorLockMode.Confined;

        // Confined 상태에서는 기본적으로 커서가 보이지만, 명시적으로 설정해도 좋습니다.
        // Cursor.visible = true;
    }

    // 이 메소드는 게임 창이 포커스를 얻거나 잃을 때 호출됩니다.
    // 예를 들어, 사용자가 게임 창 밖의 다른 프로그램을 클릭했다가 다시 게임 창을 클릭할 때 사용됩니다.
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            // 게임 창이 포커스를 다시 얻으면 커서를 다시 게임 창 안에 가둡니다.
            Cursor.lockState = CursorLockMode.Confined;
            // Cursor.visible = true; // 커서가 보이도록
        }
        else
        {
            // 게임 창이 포커스를 잃으면 커서를 해제하여 사용자가 다른 창과 상호작용할 수 있게 합니다.
            Cursor.lockState = CursorLockMode.None;
            // Cursor.visible = true; // 해제 상태에서도 커서는 보이게 유지합니다.
        }
    }
}