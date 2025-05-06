using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerMeshes = new List<GameObject>();
    [SerializeField] private GameObject playerObj = null;

    void Start()
    {
        if (playerMeshes == null || playerObj == null || GameManager.selectPlayer < 0 || GameManager.selectPlayer >= playerMeshes.Count)
        {
            // 디버그 메시지를 출력하여 문제가 있음을 알 수 있도록 하는 것이 좋습니다.
            Debug.LogError("PlayerManager 초기화 오류: playerMeshes, playerObj가 null이거나 selectPlayer 인덱스가 유효하지 않습니다.");
            return;
        }

        for(int index = 0; index < playerMeshes.Count; index++)
            if (index == GameManager.selectPlayer)
            {
                Quaternion direction = Quaternion.Euler(-90f, 0f, 0f);
        
                Instantiate(playerMeshes[GameManager.selectPlayer], Vector3.zero, direction, playerObj.transform);
            }

        GameManager.ResetGame(); 
    }
}