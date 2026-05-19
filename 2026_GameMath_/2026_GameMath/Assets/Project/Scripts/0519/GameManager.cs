using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player GameObjects")]
    public GameObject player1Ball;
    public GameObject player2Ball;
    public List<Rigidbody> allBallRigidbodys;

    [Header("UI & Camera System")]
    public TextMeshProUGUI scoreUiText;
    public CameraOrbit cameraOrbit;

    [Header("Game State")]
    public int currentTurn = 1;
    public int p1Score = 0;
    public int p2Score = 0;

    public bool canInput = true;
    private bool isTurnActive = false;
    private float turnStartDelayTimer = 0f;

    private HashSet<GameObject> hitObjectsInThisTurn = new HashSet<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        if (isTurnActive)
        {
            if (turnStartDelayTimer > 0f)
            {
                turnStartDelayTimer -= Time.deltaTime;
                return;
            }

            if (IsAllBallsStopped())
            {
                EndTurnAndEvaluate();
            }
        }
    }
    public bool IsValidBallForCurrentTurn(GameObject selectedBall)
    {
        if (currentTurn == 1 && selectedBall == player1Ball) return true;
        if (currentTurn == 2 && selectedBall == player2Ball) return true;
        return false;
    }

    public void OnBallHit()
    {
        canInput = false;  
        isTurnActive = true;  
        hitObjectsInThisTurn.Clear();
        turnStartDelayTimer = 2.0f;
    }

    public void RecordCollision(GameObject ball, GameObject opponent)
    {
        if (!isTurnActive) return;

        // 현재 턴의 주체 공이 부딪힌 대상을 기록
        if ((currentTurn == 1 && ball == player1Ball) || (currentTurn == 2 && ball == player2Ball))
        {
            hitObjectsInThisTurn.Add(opponent);
        }
    }

    private bool IsAllBallsStopped()
    {
        foreach (Rigidbody rb in allBallRigidbodys)
        {
            if (rb.linearVelocity.magnitude > 0.05f)
            {
                return false; // 하나라도 움직이면 아직 안 끝남
            }
        }
        return true;
    }

    private void EndTurnAndEvaluate()
    {
        isTurnActive = false;

        GameObject enemyBall = (currentTurn == 1) ? player2Ball : player1Ball;
        int targetHitCount = 0;
        bool hitEnemy = false;

        foreach (GameObject obj in hitObjectsInThisTurn)
        {
            if (obj == enemyBall) hitEnemy = true;
            else if (obj.CompareTag("Target")) targetHitCount++; 
        }


        if (hitEnemy)
        {
            if (currentTurn == 1) p1Score = Mathf.Max(0, p1Score - 1);
            else p2Score = Mathf.Max(0, p2Score - 1);
            Debug.Log($"{currentTurn}P: 상대 공을 맞추어 감점됩니다.");
        }
        else if (targetHitCount >= 2)
        {
            if (currentTurn == 1) p1Score++;
            else p2Score++;
            Debug.Log($"{currentTurn}P: 목적구를 모두 맞춰 1점 획득!");
        }

        if (p1Score >= 5 || p2Score >= 5)
        {
            string winner = p1Score >= 5 ? "1P" : "2P";
            Debug.Log($"게임 종료! 승리자: {winner}");
            return; // 턴 전환을 하지 않고 정지
        }

        currentTurn = (currentTurn == 1) ? 2 : 1;
        canInput = true; // 다시 입력 가능하게 해제

        if (cameraOrbit != null)
        {
            GameObject activeBall = (currentTurn == 1) ? player1Ball : player2Ball;
            if (activeBall != null)
            {
                cameraOrbit.target = activeBall.transform;
            }
        }

            Debug.Log($"현재 턴: {currentTurn}P | 점수현황 - 1P: {p1Score} / 2P: {p2Score}");

        string statusMessage = $"현재 턴: {currentTurn}P | 점수현황 - 1P: {p1Score} / 2P: {p2Score}";
        Debug.Log(statusMessage);

        if (scoreUiText != null)
        {
            scoreUiText.text = statusMessage; // UI 텍스트에 그대로 입력
        }
    }
}