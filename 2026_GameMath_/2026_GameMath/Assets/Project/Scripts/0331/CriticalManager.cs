using UnityEngine;
using TMPro; // UI 출력을 위해 필수

public class CriticalManager : MonoBehaviour
{
    [Header("통계 데이터")]
    public int totalHits = 0;
    public int critHits = 0;
    public float targetRate = 0.1f; // 10% 목표 확률

    [Header("UI 연결")]
    public TextMeshProUGUI statusText; // 전체 통계용 텍스트
    public TextMeshProUGUI lastHitText; // 방금 친 공격의 결과 (Critical! / Normal)

    // 버튼에 연결할 함수
    public void SimulateCritical()
    {
        bool isCrit = RollCrit();

        // 1. 방금 발생한 히트 결과 연출
        if (isCrit)
        {
            lastHitText.text = "<color=red><size=120%>CRITICAL HIT!</size></color>";
        }
        else
        {
            lastHitText.text = "<color=white>Normal Hit</color>";
        }

        // 2. 전체 통계 텍스트 업데이트
        float currentRate = totalHits > 0 ? (float)critHits / totalHits * 100f : 0f;

        statusText.text = $"전체 공격 횟수: {totalHits}회\n" +
                          $"치명타 발생 횟수: {critHits}회\n" +
                          $"현재 치명타 확률: <color=yellow>{currentRate:F2}%</color>\n" +
                          $"(목표 확률: {targetRate * 100f}%)";

        // 콘솔 로그 병행
        Debug.Log($"Hit #{totalHits}: {(isCrit ? "Crit" : "Normal")} | Rate: {currentRate:F2}%");
    }

    public bool RollCrit()
    {
        totalHits++;
        float currentRate = 0f;
        if (critHits > 0)
        {
            currentRate = (float)critHits / totalHits;
        }

        // 강제 발생 로직 (확률이 너무 낮을 때)
        if (currentRate < targetRate && (float)(critHits + 1) / totalHits <= targetRate)
        {
            critHits++;
            return true;
        }

        // 강제 억제 로직 (확률이 너무 높을 때)
        if (currentRate > targetRate && (float)critHits / totalHits >= targetRate)
        {
            return false;
        }

        // 일반 확률 로직
        if (Random.value < targetRate)
        {
            critHits++;
            return true;
        }

        return false;
    }
}