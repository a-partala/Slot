using UnityEngine;
using UnityEngine.Events;

public class SlotMachine : MonoBehaviour
{
    [SerializeField] private Grid.Data gridData;
    [SerializeField] private Line[] lines = new Line[3];
    [SerializeField] private SymbolsCreator symbolsCreator;
    [SerializeField] private SpinHandler rollsHandler;
    [SerializeField] private UnityEvent OnReward;

    private CombosHandler combosHandler;
    private Grid grid;

    private void Start()
    {
        grid = new(symbolsCreator, gridData);
        combosHandler = new(grid, lines);
        rollsHandler.Initialize(grid);

        combosHandler.OnReward += () => OnReward?.Invoke();
        rollsHandler.OnRollEnd.AddListener(combosHandler.CheckCombos);
        rollsHandler.OnRollStart.AddListener(combosHandler.ResetRewardIndicators);
    }

    public void StartSpin()
    {
        rollsHandler.StartSpin();
    }
}
