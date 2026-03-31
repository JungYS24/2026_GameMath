using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TotalCombatManager : MonoBehaviour
{
    [Header("전투 설정")]
    public float playerAtk = 30f;
    public float enemyMaxHp = 300f;
    private float currentEnemyHp;

    [Header("치명타 보정 설정")]
    public float critTargetRate = 0.3f;
    private int totalHits = 0;
    private int critHits = 0;

    [Header("아이템 드랍 확률 설정")]
    // 0:일반(50%), 1:고급(30%), 2:희귀(15%), 3:전설(5%) 초기값
    private float[] baseDropRates = { 0.5f, 0.3f, 0.15f, 0.05f };
    private float[] currentDropRates = { 0.5f, 0.3f, 0.15f, 0.05f };
    private int[] dropCount = new int[4];

    [Header("아이템 이미지 리소스")]
    public Sprite[] itemSprites;

    [Header("UI 연결")]
    public TextMeshProUGUI combatStatusText; 
    public TextMeshProUGUI enemyHpText;      
    public TextMeshProUGUI dropRateText;     
    public TextMeshProUGUI inventoryText;    
    public Image dropDisplayImage;           

    void Start()
    {
        ResetEnemy();
        if (dropDisplayImage != null) dropDisplayImage.gameObject.SetActive(false);
        UpdateUI();
    }

    // [공격 버튼 On Click 이벤트에 연결]
    // [공격 버튼 On Click 이벤트에 연결]
    public void OnAttackButton()
    {
        if (dropDisplayImage != null) dropDisplayImage.gameObject.SetActive(false);

        if (currentEnemyHp <= 0) return;

        bool isCrit = RollCritWithCorrection();
        float damage = isCrit ? playerAtk * 2 : playerAtk;

        currentEnemyHp -= damage;
        if (currentEnemyHp < 0) currentEnemyHp = 0;

        if (currentEnemyHp <= 0)
        {
            ProcessDrop();   // 1. 아이템을 먼저 주고
            ResetEnemy();    // 2. 적의 체력을 다시 풀로 채웁니다 (리셋)
        }

        UpdateUI();
    }

    bool RollCritWithCorrection()
    {
        totalHits++;
        // 첫 공격이 아닐 때만 현재 비율 계산
        float currentRate = totalHits > 1 ? (float)critHits / (totalHits - 1) : 0f;

        // 강제 발생 조건: 현재 비율이 목표보다 낮고, 발생시켜도 목표치를 넘지 않을 때
        if (currentRate < critTargetRate && (float)(critHits + 1) / totalHits <= critTargetRate)
        {
            critHits++;
            return true;
        }
        // 강제 억제 조건: 현재 비율이 목표보다 높을 때
        if (currentRate > critTargetRate && (float)critHits / totalHits >= critTargetRate)
        {
            return false;
        }
        // 기본 확률 판정
        if (Random.value < critTargetRate)
        {
            critHits++;
            return true;
        }
        return false;
    }

    // 전리품 획득 및 전설 천장 보정 로직
    void ProcessDrop()
    {
        float r = Random.value;
        float cumulative = 0f;
        int selectedIndex = -1;

        // 누적 확률 기반 등급 결정 (전설부터 체크)
        for (int i = 3; i >= 0; i--)
        {
            cumulative += currentDropRates[i];
            if (r <= cumulative)
            {
                selectedIndex = i;
                break;
            }
        }

        if (selectedIndex != -1)
        {
            dropCount[selectedIndex]++;

            // UI 이미지 업데이트
            if (dropDisplayImage != null && itemSprites.Length > selectedIndex)
            {
                dropDisplayImage.gameObject.SetActive(true);
                dropDisplayImage.sprite = itemSprites[selectedIndex];
            }

            // 전설 아이템(인덱스 3) 처리
            if (selectedIndex == 3)
            {
                // 획득 시 확률 초기화
                System.Array.Copy(baseDropRates, currentDropRates, 4);
                Debug.Log("<color=orange>전설 아이템 획득! 확률이 초기화되었습니다.</color>");
            }
            else
            {
                // 미획득 시 보정: 전설 +1.5%, 나머지 각 -0.5%
                currentDropRates[3] += 0.015f;
                for (int i = 0; i < 3; i++)
                {
                    currentDropRates[i] -= 0.005f;
                    if (currentDropRates[i] < 0) currentDropRates[i] = 0;
                }
            }
        }
    }

    // 적 부활 (체력 리셋) 버튼 등으로 활용 가능
    public void ResetEnemy()
    {
        currentEnemyHp = enemyMaxHp;
        UpdateUI();
    }

    void UpdateUI()
    {
        // 전투 통계
        combatStatusText.text = $"전체 공격 횟수 : {totalHits}\n" +
                               $"발생한 치명타 횟수 : {critHits}\n" +
                               $"설정된 치명타 확률 : {critTargetRate * 100:F2}%\n" +
                               $"실제 치명타 확률 : {(totalHits > 0 ? (float)critHits / totalHits * 100 : 0):F2}%";

        //  적 체력
        enemyHpText.text = $"체력 : {currentEnemyHp:F0} / {enemyMaxHp}";

        //  드랍 확률 (실시간 변동 표시)
        dropRateText.text = $"현재 아이템 확률\n" +
                            $"일반 : {currentDropRates[0] * 100:F1}%\n" +
                            $"고급 : {currentDropRates[1] * 100:F1}%\n" +
                            $"희귀 : {currentDropRates[2] * 100:F1}%\n" +
                            $"전설 : <color=orange>{currentDropRates[3] * 100:F1}%</color>";

        // 인벤토리
        inventoryText.text = $"현재 드랍된 아이템\n" +
                             $"일반 : {dropCount[0]}\n" +
                             $"고급 : {dropCount[1]}\n" +
                             $"희귀 : {dropCount[2]}\n" +
                             $"전설 : {dropCount[3]}";
    }
}