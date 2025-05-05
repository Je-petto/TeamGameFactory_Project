using CustomInspector;
using UnityEngine;
using UnityEngine.Timeline;
using System.Collections.Generic;

// Rigidbody 컴포넌트가 필요함을 명시적으로 표시
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour : MonoBehaviour
{ 
    [Header("Player Data")]
    public List<PlayerData> data;
    [ReadOnly] private int maxHealth = 100;
    [ReadOnly] private float xMoveSpeed = 1;
    [ReadOnly] private float jumpForce = 5f;

    // movementLimits는 Rect (x, y, width, height) 형태로 정의되어 있어.
    // x = xMin, y = yMin, width = xMax - xMin, height = yMax - yMin
    // 예시: x 범위 -7 ~ 7, y 범위 0 ~ 10
    // 네가 설정한 Rect(-5, 0, 1, 1)는 x=-5~-4, y=0~1 범위로 매우 좁아. 의도한 범위가 맞는지 확인해봐!
    public Rect movementLimits = new Rect(-5, 0, 10, 1); 
    public float yAxisLimit = 5f;

    public int health;

    // Rigidbody 컴포넌트 참조 변수
    private Rigidbody rb;
    [SerializeField] UIManager ui;


    // 점프 쿨타임이나 바닥 체크 변수가 있으면 좋겠지만, 일단은 기본 기능만 수정.

    //==================================================================================
    void Awake()
    {
        SetupData();
        health = maxHealth;

        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody 컴포넌트가 필요합니다!");
            enabled = false;
        }
    }

    void Update()
    {
        if (GameManager.isLive)
        {
            Vector3 oldPosition = transform.position; 
            // 마우스 이동 및 경계 체크 메소드 호출
            MoveMouseWithinLimits();

            // 점프 입력 감지 및 점프 처리 메소드 호출
            HandleJumpInput(); // 메소드 이름 변경
            Death();
        }
    }

    void SetupData()
    {
        maxHealth = data[GameManager.selectPlayer].maxHealth;
        xMoveSpeed = data[GameManager.selectPlayer].xMoveSpeed;
        jumpForce = data[GameManager.selectPlayer].jumpForce;
    }
    void MoveMouseWithinLimits()
    {
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
        if (Input.GetMouseButtonDown(0))
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
            /*
                GameOver 처리
            */
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
        }
        else if (col.gameObject.tag == "Obstacle")
        {
            Obstacle obs = col.GetComponent<Obstacle>();
            OnDamage(obs.data.damage);
        }
        
        Destroy(col.gameObject);
    }
}