using UnityEngine;

[CreateAssetMenu(fileName = "Spin Config")]
public class SpinConfig : ScriptableObject
{
    public float PauseBetweenColSpins = 0.4f;
    public int FullSpinCycles = 15;
    public float Duration = 2f;
}
