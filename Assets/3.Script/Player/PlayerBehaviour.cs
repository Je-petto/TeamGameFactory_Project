using CustomInspector;
using UnityEngine;
using UnityEngine.Timeline;
using System.Collections.Generic;

// Rigidbody 컴포넌트가 필요함을 명시적으로 표시
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{
    [Header("Key Code")]
    private KeyCode KEYCODEABILITY = KeyCode.Space;
    [Header("Player Data")]
    public List<PlayerData> data;
    [ReadOnly] private int maxHealth = 100;
    [ReadOnly] private float xMoveSpeed = 1;
    [ReadOnly] private float jumpForce = 5f;

    // PlayerData에서 가져온 현재 플레이어의 어빌리티 에셋 참조
    private Ability currentAbilityAsset; // PlayerData에 Ability ability; 필드가 있어야 함
    private float currentAbilityLastUseTime = -Mathf.Infinity;

    // movementLimits는 Rect (x, y, width, height) 형태로 정의되어 있어.
    // x = xMin, y = yMin, width = xMax - xMin, height = yMax - yMin
    // 예시: x 범위 -7 ~ 7, y 범위 0 ~ 10
    // 네가 설정한 Rect(-5, 0, 1, 1)는 x=-5~-4, y=0~1 범위로 매우 좁아. 의도한 범위가 맞는지 확인해봐!
    public Rect movementLimits = new Rect(-5, 0, 10, 1);
    public float yAxisLimit = 5f;

    public int health;

    // Rigidbody 컴포넌트 참조 변수
    private Rigidbody rb;
    private Coroutine activeAbilityCoroutine = null;

    [ReadOnly] public bool canUse;
    [SerializeField] UIManager ui;

    // 점프 쿨타임이나 바닥 체크 변수가 있으면 좋겠지만, 일단은 기본 기능만 수정.

    //==================================================================================
    void Awake()
    {
        health = maxHealth;

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
            PlayerData selectedData = data[GameManager.selectPlayer];

            // 현재 플레이어의 어빌리티 에셋 참조 가져오기
            currentAbilityAsset = selectedData.ability;
        }
        maxHealth = data[GameManager.selectPlayer].maxHealth;
        xMoveSpeed = data[GameManager.selectPlayer].xMoveSpeed;
        jumpForce = data[GameManager.selectPlayer].jumpForce;
    }
    void MoveMouseWithinLimits()
    {
        if (GameManager.isPause) return;
        Vector3 moveDelta = new Vector3(Input.GetAxis("Mouse X") * xMoveSpeed, 0, Input.GetAxis("Mouse Y") * xMoveSpeed);
        transform.Translate(moveDelta);

        // 오브젝트의 현재 위치를 가져와서 경계 안에 있는지 확인
        Vector3 currentPosition = transform.position;

        float clampedX = Mathf.Clamp(currentPosition.x, movementLimits.xMin, movementLimits.xMax);
        float clampedZ = Mathf.Clamp(currentPosition.z, movementLimits.yMin, movementLimits.yMax);

        transform.position = new Vector3(clampedX, transform.position.y, clampedZ);
    }

    // 점프 입력 감지 및 처리
    void HandleJumpInput() // 메소드 이름 변경
    {
        // 마우스 왼쪽 버튼 클릭 감지
        if (Input.GetMouseButtonDown(0) && !GameManager.isPause)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Input.GetMouseButtonDown(1))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.down * jumpForce, ForceMode.Impulse);
        }
        if (transform.position.y > yAxisLimit)
            transform.position = new Vector3(transform.position.x, yAxisLimit, transform.position.z);
    }
    void UseAbility()
    {
        // GameManager.Instance 사용 및 isLive, isPause 체크
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

                    // 어빌리티 사용이 가능하다면
                    if (canUse)
                    {
                        // *** 어빌리티 사용 로직 시작 ***
                        // 마지막 사용 시간 업데이트 (쿨타임 시작)
                        currentAbilityLastUseTime = Time.time;

                        // 어빌리티 에셋의 ActivateAbility 코루틴 시작
                        // PlayerBehaviour 인스턴스 (this)를 넘겨주어 코루틴을 시작하게 합니다.
                        // ActivateAbility 코루틴 자체에는 user GameObject만 넘겨주도록 했습니다.
                        activeAbilityCoroutine = StartCoroutine(currentAbilityAsset.ActivateAbility(gameObject)); // PlayerBehaviour가 아닌 gameObject를 넘겨줌

                        // TODO: 어빌리티 사용 시작 UI, 사운드 효과 등 추가
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
        canUse = Time.time >= currentAbilityLastUseTime + currentAbilityAsset.coolDown;
    }

    public void OnDamage(int damage)
    {
        if (health - damage < 0) health = 0;
        else health -= damage;
        ui.DamageUI();
    }
    public void OnHealing(int heal)
    {
        if (health + heal > maxHealth) health = maxHealth;
        else health += heal;

    }
    private void Death()
    {
        if (health == 0 || transform.position.y < -10f)
        {
            GameManager.isLive = false;
            Time.timeScale = 0f;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Collectable")
        {
            Collectable item = col.GetComponent<Collectable>();
            if (item.data.type == CollectableType.HEALTH)
                OnHealing(((CollectableHealth)item).gainHealth);
            else if (item.data.type == CollectableType.SCORE)
                GameManager.GainScore(((CollectableScore)item).gainScore);
            else if (item.data.type == CollectableType.DOBS)
                item.ClearObstacles();



        }
        else if (col.gameObject.tag == "Obstacle")
        {
            Obstacle obs = col.GetComponent<Obstacle>();
            OnDamage(obs.data.damage);
        }

        Destroy(col.gameObject);
    }
}