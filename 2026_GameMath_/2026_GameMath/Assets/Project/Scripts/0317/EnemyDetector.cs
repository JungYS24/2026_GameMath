using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [Header("설정")]
    public Transform player;         // 플레이어 트랜스폼
    public float viewAngle = 60f;    // 시야각 (전체 각도)
    public float viewDistance = 5f;  // 최대 시야 거리

    private Vector3 originalScale;   // 원래 크기 저장용

    void Start()
    {
        originalScale = transform.localScale; // 처음 시작할 때의 크기를 저장
    }

    void Update()
    {
        if (player == null) return;

        // 1. 적과 플레이어 사이의 거리 계산 (Vector3.Distance)
        float distance = Vector3.Distance(transform.position, player.position);

        // 2. 적의 정면 벡터와 플레이어를 향하는 방향 벡터 계산
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        // 3. 내적을 이용해 두 벡터 사이의 각도 계산
        float dot = Vector3.Dot(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        // 4. 거리 조건(이하) AND 시야각 조건(절반 이하) 판별
        if (distance <= viewDistance && angle <= viewAngle / 2f)
        {
            // 시야 안에 들어오면 크기를 2배로 키움
            transform.localScale = Vector3.one * 2f;
            Debug.Log("<color=red>플레이어 포착! 적의 크기 증가</color>");
        }
        else
        {
            transform.localScale = originalScale; // 시야를 벗어나면 원래 크기로 복구
        }
    }

    // 에디터 뷰에서 시야 거리를 시각적으로 확인하기 위함
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
    }
}