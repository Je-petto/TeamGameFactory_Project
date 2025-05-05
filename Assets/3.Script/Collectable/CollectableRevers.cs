using UnityEngine;

public class CollectableRevers : Collectable
{
    public float reverseDuration = 5f; // 반전 지속 시간
    private bool isReversed = false;
    private float timer = 0f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        if (isReversed)
        {
            timer += Time.deltaTime;
            if (timer >= reverseDuration)
            {
                ReverseControls(false);  // 반전 해제
            }
        }
    }

    public void ReverseControls(bool reverse)
    {
        // 반전 상태 설정
        PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
        
        if (reverse)
        {
            // PlayerData에서 xMoveSpeed와 jumpForce 반전
            player.data[GameManager.selectPlayer].xMoveSpeed *= -1;
            player.data[GameManager.selectPlayer].jumpForce *= -1;
            isReversed = true;
            timer = 0f;
        }
        else
        {
            // 반전 해제 (원래 값으로 복귀)
            player.data[GameManager.selectPlayer].xMoveSpeed *= -1;
            player.data[GameManager.selectPlayer].jumpForce *= -1;
            isReversed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReverseControls(true); // 반전 시작
            Destroy(gameObject); // 아이템 삭제
        }
    }
}