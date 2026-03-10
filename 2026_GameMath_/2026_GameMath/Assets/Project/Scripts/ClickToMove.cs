using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;

    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }
    public void OnClick(InputValue value)
    {

        if (value.isPressed)
        {
           
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                
                if (hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;

                    isMoving = true;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
           
            Vector3 direction = targetPosition - transform.position;

            float sqrMag = (direction.x * direction.x) + (direction.y * direction.y) + (direction.z * direction.z);
            float magnitude = Mathf.Sqrt(sqrMag);

            if (magnitude < 0.05f)
            {
                isMoving = false;
                return;
            }
            Vector3 normalizedDir = direction / magnitude;
            transform.position += normalizedDir * moveSpeed * Time.deltaTime;
        }
    }
}