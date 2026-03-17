using UnityEngine;

public class SolarSystem : MonoBehaviour
{
    [Header("공전 설정")]
    public Transform center;       // 공전의 중심 (태양 또는 지구)
    public float orbitRadius = 5f; // 궤도 반지름
    public float orbitSpeed = 2f;  // 공전 속도

    private float currentAngle = 0f;

    void Update()
    {
        if (center == null) return;

        // 1. 시간에 따라 각도 업데이트 (라디안 단위로 누적)
        currentAngle += orbitSpeed * Time.deltaTime;

        // 2. 삼각함수를 이용한 궤도 좌표 계산 (X, Z 평면 공전)
        float x = Mathf.Cos(currentAngle) * orbitRadius;
        float z = Mathf.Sin(currentAngle) * orbitRadius;

        // 3. 중심점의 위치에 계산된 좌표를 더해 최종 위치 결정
        // Y값은 중심점과 동일하게 유지하여 수평 공전 구현
        transform.position = new Vector3(center.position.x + x, center.position.y, center.position.z + z);
    }
}