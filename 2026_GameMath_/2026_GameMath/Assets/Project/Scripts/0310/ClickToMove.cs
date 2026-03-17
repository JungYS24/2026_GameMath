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

    void Update()
    {
        if (isMoving)
        {
            Vector3 diff = targetPos - transform.position;
            float dist = Mathf.Sqrt(diff.x * diff.x + diff.y * diff.y + diff.z * diff.z);

            if (dist < 0.1f) isMoving = false;

            lastDir = diff / dist; // Normalize direction
            transform.position += lastDir * (isSprinting ? moveSpeed * 2f : moveSpeed) * Time.deltaTime; //스프린트 이동속도 2배
        }
    }
}