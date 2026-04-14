using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class TurnBasedSimulator : MonoBehaviour
{
    [Header("설정 수치")]
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    [Header("UI 연결")]
    public TextMeshProUGUI combatResultText;   // 전투 결과 텍스트
    public TextMeshProUGUI itemResultText;     // 획득 아이템 텍스트

    // 데이터 기록용
    int turn = 0;
    int totalEnemies = 0;
    int killedEnemies = 0;
    int totalHitsAttempted = 0;
    int totalHitsSucceeded = 0;
    int totalCrits = 0;
    float maxDamage = 0f;
    float minDamage = float.MaxValue;

    // 아이템 카운트
    Dictionary<string, int> inventory = new Dictionary<string, int>()
    {
        {"Potion", 0}, {"Gold", 0}, {"Weapon-Normal", 0}, {"Weapon-Rare", 0}, {"Armor-Normal", 0}, {"Armor-Rare", 0}
    };

    // 확률 보정 변수
    float currentRareBonus = 0f;

    public void StartSimulation()
    {
        ResetData();
        bool rareObtained = false;

        while (!rareObtained)
        {
            turn++;
            rareObtained = SimulateTurn();
            // 턴마다 레어 아이템 획득 확률 5%씩 상승
            currentRareBonus += 0.05f;
        }

        UpdateUI();
    }

    private void ResetData()
    {
        turn = 0; totalEnemies = 0; killedEnemies = 0;
        totalHitsAttempted = 0; totalHitsSucceeded = 0; totalCrits = 0;
        maxDamage = 0f; minDamage = float.MaxValue;
        currentRareBonus = 0f;

        var keys = new List<string>(inventory.Keys);
        foreach (var key in keys) inventory[key] = 0;
    }

    bool SimulateTurn()
    {
        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemies += enemyCount;

        for (int i = 0; i < enemyCount; i++)
        {
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalHitsAttempted += maxHitsPerTurn;
            totalHitsSucceeded += hits;

            float turnTotalDamage = 0f;
            for (int j = 0; j < hits; j++)
            {
                float dmg = SampleNormal(meanDamage, stdDevDamage);

                bool isCrit = Random.value < critChance;
                if (isCrit) { dmg *= critDamageRate; totalCrits++; }

                if (dmg > maxDamage) maxDamage = dmg;
                if (dmg < minDamage) minDamage = dmg;

                turnTotalDamage += dmg;
            }

            if (turnTotalDamage >= enemyHP)
            {
                killedEnemies++;
                if (ProcessDrop()) return true; // 레어 아이템 획득 시 즉시 종료
            }
        }
        return false;
    }

    bool ProcessDrop()
    {
        string[] pool = { "Gold", "Weapon", "Armor", "Potion" };
        string reward = pool[Random.Range(0, pool.Length)];

        // 보정된 확률 계산 (기본 20% + 턴당 5%)
        float rareChance = 0.2f + currentRareBonus;

        if (reward == "Weapon")
        {
            if (Random.value < rareChance) { inventory["Weapon-Rare"]++; return true; }
            else inventory["Weapon-Normal"]++;
        }
        else if (reward == "Armor")
        {
            if (Random.value < rareChance) { inventory["Armor-Rare"]++; return true; }
            else inventory["Armor-Normal"]++;
        }
        else { inventory[reward]++; }

        return false;
    }

    void UpdateUI()
    {
        // 전투 결과 정렬
        combatResultText.text = $"<color=yellow><size=120%>전투 결과</size></color>\n\n" +
            $"총 진행 턴 수 : {turn}\n" +
            $"발생한 적 : {totalEnemies}\n" +
            $"처치한 적 : {killedEnemies}\n" +
            $"공격 명중 결과 : {(totalHitsAttempted > 0 ? (float)totalHitsSucceeded / totalHitsAttempted * 100 : 0):F2}%\n" +
            $"발생한 치명타율 결과 : {(totalHitsSucceeded > 0 ? (float)totalCrits / totalHitsSucceeded * 100 : 0):F2}%\n" +
            $"최대 데미지 : {maxDamage:F2}\n" +
            $"최소 데미지 : {(minDamage == float.MaxValue ? 0 : minDamage):F2}";

        // 획득 아이템 정렬
        itemResultText.text = $"<color=orange><size=120%>획득한 아이템</size></color>\n\n" +
            $"포션 : {inventory["Potion"]}개\n" +
            $"골드 : {inventory["Gold"]}개\n" +
            $"무기 - 일반 : {inventory["Weapon-Normal"]}개\n" +
            $"무기 - <color=green>레어</color> : {inventory["Weapon-Rare"]}개\n" +
            $"방어구 - 일반 : {inventory["Armor-Normal"]}개\n" +
            $"방어구 - <color=green>레어</color> : {inventory["Armor-Rare"]}개";
    }

    int SamplePoisson(float lambda)
    {
        int k = 0; float p = 1f; float L = Mathf.Exp(-lambda);
        while (p > L) { k++; p *= (1.0f - Random.value); }
        return k - 1;
    }

    int SampleBinomial(int n, float p)
    {
        int s = 0; for (int i = 0; i < n; i++) if (Random.value < p) s++;
        return s;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value; float u2 = 1.0f - Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }
}