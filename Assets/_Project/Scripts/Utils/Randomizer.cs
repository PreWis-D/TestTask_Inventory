using UnityEngine;

public class Randomizer
{
    public int GetRandomInteger(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
}