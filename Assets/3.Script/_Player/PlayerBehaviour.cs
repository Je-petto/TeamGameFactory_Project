using CustomInspector;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// Rigidbody 컴포넌트가 필요함을 명시적으로 표시
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Key Code")]
    private KeyCode KEYCODEABILITY = KeyCode.Space; // 능력을 사용하기 위한 키를 미리 정의해둠
    [Header("Player Data")]
    public List<PlayerData> data;   // GameManager.selectPlayer에 의하여 data의 요소들을 꺼내서 쓰기 위해 선언.

    // Player Data에서 가져온 데이터를 여기서 쓰기 위해 다시 정의(선언)함.
    [ReadOnly] private int maxHealth = 0;
    [ReadOnly] private float xMoveSpeed = 0;
    [ReadOnly] private float jumpForce = 0;
    [ReadOnly] private Ability currentAbilityAsset; // PlayerData에 Ability ability; 필드가 있어야 함
    

    // PlayerData에서 가져온 현재 플레이어의 어빌리티 에셋 참조

    // if(현재 시간(0f) > 전에 능력을 사용한 시간 때(-****f) + 현재 능력의 굴다운 시간(6f)
    private float currentAbilityLastUseTime;
    

    // movementLimits는 Rect (x, y, width, height) 형태로 정의되어 있어.
    // x = xMin, y = yMin, width = xMax - xMin, height = yMax - yMin
    public Rect movementLimits = new Rect(-5, 0, 10, 1);
    public float yAxisLimit = 5f;

    // 현재 Health
    [ReadOnly] public int health;

    // Rigidbody 컴포넌트 참조 변수

    // 물리적인 힘을 사용해서 캐릭터를 움직임
    // 또한 중력을 적용하기 위해서 넣음.
    private Rigidbody rb;

    private Coroutine activeAbilityCoroutine = null;

    // 능력 사용할 수 있는 지 판단하기 위한 Bool 데이터
    [ReadOnly] public bool canUse;
    [SerializeField] private UIManager ui;
    [SerializeField] private GameOverManager gameOverManager;

    // 점프 쿨타임이나 바닥 체크 변수가 있으면 좋겠지만, 일단은 기본 기능만 수정.

    //==================================================================================
    void Awake()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 필요합니다!");
            enabled = false;
        }
    }
    void Start()
    {
        SetupData();
        currentAbilityLastUseTime = -Mathf.Infinity; // 게임 시작 시 바로 사용 가능하도록 초기화
    }

    void Update()
    {
        if (GameManager.isLive)
        {
            Vector3 oldPosition = transform.position;
            // 마우스 이동 및 경계 체크 메소드 호출
            MoveMouseWithinLimits();
            // 능력을 사용할 수 있는 지 판단 메서드 / 능력을 사용하는 메서드
            CanUseAbility();
            UseAbility();
            // 점프 입력 감지 및 점프 처리 메소드 호출
            HandleJumpInput(); // 메소드 이름 변경
            Death();
        }
    }

    void SetupData()
    {
        if (data != null && GameManager.selectPlayer >= 0 && GameManager.selectPlayer < data.Count)
        {
            // 현재 플레이어의 어빌리티 에셋 참조 가져오기
            currentAbilityAsset = data[GameManager.selectPlayer].ability;
            maxHealth = data[GameManager.selectPlayer].maxHealth;
            xMoveSpeed = data[GameManager.selectPlayer].xMoveSpeed;
            jumpForce = data[GameManager.selectPlayer].jumpForce;

            health = maxHealth;
        }
    }
    void MoveMouseWithinLimits()
    {
        // 게임이 일시정지됐을 때, 이동하지 않기 위한 방어 코드
        if (GameManager.isPause) return;

        // 마우스의 X와 Y값을 Input으로 받아서 Vector3로 저장함.
        Vector3 moveDelta = new Vector3(Input.GetAxis("Mouse X") * xMoveSpeed, 0f, Input.GetAxis("Mouse Y") * xMoveSpeed);
        // 그 Vector3 값으로 그냥 이동.
        transform.Translate(moveDelta);

        // 오브젝트의 현재 위치를 가져와서 경계 안에 있는지 확인
            Vector3 currentPosition = transform.position;

            // 경계 범위 내로 현재 위치를 좌표를 깎아냄.
            float clampedX = Mathf.Clamp(currentPosition.x, movementLimits.xMin, movementLimits.xMax);
            float clampedZ = Mathf.Clamp(currentPosition.z, movementLimits.yMin, movementLimits.yMax);
            
            // 현재 위치를 깎아낸 좌표로 재수정함.
            transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    // 점프 입력 감지 및 처리
    void HandleJumpInput()
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0) && !GameManager.isPause)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // 위로 만 이동할 수 있게 Vector3.up(0f, 1f, 0f)로 힘을 가함.
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            // 위로 만 이동할 수 있게 Vector3.down(0f, -1f, 0f)로 힘을 가함.
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        }

        // 만약 경계에서 벗어나면?
        if (transform.position.y > yAxisLimit)
        // 좌표 깎아냄.
            transform.position = new Vector3(transform.position.x, yAxisLimit, transform.position.z);
    }

    void UseAbility()
    {
        // 캐릭터가 살아있는지? 일시정지 안 되어있는지?
        if (GameManager.isLive && !GameManager.isPause)
        {
            // 어빌리티 사용 키 입력 감지
            if (Input.GetKeyDown(KEYCODEABILITY))
            {
                // 현재 플레이어에게 어빌리티 에셋이 할당되어 있는지 확인
                if (currentAbilityAsset != null)
                {
                    // *** 쿨타임 체크 로직 ***
                    // 현재 시간 >= 마지막 사용 시간 + 어빌리티 에셋의 쿨다운 시간
                    // canUse = Time.time >= currentAbilityLastUseTime + currentAbilityAsset.coolDown;

                    // 디버그 로그로 쿨타임 상태 확인
                    Debug.Log($"어빌리티 사용 시도: 현재 시간 {Time.time:F2}, 마지막 사용 시간 {currentAbilityLastUseTime:F2}, 쿨다운 {currentAbilityAsset.coolDown:F2}. 사용 가능? {canUse}");

                    // 어빌리티 사용이 가능하다면 true
                    if (canUse)
                    {
                        // *** 어빌리티 사용 로직 시작 ***
                        // 마지막으로 능력을 사용한 시간 업데이트 (쿨타임 시작)
                        currentAbilityLastUseTime = Time.time;

                        // 어빌리티 에셋의 ActivateAbility 코루틴 시작
                        // PlayerBehaviour 인스턴스 (this)를 넘겨주어 코루틴을 시작하게 합니다.
                        // ActivateAbility 코루틴 자체에는 user GameObject만 넘겨주도록 했습니다.
                        activeAbilityCoroutine = StartCoroutine(currentAbilityAsset.ActivateAbility(gameObject)); // PlayerBehaviour가 아닌 gameObject를 넘겨줌
                    }
                    else
                        // 쿨타임 중이라면 남은 시간 표시 (소수점 둘째 자리까지)
                        Debug.LogWarning($"어빌리티 사용 불가능: 쿨다운 중입니다. 남은 시간: {currentAbilityLastUseTime + currentAbilityAsset.coolDown - Time.time:F2} 초");
                    // TODO: 어빌리티 사용 불가능 UI나 사운드 효과
                }
                else
                    Debug.LogWarning("선택된 플레이어에게 할당된 어빌리티 에셋이 없습니다.");
            }
        }
    }
    void CanUseAbility()
    {
        // 능력 쿨다운 확인용 Bool 데이터 관리
        // bool 데이터 = 현재 시간 >= 마지막에 능력을 사용한 시간(능력을 사용할 때마다 최신화됨) + 현재 굴다운 시간
        canUse = Time.time >= currentAbilityLastUseTime + currentAbilityAsset.coolDown;
    }

    // 장애물과 충돌할 때 피해를 받는 메서드
    public void OnDamage(int damage)
    {
        if (health - damage < 0) health = 0;
        else health -= damage;
        ui.DamageUI();
    }
    
    // 체력 아이템 먹었을 때 회복하는 메서드
    public void OnHealing(int heal)
    {
        if (health + heal > maxHealth) health = maxHealth;
        else health += heal;

    }

    private void Death()
    {
        if (health == 0 || transform.position.y < -yAxisLimit * 1.2f)
        {
            GameManager.isLive = false;
            Time.timeScale = 0f;

            gameOverManager.ShowGameOverUI(); // GameOver UI 띄우기
        }
    }
    
    // 장애물 혹은 아이템과 충돌했을 때만 실행되는 메서드
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Collectable")
        {
            // 일단 어떤 아이템을 먹었는 지는 모르겠고, 일단 스크립트만 가져옴.
            Collectable item = col.GetComponent<Collectable>();

            // 어떤 아이템을 먹었는지 확인하기 위해 Type받아서 직접 확인함.
            if (item.data.type == CollectableType.HEALTH) // 체력 +
                OnHealing((int)(((CollectableHealth)item).gainHealth * GameManager.collectableIncresePersent));

            else if (item.data.type == CollectableType.SCORE) // 점수 +
                GameManager.GainScore((int)(((CollectableScore)item).gainScore * GameManager.collectableIncresePersent));

            else if (item.data.type == CollectableType.DOBS) // 나와있는 장애물 전체 삭제
                item.ClearObstacles();
                
            else if (item.data.type == CollectableType.REVERS) // 아이템 획득시 리버스
            {
                CollectableRevers reversItem = item as CollectableRevers;
                if (reversItem != null)
                    reversItem.Reverse(this);
            }

        }
        // Invincible 능력이 켜져있을 때는 false되서 Obstacle과 충돌하지 않음.
        
        else if (col.gameObject.tag == "Obstacle" && !GameManager.isInvincible)
        {
            Obstacle obs = col.GetComponent<Obstacle>();
            OnDamage(obs.data.damage);
        }


        Destroy(col.gameObject);
    }
    public IEnumerator ReverseMovement(float duration)
    {
        xMoveSpeed *= -1; // 반전
        yield return new WaitForSeconds(duration);
        xMoveSpeed *= -1; // 원래대로 복원
    }
}