using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class DiceSimulator : MonoBehaviour
{
    [Header("설정")]
    public int trials = 100; // 시뮬레이션 횟수

    [Header("UI 연결")]
    public TextMeshProUGUI resultText; // 결과를 출력할 TMP 텍스트

    // 버튼에 연결할 함수
    public void Simulate()
    {
        if (resultText == null)
        {
            Debug.LogError("결과를 표시할 Text UI가 연결되지 않았습니다.");
            return;
        }

        // 1. 데이터 초기화 (버튼 누를 때마다 새로 계산)
        int[] counts = new int[6];
        string report = $"<size=120%>시뮬레이션 결과 ({trials}회)</size>\n\n";

        // 2. 지정된 횟수만큼 주사위 던지기
        for (int i = 0; i < trials; i++)
        {
            int result = Random.Range(1, 7);
            counts[result - 1]++;
        }

        // 3. 결과 문자열 생성 및 UI 출력
        for (int i = 0; i < counts.Length; i++)
        {
            float percent = (float)counts[i] / trials * 100f;

            // UI에 표시될 텍스트 누적
            report += $"{i + 1}번 눈: {counts[i]}회 ({percent:F1}%)\n";
        }

        // 4. 최종적으로 UI 텍스트 컴포넌트에 할당
        resultText.text = report;

        Debug.Log($"{trials}회 시뮬레이션 완료");
    }
}