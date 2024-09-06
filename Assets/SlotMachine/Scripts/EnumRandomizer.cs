using System;

public class EnumRandomizer<T> where T : Enum
{
    private static readonly int valuesAmount = Enum.GetValues(typeof(T)).Length;

    public virtual T GetRandom()
    {
        int randomIndex = UnityEngine.Random.Range(0, valuesAmount);
        return (T)Enum.ToObject(typeof(T), randomIndex);
    }
}
