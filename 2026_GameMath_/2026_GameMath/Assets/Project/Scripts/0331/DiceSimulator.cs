using UnityEngine;

public class DiceSimulator : MonoBehaviour
{
    int[] counts = new int[6];
    public int trials = 100;

    void Start()
    {
        // 지정된 횟수(trials)만큼 주사위를 던짐
        for (int i = 0; i < trials; i++)
        {
            int result = Random.Range(1, 7); // 1~6 사이의 난수 발생
            counts[result - 1]++; // 결과값에 해당하는 배열 인덱스의 카운트 증가
        }

        // 결과 통계 출력
        for (int i = 0; i < counts.Length; i++)
        {
            // 확률 계산 (실수형 변환 주의)
            float percent = (float)counts[i] / trials * 100f;

            // 보간 문자열($)과 서식 지정자(F2)를 사용한 로그 출력
            Debug.Log($"{i + 1}: {counts[i]}회 ({percent:F2}%)");
        }
    }
}