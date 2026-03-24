using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDistance = 5f;

    private Vector2 mousePos;
    private Vector3 targetPos;
    private Vector3 lastDir = Vector3.forward;

    private bool isMoving;
    private bool isSprinting;

    [Header("패링 상태")]
    public bool isParryingLeft;  // Q 키 상태
    public bool isParryingRight; // E 키 상태

    public void OnPoint(InputValue value) => mousePos = value.Get<Vector2>();

    public void OnClick(InputValue value)
    {
        if (value.isPressed && Physics.Raycast(Camera.main.ScreenPointToRay(mousePos), out RaycastHit hit))
        {
            if (hit.collider.gameObject != gameObject)
            {
                targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                isMoving = true;
            }
        }
    }

    public void OnSprint(InputValue value) => isSprinting = value.isPressed;

    public void OnDash(InputValue value)
    {
        if (value.isPressed)
        {
            isMoving = false;
            transform.position += lastDir * dashDistance;
        }
    }

    // 패링
    public void OnParryLeft(InputValue value) => isParryingLeft = value.isPressed;
    public void OnParryRight(InputValue value) => isParryingRight = value.isPressed;

    void Update()
    {
        if (isMoving)
        {
            Vector3 diff = targetPos - transform.position;
            float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);

            if (dist < 0.1f)
            {
                isMoving = false;
                return;
            }

            // 방향 정규화 (Normalize) 직접 계산
            lastDir = diff / dist;

            // 스프린트 시 속도 2배 적용
            float currentSpeed = isSprinting ? moveSpeed * 2f : moveSpeed;
            transform.position += lastDir * currentSpeed * Time.deltaTime;

            // 이동 방향을 바라보게 회전 (선택 사항)
            if (lastDir != Vector3.zero)
            {
                transform.forward = lastDir;
            }
        }
    }
}