using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float walkSpeed = 5f;   // 기본 걷기 속도
    public float sprintSpeed = 10f; // 스프린트 시 속도
    private float currentSpeed;     // 현재 적용 중인 속도

    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isSprinting = false;

    void Awake()
    {
        // 시작 시 기본 속도로 설정
        currentSpeed = walkSpeed;
    }

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;
                }
            }
        }
    }

    // 이미지의 OnSprint 메시지를 처리하는 메서드
    public void OnSprint(InputValue value)
    {
        // 버튼을 누르고 있으면 true, 떼면 false가 들어옵니다.
        isSprinting = value.isPressed;

        // 스프린트 여부에 따라 현재 속도 변경
        currentSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Debug.Log(isSprinting ? "스프린트 시작!" : "스프린트 중단");
    }

    void Update()
    {
        if (isMoving)
        {
            // 1. 방향 벡터 계산
            Vector3 direction = targetPosition - transform.position;

            // 2. 크기(거리) 직접 계산
            float sqrMag = (direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z);
            float magnitude = Mathf.Sqrt(sqrMag);

            // 3. 도착 판정
            if (magnitude < 0.05f)
            {
                isMoving = false;
                return;
            }

            // 4. 정규화 직접 구현 (방향 / 크기)
            Vector3 normalizedDir = direction / magnitude;

            // 5. 최종 이동 (currentSpeed 적용)
            transform.position += normalizedDir * currentSpeed * Time.deltaTime;
        }
    }
}