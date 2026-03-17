using UnityEngine;
using TMPro;

public class SimpleAngleLauncher : MonoBehaviour
{
    public TMP_InputField angleInputField;
    public GameObject[] spherePrefabs; // 여러 색의 공 프리팹들을 담을 배열
    public Transform firePoint;
    public float force = 15f;

    public void Launch()
    {
        if (angleInputField == null || string.IsNullOrEmpty(angleInputField.text)) return;
        // 등록된 프리팹이 하나도 없으면 실행 안 함
        if (spherePrefabs == null || spherePrefabs.Length == 0) return;

        if (float.TryParse(angleInputField.text, out float angle))
        {
            float rad = angle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Cos(rad), 0f, Mathf.Sin(rad));

            // 0부터 리스트 개수 사이에서 랜덤한 인덱스 선택 - 발사하는 공 색깔 랜덤
            int randomIndex = Random.Range(0, spherePrefabs.Length);
            GameObject selectedPrefab = spherePrefabs[randomIndex];

            // 선택된 랜덤 프리팹 생성
            GameObject sphere = Instantiate(selectedPrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = sphere.GetComponent<Rigidbody>();

            if (rb != null)
            {
                Vector3 launchDirection = (dir + Vector3.up * 0.3f).normalized;
                rb.AddForce(launchDirection * force, ForceMode.Impulse);
            }
        }
    }
}