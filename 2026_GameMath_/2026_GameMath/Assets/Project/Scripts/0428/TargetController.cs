using UnityEngine;
using UnityEngine.InputSystem;

public class TargetController : MonoBehaviour
{
    [Header("연결 설정")]
    [SerializeField] private CameraSlerp cameraSlerp;
    [SerializeField] private PredictionLineRender predLine;

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (cameraSlerp != null)
                {
                    cameraSlerp.target = hit.transform;
                }

                if (predLine != null)
                {
                    predLine.endPos = hit.transform;
                    predLine.gameObject.SetActive(true);
                }
            }
            else
            {
                ClearTargeting();
            }
        }
        else
        {
            ClearTargeting();
        }
    }

    private void ClearTargeting()
    {
        if (cameraSlerp != null)
        {
            cameraSlerp.target = null;
        }

        if (predLine != null)
        {
            predLine.endPos = null;
            predLine.gameObject.SetActive(false);
        }
    }
}