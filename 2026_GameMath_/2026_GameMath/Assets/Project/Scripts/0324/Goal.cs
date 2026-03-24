using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour
{
    [Header("UI 설정")]
    public GameObject winUI;

    private void OnTriggerEnter(Collider other)
    {
        // 플레이어 태그 확인
        if (other.CompareTag("Player"))
        {
            Success();
        }
    }

    void Success()
    {
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        // 게임 정지
        Time.timeScale = 0f;

        Debug.Log("<color=yellow>축하합니다! 목적지에 도착했습니다.</color>");
    }
}