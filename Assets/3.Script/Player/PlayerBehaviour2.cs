using UnityEngine;

// Rigidbody 컴포넌트가 필요함을 명시적으로 표시
[RequireComponent(typeof(Rigidbody))]
public class PlayerBehaviour2 : MonoBehaviour
{
    // public Shooter weapon; // 주석 처리된 부분은 그대로 둠
    public int maxLife = 10;
    public int maxWeaponLevel = 5;
    public float speed = 1;
    // movementLimits는 Rect (x, y, width, height) 형태로 정의되어 있어.
    // x = xMin, y = yMin, width = xMax - xMin, height = yMax - yMin
    // 예시: x 범위 -7 ~ 7, y 범위 0 ~ 10
    // 네가 설정한 Rect(-5, 0, 1, 1)는 x=-5~-4, y=0~1 범위로 매우 좁아. 의도한 범위가 맞는지 확인해봐!
    public Rect movementLimits = new Rect(-5, 0, 1, 1); 
    public float yAxisLimit = 5f;

    [HideInInspector]
    int weaponLevel;
    int life;
    Vector3 initialPosition;
    Quaternion initialRotation;

    // 점프 관련 변수 (jumpRange, jumpDuration 대신 Rigidbody에 사용할 변수로 변경)
    // Rigidbody를 사용해서 점프할 때는 '힘'이나 '속도' 개념을 사용해.
    public float jumpForce = 5f; // 점프할 때 위로 가할 힘의 크기

    // 중력 관련 변수는 Rigidbody.useGravity로 대체
    // public float gravityForce = 0.00001f; // 이 변수는 이제 사용하지 않아.

    // Rigidbody 컴포넌트 참조 변수
    private Rigidbody rb;

    // 점프 쿨타임이나 바닥 체크 변수가 있으면 좋겠지만, 일단은 기본 기능만 수정.

    //==================================================================================
    void Awake()
    {
        life = maxLife;
        initialPosition = transform.position;
        initialRotation = transform.rotation;

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
        // If not Paused
        if (Time.timeScale != 0)
        {
            Vector3 oldPosition = transform.position; 
            // 마우스 이동 및 경계 체크 메소드 호출
            MoveMouseWithinLimits();

            // 점프 입력 감지 및 점프 처리 메소드 호출
            HandleJumpInput(); // 메소드 이름 변경
        }
    }
    void MoveMouseWithinLimits()
    {
        Vector3 moveDelta = new Vector3(Input.GetAxis("Mouse X") * speed, 0, Input.GetAxis("Mouse Y") * speed);
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
}