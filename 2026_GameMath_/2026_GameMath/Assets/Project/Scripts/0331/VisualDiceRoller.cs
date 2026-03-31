using UnityEngine;
using UnityEngine.UI;

public class VisualDiceRoller : MonoBehaviour
{
    [Header("주사위 이미지 설정 (6개)")]
    public Sprite[] diceSprites;

    [Header("UI 연결")]
    public Image diceDisplayImage; // 화면에 보일 Image 컴포넌트

    void Start()
    {
        // 시작할 때 비어있지 않도록 첫 번째 주사위로 초기화
        if (diceSprites.Length >= 6 && diceDisplayImage != null)
        {
            diceDisplayImage.sprite = diceSprites[0];
        }
    }

    // 버튼에 연결할 함수
    public void RollDice()
    {
        if (diceSprites.Length < 6 || diceDisplayImage == null)
        {
            Debug.LogError("주사위 스프라이트 6개가 다 없거나 UI Image가 연결되지 않았습니다.");
            return;
        }

        // 1. 1~6 사이의 랜덤 값 생성
        int result = Random.Range(1, 7);

        // 2. 결과값에 맞는 스프라이트로 UI 변경
        // 배열 인덱스는 0부터 시작하므로 result - 1을 사용합니다.
        diceDisplayImage.sprite = diceSprites[result - 1];

        // 디버그 로그도 병행 (선택 사항)
        Debug.Log($"<color=cyan>주사위 결과: {result}</color>");
    }
}