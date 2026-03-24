using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyType { White, Yellow, Red }

    [Header("적 타입 설정")]
    public EnemyType type;
    public Transform player;

    [Header("시야 및 이동 수치")]
    public float viewAngle = 60f;    // 시야각
    public float viewDistance = 5f; // 시야 거리
    public float rotSpeed = 30f;     // 회전 속도 
    public float moveSpeed = 3f;     // 돌진 속도

    private bool isChasing = false;

    // 내적 (Vector3.Dot 대신 사용)
    float CustomDot(Vector3 a, Vector3 b)
    {
        return (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    }

    // 외적 (Vector3.Cross 대신 사용)
    Vector3 CustomCross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.y * b.z - a.z * b.y,
            a.z * b.x - a.x * b.z,
            a.x * b.y - a.y * b.x
        );
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            SearchPlayer();
        }
    }

    void SearchPlayer()
    {
        // 1. 매 초 회전 (rotSpeed 사용)
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);

        if (player == null) return;

        Vector3 toPlayer = (player.position - transform.position).normalized;
        float dot = CustomDot(transform.forward, toPlayer);

        // 내적값으로 각도 계산
        float angle = Mathf.Acos(Mathf.Clamp(dot, -1f, 1f)) * Mathf.Rad2Deg;
        float dist = Vector3.Distance(transform.position, player.position);

        // 시야 판정
        if (angle < viewAngle / 2f && dist < viewDistance)
        {
            isChasing = true;
        }
    }

    void ChasePlayer()
    {
        // 플레이어에게 돌진
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        // 거리가 가까워지면 패링 체크
        if (Vector3.Distance(transform.position, player.position) < 1.2f)
        {
            CheckParry();
        }
    }

    void CheckParry()
    {
        var pc = player.GetComponent<PlayerController>();
        if (pc == null) return;

        // 적에서 플레이어를 향하는 방향 벡터 계산 (정규화)
        Vector3 toPlayer = (player.position - transform.position).normalized;

        Vector3 cross = CustomCross(transform.forward, toPlayer);

        // 유니티 왼손 좌표계 기준: 외적의 Y값이 0보다 작으면 플레이어는 적의 왼쪽
        bool isPlayerLeft = cross.y < 0;

        if ((isPlayerLeft && pc.isParryingLeft) || (!isPlayerLeft && pc.isParryingRight))
        {
            Debug.Log("<color=green>패링 성공! 적을 제거합니다.</color>");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("<color=red>패링 실패! 시작 지점으로 돌아갑니다.</color>");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnDrawGizmos()
    {
        // 1. 적의 위치와 정면 방향 설정
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward * viewDistance;

        Gizmos.color = Color.red; // 2. 정면 화살표 (빨간색)
        Gizmos.DrawRay(pos, forward);

        Vector3 rightArrowHead = Quaternion.Euler(0, 160, 0) * transform.forward * 0.5f;
        Vector3 leftArrowHead = Quaternion.Euler(0, -160, 0) * transform.forward * 0.5f;

        Gizmos.DrawRay(pos + forward, rightArrowHead);
        Gizmos.DrawRay(pos + forward, leftArrowHead);

        Gizmos.color = Color.yellow; // 3. 시야각 부채꼴 경계선 (노란색)
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2f, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2f, 0) * forward;

        Gizmos.DrawRay(pos, leftBoundary);
        Gizmos.DrawRay(pos, rightBoundary);
    }
}