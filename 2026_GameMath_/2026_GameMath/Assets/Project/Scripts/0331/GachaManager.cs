using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI 출력을 위해 추가

public class GachaManager : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI gachaResultText; // 결과창 TMP 연결

    // 단일 뽑기 버튼에 연결
    public void SimulateGachaSingle()
    {
        string finalResult = Simulate();
        gachaResultText.text = $"[단일 뽑기 결과]\n<color=yellow>{finalResult}</color> 등급";
        Debug.Log("Gacha Result: " + finalResult);
    }

    // 10연속 뽑기 버튼에 연결
    public void SimulateGachaTenTime()
    {
        List<string> results = new List<string>();
        for (int i = 0; i < 9; i++)
        {
            results.Add(Simulate());
        }

        // 10번째 천장 로직
        float r2 = Random.value;
        string result2 = (r2 < 2f / 3f) ? "A" : "S";
        results.Add(result2);

        // UI 출력용 문자열 구성
        string report = "<size=120%>★ 10연속 뽑기 결과 ★</size>\n\n";
        for (int i = 0; i < results.Count; i++)
        {
            // S등급은 빨간색, A등급은 주황색으로 강조 (선택 사항)
            string color = results[i] == "S" ? "red" : (results[i] == "A" ? "orange" : "white");
            report += $"{i + 1}회: <color={color}>{results[i]}</color>  ";

            if ((i + 1) % 5 == 0) report += "\n"; // 5개마다 줄바꿈
        }

        gachaResultText.text = report;
        Debug.Log("Gacha Results: " + string.Join(", ", results));
    }

    string Simulate()
    {
        float r = Random.value;
        if (r < 0.4f) return "C";
        if (r < 0.7f) return "B";
        if (r < 0.9f) return "A";
        return "S";
    }
}