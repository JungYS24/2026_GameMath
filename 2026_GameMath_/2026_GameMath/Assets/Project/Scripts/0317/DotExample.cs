using UnityEngine;

public class DotExample : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        // 1. 적에서 플레이어로 가는 방향 벡터 계산
        Vector3 toPlayer = player.position - transform.position;
        toPlayer.y = 0f; // 높이 차이는 무시 (평면 판정)

        // 2. 적의 정면(앞) 방향 벡터
        Vector3 forward = transform.forward;
        forward.y = 0f;

        // 3. 정확한 내적 계산을 위해 두 벡터를 단위 벡터로 정규화
        forward.Normalize();
        toPlayer.Normalize();

        // 4. 두 벡터를 내적 (결과값은 -1 ~ 1 사이)
        float dot = Vector3.Dot(forward, toPlayer);

        // 5. 내적 결과에 따른 방향 판별
        if (dot > 0f)
        {
            Debug.Log("<b><color=yellow>플레이어가 적의 앞쪽에 있음:</color></b>");
        }
        else if (dot < 0f)
        {
            Debug.Log("<b><color=blue>플레이어가 적의 뒤쪽에 있음:</color></b>");
        }
        else
        {
            Debug.Log("<b><color=red>플레이어가 적의 옆쪽에 있음:</color></b>");
        }
    }
}