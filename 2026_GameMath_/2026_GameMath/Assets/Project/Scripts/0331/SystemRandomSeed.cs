using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemRandomSeed : MonoBehaviour
{
    void Start()
    {
        // System.Random rnd = new System.Random(1234); // 항상 같은 순서로 출력됨
        System.Random rnd = new System.Random(1234);

        for (int i = 0; i < 5; i++)
        {
            Debug.Log(rnd.Next(1, 7)); // 1~6 사이의 정수
        }
    }
}