using UnityEngine;

public class BezierShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform targetTransform;

    [Header("Settings")]
    public int shotCount = 10;
    public float scatterRadius = 3f;
    public float curveHeight = 5f;
    public float duration = 1.5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && targetTransform != null)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        for (int i = 0; i < shotCount; i++)
        {
            GameObject go = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            BezierMover mover = go.GetComponent<BezierMover>();

            if (mover != null)
            {
                mover.Init(transform.position, targetTransform.position, scatterRadius, curveHeight, duration);
            }
        }
    }
}