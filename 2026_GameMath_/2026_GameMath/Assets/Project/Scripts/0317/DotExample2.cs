using UnityEngine;

public class DotExample2 : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f;

    void Update()
    {
        if (player == null) return;

        // 1. 방향 벡터 계산 및 정규화
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        // 2. Vector3.Dot을 사용하지 않고 직접 내적(Dot Product) 계산
        // 공식: x1*x2 + y1*y2 + z1*z2
        float dot = (forward.x * toPlayer.x) + (forward.y * toPlayer.y) + (forward.z * toPlayer.z);

        // 3. 내적 결과를 아크코사인(Acos)으로 역산하여 사이 각도(Radian)를 구함
        // 그 후 Rad2Deg를 곱해 우리가 아는 각도(Degree)로 변환
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 4. 시야각 판별 (시야각의 절반보다 작으면 범위 내에 있는 것)
        if (angle < viewAngle / 2f)
        {
            Debug.Log("<color=yellow>플레이어가 시야 안에 있음!</color>");
        }
    }
}