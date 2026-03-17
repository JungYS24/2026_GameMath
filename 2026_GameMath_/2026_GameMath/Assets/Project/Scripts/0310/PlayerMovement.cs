using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    Vector3 normalizedVector;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        Vector3 direction = new Vector3(moveInput.x, moveInput.y, 0);

        float sqrMagnitude = direction.x * direction.x + direction.y * direction.y + direction.z * direction.z;
        float magnitude = Mathf.Sqrt(sqrMagnitude);

        // (0으로 나누기 방지)
        if (magnitude > 0)
        {
            normalizedVector = direction / magnitude;
        }
        else
        {
            normalizedVector = Vector3.zero;
        }

        transform.Translate(normalizedVector * moveSpeed * Time.deltaTime);
    }
}