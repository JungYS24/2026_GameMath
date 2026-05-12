using System.Collections.Generic;
using UnityEngine;

public class BezierMover : MonoBehaviour
{
    private List<Vector3> points;
    private float timeValue = 0f;
    private float duration;
    private bool isMoving = false;

    public void Init(Vector3 start, Vector3 target, float radius, float height, float speed)
    {
        duration = speed;
        Vector2 rand1 = Random.insideUnitCircle * radius;
        Vector3 p1 = start + new Vector3(rand1.x, height, rand1.y);

        Vector2 rand2 = Random.insideUnitCircle * radius;
        Vector3 p2 = target + new Vector3(rand2.x, height, rand2.y);

        points = new List<Vector3> { start, p1, p2, target };

        timeValue = 0f;
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        timeValue += Time.deltaTime / duration;
        transform.position = DeCasteljau(new List<Vector3>(points), timeValue);

        if (timeValue >= 1f)
        {
            isMoving = false;
            Destroy(gameObject);
        }
    }

    private Vector3 DeCasteljau(List<Vector3> p, float t)
    {
        while (p.Count > 1)
        {
            int last = p.Count - 1;
            List<Vector3> next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
            {
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            }
            p = next;
        }
        return p[0];
    }
}