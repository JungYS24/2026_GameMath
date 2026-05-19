using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycastTest : MonoBehaviour
{
    public float rayDistance = 100f;
    public float hitForce = 15f;

    public void OnClick(InputValue value)
    {
        if (!value.isPressed) return;

        Debug.Log("마우스 클릭 감지됨!");

        if (!GameManager.Instance.canInput) return;

        // 마우스 스크린 좌표를 레이(Ray)로 변환
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;

            if (rb != null)
            {
                if (!GameManager.Instance.IsValidBallForCurrentTurn(rb.gameObject)) return;

                // 힘의 방향 = 공의 중심 - 마우스로 클릭한 표면 지점
                Vector3 ballCenter = rb.transform.position;
                Vector3 clickPoint = hit.point;

                // Y축을 동일하게 맞춰 수평 힘만 가해지도록 제한 (마쎄이 방지)
                clickPoint.y = ballCenter.y;

                Vector3 pushDirection = (ballCenter - clickPoint).normalized;

                // 순간적인 충격량(Impulse)으로 타격
                rb.AddForce(pushDirection * hitForce, ForceMode.Impulse);

                GameManager.Instance.OnBallHit();
            }
        }
    }
}