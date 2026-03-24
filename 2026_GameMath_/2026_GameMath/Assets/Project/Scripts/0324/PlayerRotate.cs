using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotate : MonoBehaviour
{
    public float rotationSpeed = 100f;
    private Vector2 moveInput;

    // Input System에서 Move 액션(WASD 등)이 발생할 때 호출
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {

        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotationSpeed * Time.deltaTime, 0f);

        transform.rotation = rotation * transform.rotation;
    }
}