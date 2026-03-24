using UnityEngine;

public class FOVVisualizer : MonoBehaviour
{
    public float viewAngle = 60f;
    public float viewDistance = 5f;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // 시야 거리만큼 확장된 정면 벡터
        Vector3 forward = transform.forward * viewDistance;

        // 왼쪽 시야 경계: 정면 벡터를 Y축 기준으로 -viewAngle / 2만큼 회전
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;

        // 오른쪽 시야 경계: 정면 벡터를 Y축 기준으로 viewAngle / 2만큼 회전
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        // 시야 경계선 그리기
        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);

        // 캐릭터 앞쪽 방향 (빨간색 선)
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, forward);
    }
}