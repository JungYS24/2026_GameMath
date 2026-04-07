using UnityEngine;
using System.Linq;

public class StatisticsTest : MonoBehaviour
{
    [Header("통계 설정")]
    [Tooltip("생성할 샘플 데이터의 개수")]
    public int sampleCount = 1000;

    void Start()
    {
        StandardDeviation();
    }

    void StandardDeviation()
    {
        // 인스펙터에서 설정한 sampleCount를 사용합니다.
        int n = sampleCount;

        // n이 0 이하일 경우 에러 방지
        if (n <= 0)
        {
            Debug.LogWarning("샘플 수는 0보다 커야 합니다.");
            return;
        }

        float[] samples = new float[n];
        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(0f, 1f);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);

        Debug.Log($"[샘플 {n}개] 평균: {mean:F4}, 표준편차: {stdDev:F4}");
    }
}