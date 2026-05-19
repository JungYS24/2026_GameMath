using UnityEngine;

public class BallCollisionDetector : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.Instance.RecordCollision(this.gameObject, collision.gameObject);
    }
}