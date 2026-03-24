using UnityEngine;

public class EnemyCross : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null) return;
        Vector3 forward = transform.forward;// 1. 나의 정면 방향 벡터

        // 2. 나에서 타겟을 향하는 방향 벡터 (정규화)
        Vector3 dirToTarget = (target.position - transform.position).normalized;

        // 3. 두 벡터를 외적 (결과값은 두 벡터에 수직인 벡터)
        Vector3 crossProduct = Vector3.Cross(forward, dirToTarget);

        // 4. 외적 결과의 Y값 부호에 따라 좌우 판별
        if (crossProduct.y > 0.1f)
        {
            Debug.Log("<color=cyan>적이 오른쪽에 있습니다.</color>");
        }
        else if (crossProduct.y < -0.1f)
        {
            Debug.Log("<color=magenta>적이 왼쪽에 있습니다.</color>");
        }
    }
}