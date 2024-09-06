using System;
using System.Collections.Generic;
using UnityEngine;

public class Line : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public GameObject rewardIndicator;//placeholder
        public int[] YIDs;
    }
    [SerializeField] private Data data;
    private Grid grid;
    private ComboGroupsCollector combosCollector;

    public void Initialize(Grid grid)
    {
        this.grid = grid;
        combosCollector = new(GetSymbols());
    }

    public List<ComboGroup> GetComboGroups()
    {
        return combosCollector.GetCombos();
    }

    public void SetHighlight(bool value)
    {
        data.rewardIndicator.SetActive(value);
    }

    private Symbol[] GetSymbols()
    {
        var bounds = grid.GetBounds();
        Symbol[] symbols = new Symbol[bounds.x];
        for (int i = 0; i < bounds.x; i++)
        {
            symbols[i] = GetSymbol(i);
        }

        return symbols;
    }
    public Symbol GetSymbol(int xID)
    {
        if (xID >= data.YIDs.Length || xID < 0)
        {
            Debug.LogError("Incorrect line data");
            return grid.GetSymbol(0, 0);
        }

        return grid.GetSymbol(xID, data.YIDs[xID]);
    }
}
