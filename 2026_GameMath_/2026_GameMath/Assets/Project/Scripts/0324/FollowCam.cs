using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 4f, -5f);

    public float smoothSpeed = 5f;
    void LateUpdate()
    {
        if (target == null) return;

        // 1. 타겟의 Y축 회전값을 고려하여 오프셋 위치를 계산
        Vector3 latePosition = target.position + Quaternion.Euler(0f, target.eulerAngles.y, 0f) * offset;

        transform.position = latePosition; // 2. 계산된 위치로 카메라 이동
        transform.LookAt(target); // 3. 카메라가 항상 타겟을 바라보도록 설정
    }
}