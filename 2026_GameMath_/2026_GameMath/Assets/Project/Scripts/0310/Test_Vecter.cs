using UnityEngine;

public class Test_Vector : MonoBehaviour
{
    [Header("실습용 대상")]
    public Transform target; // 에디터에서 다른 오브젝트를 할당하세요.

    void Start()
    {
        // 1. 벡터의 생성
        Vector3 myPos = transform.position;
        Debug.Log($"내 현재 위치: {myPos}");
    }

    void Update()
    {
        // 실습 1: 벡터의 뺄셈 (방향 구하기)
        // 공식: 타겟 위치 - 내 위치 = 나로부터 타겟으로 향하는 화살표
        if (target != null)
        {
            Vector3 directionToTarget = target.position - transform.position;

            // 실습 2: 벡터의 크기 (거리)
            float distance = directionToTarget.magnitude;

            // 실습 3: 벡터의 정규화 (방향 데이터만 남기기)
            // 크기가 1인 단위 벡터로 만듭니다. (이동 속도 계산 시 필수)
            Vector3 normalizedDir = directionToTarget.normalized;

            // --- 시각화 (Scene 뷰에서 확인 가능) ---
            // 나(빨간색): 타겟까지의 실제 거리와 방향
            Debug.DrawRay(transform.position, directionToTarget, Color.red);

            // 나(파란색): 방향만 나타내는 단위 벡터 (크기가 1이라 짧음)
            Debug.DrawRay(transform.position, normalizedDir * 2f, Color.blue);

            // 콘솔 출력 (너무 자주 찍히지 않게 주의)
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log($"타겟까지 거리: {distance}");
                Debug.Log($"방향 단위 벡터: {normalizedDir}");
            }
        }

        // 실습 4: 벡터의 덧셈 (이동)
        // 매 프레임 오른쪽으로 조금씩 이동하는 벡터 합산
        if (Input.GetKey(KeyCode.D))
        {
            Vector3 moveStep = Vector3.right * 5f * Time.deltaTime;
            transform.position += moveStep;
        }
    }
}