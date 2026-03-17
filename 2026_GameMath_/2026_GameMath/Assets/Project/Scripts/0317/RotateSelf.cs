using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    public float rotateSpeed = 10f;

    void Update()
    {
        // 제자리에서 Y축을 기준으로 회전
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
    }
}