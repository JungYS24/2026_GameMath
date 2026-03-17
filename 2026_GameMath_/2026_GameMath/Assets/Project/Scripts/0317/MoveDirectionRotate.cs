using UnityEngine;
using UnityEngine.InputSystem;

public class MoveDirectionRotate : MonoBehaviour
{
    private Vector2 moveInput;

    // Input System에서 Move 액션이 발생할 때 호출됩니다.
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        // 입력 벡터의 길이가 0이 아닐 때만 회전 (입력이 멈췄을 때 원래 방향 유지)
        if (moveInput.sqrMagnitude > 0.01f)
        {
            // Atan2(y, x)는 라디안 값을 반환하므로 Rad2Deg를 곱해 도로 변환합니다.
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;

            // 계산된 각도를 이용해 Z축 방향으로 회전시킵니다 (2D 탑다운 기준).
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}