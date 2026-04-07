using UnityEngine;
using TMPro; 

public class GaussianGenerator : MonoBehaviour
{
    [Header("설정")]
    public float mean = 10f;    // 평균 
    public float stdDev = 2f;   // 표준편차 

    [Header("UI 연결")]
    public TextMeshProUGUI resultDisplayText; // 화면에 값을 표시할 TMP 텍스트

    public void GenerateAndShowUI()
    {
        if (resultDisplayText == null)
        {
            Debug.LogError("결과를 표시할 TextMeshProUGUI가 연결되지 않았습니다.");
            return;
        }

        float result = GenerateGaussian(mean, stdDev);

        resultDisplayText.text = $"생성된 난수: <color=yellow>{result:F2}</color>";

        Debug.Log($"<color=cyan>생성된 가우시안 난수: {result:F2}</color>");
    }

    float GenerateGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        // 박스-뮬러 변환 공식 (정규분포 형성)
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }
}