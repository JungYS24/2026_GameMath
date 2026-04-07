using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DamageSimulator : MonoBehaviour
{
    [Header("UI 연결")]
    public TextMeshProUGUI statusDisplay;
    public TextMeshProUGUI resultDisplay;
    public TextMeshProUGUI logDisplay;
    public TextMeshProUGUI rangeDisplay;
    public Image weaponImage;

    [Header("무기 이미지 리소스")]
    // 0:단검, 1:장검, 2:도끼, 3:활
    public Sprite[] weaponSprites;

    private int level = 1;
    private float baseDamage = 20f;
    private float totalDamage = 0;
    private int attackCount = 0;

    private string weaponName;
    private float stdDevMult, critRate, critMult;
    private float maxDamageEver = 0f;

    void Start()
    {
        SetWeapon(0); // 시작 시 단검
    }

    private void ResetData()
    {
        totalDamage = 0;
        attackCount = 0;
        maxDamageEver = 0f;
    }

    // 버튼 연동용 함수 (ID 0:단검, 1:장검, 2:도끼, 3:활)
    public void SetWeapon(int id)
    {
        ResetData();

        level = 1;
        baseDamage = 20f;

        switch (id)
        {
            case 0: SetStats("단검", 0.1f, 0.4f, 1.5f); break;
            case 1: SetStats("장검", 0.2f, 0.3f, 2.0f); break;
            case 2: SetStats("도끼", 0.3f, 0.2f, 3.0f); break;
            case 3: SetStats("활", 0.05f, 0.5f, 2.5f); break; // 활: 낮은 편차, 높은 치명타
        }

        if (weaponImage != null && weaponSprites.Length > id)
        {
            weaponImage.sprite = weaponSprites[id];
        }

        logDisplay.text = $"{weaponName} 장착 완료!";
        UpdateUI();
    }

    private void SetStats(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    public void LevelUp()
    {
        ResetData();
        level++;
        baseDamage = level * 20f;
        logDisplay.text = $"레벨업! 현재 레벨: {level}";
        UpdateUI();
    }

    public void OnAttack()
    {
        var res = CalculateHit();
        attackCount++;
        totalDamage += res.damage;
        if (res.damage > maxDamageEver) maxDamageEver = res.damage;

        string hitMsg = res.isMiss ? "[Miss]" :
                        (res.isWeakPoint ? "<color=blue>[Weak!]</color> " : "");
        string critMsg = res.isCrit ? "<color=red>[Crit!]</color> " : "";

        logDisplay.text = $"{hitMsg}{critMsg}데미지: {res.damage:F1}";
        UpdateUI();
    }

    public void OnAttack1000()
    {
        int weak = 0, miss = 0, crit = 0;
        float batchSum = 0f;

        for (int i = 0; i < 1000; i++)
        {
            var res = CalculateHit();
            if (res.isMiss) miss++;
            if (res.isWeakPoint) weak++;
            if (res.isCrit) crit++;
            batchSum += res.damage;
            if (res.damage > maxDamageEver) maxDamageEver = res.damage;
        }

        attackCount += 1000;
        totalDamage += batchSum;

        logDisplay.text = $"<size=85%>1000회 결과:\n약점공격 {weak} | 명중실패 {miss} | 전체 크리티컬 {crit}\n최대 데미지: {maxDamageEver:F1}</size>";
        UpdateUI();
    }

    private (float damage, bool isCrit, bool isWeakPoint, bool isMiss) CalculateHit()
    {
        float sd = baseDamage * stdDevMult;
        float raw = GetGaussian(baseDamage, sd);

        if (raw < baseDamage - (2 * sd)) return (0, false, false, true); // 명중 실패

        bool isWeak = raw > baseDamage + (2 * sd);
        float current = isWeak ? raw * 2f : raw; // 약점 공격 2배

        bool isCrit = Random.value < critRate;
        float final = isCrit ? current * critMult : current;

        return (final, isCrit, isWeak, false);
    }

    private void UpdateUI()
    {
        statusDisplay.text = $"Lv.{level} {weaponName}\n기본 {baseDamage} | 치명 {critRate * 100}% (x{critMult})";

        float sd = baseDamage * stdDevMult;
        rangeDisplay.text = $"±3σ: [{baseDamage - 3 * sd:F1} ~ {baseDamage + 3 * sd:F1}]\n" +
                           $"약점(>2σ): {baseDamage + 2 * sd:F1} | 명중실패(<2σ): {baseDamage - 2 * sd:F1}";

        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultDisplay.text = $"누적: {totalDamage:F0} / 횟수: {attackCount}\nDPA: {dpa:F2} | Max: {maxDamageEver:F1}";
    }

    private float GetGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}