using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributionTest2 : MonoBehaviour
{
    int BinomialDistribution(int trials, float chance)
    {
        int successes = 0;
        for (int i = 0; i < trials; i++)
        {
            if (Random.value < chance)
            {
                successes++;
            }
        }
        return successes;
    }

    void Start()
    {
        int result = BinomialDistribution(10, 0.3f);
        Debug.Log($"Successes out of 10 trials: {result}");
    }
}