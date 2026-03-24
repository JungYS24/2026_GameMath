using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("패링 상태")]
    public bool isParryingLeft;
    public bool isParryingRight;

    // 1. WASD 입력 처리 (Action Name이 'Move'일 때)
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // 2. 패링 입력 처리 (Action Name이 'ParryLeft', 'ParryRight'일 때)
    public void OnParryLeft(InputValue value) => isParryingLeft = value.isPressed;
    public void OnParryRight(InputValue value) => isParryingRight = value.isPressed;

    void Update()
    {
        Move();
    }

    void Move()
    {
        // Vector2 입력을 Vector3로 변환 (X축은 좌우, Z축은 앞뒤)
        Vector3 moveDir = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDir.magnitude > 0.01f)
        {
            // 직접 수식 기반 이동
            transform.position += moveDir * moveSpeed * Time.deltaTime;

            // 이동 방향 바라보기 (부드럽게 회전)
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * 10f);
        }
    }
}