using System;
using System.Collections.Generic;

public class CombosHandler
{
    private Grid grid;
    private Line[] lines;
    public event Action OnReward;
    public event Action OnComboCheckEnd;

    public CombosHandler(Grid grid, Line[] lines)
    {
        this.lines = lines;
        this.grid = grid;
        InitLines();
    }

    public void ResetRewardIndicators()
    {
        var symbolMatrix = grid.GetExtendedMatrix();
        for (int i = 0; i < symbolMatrix.GetLength(0); i++)
        {
            for (int j = 0; j < symbolMatrix.GetLength(1); j++)
            {
                symbolMatrix[i, j].ResetComboLevel();
            }
        }

        SetLinesHighlight(lines, false);
    }

    public void CheckCombos()
    {
        List<ComboGroup> combos = GetComboGroups(out var rewardLines);

        SetLinesHighlight(rewardLines, true);

        for (int i = 0; i < combos.Count; i++)
        {
            var symbols = combos[i].GetSymbols();

            for (int j = 0; j < symbols.Count; j++)
            {
                symbols[j].IncreaseComboLevel();
            }
        }

        if(combos.Count > 0)
        {
            OnReward?.Invoke();
        }
        OnComboCheckEnd?.Invoke();
    }

    private List<ComboGroup> GetComboGroups(out List<Line> rewardLines)
    {
        List<ComboGroup> combos = new();
        rewardLines = new();
        for (int i = 0; i < lines.Length; i++)
        {
            var inlineGroups = lines[i].GetComboGroups();
            if (inlineGroups.Count > 0)
            {
                combos.AddRange(inlineGroups);
                rewardLines.Add(lines[i]);
            }
        }

        return combos;
    }

    private void SetLinesHighlight(Line[] lines, bool value)
    {
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].SetHighlight(value);
        }
    }

    private void SetLinesHighlight(List<Line> lines, bool value)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            lines[i].SetHighlight(value);
        }
    }

    /// <summary>
    /// Creates three simple lines
    /// </summary>
    private void InitLines()
    {
        for(int i = 0; i < lines.Length; i++)
        {
            lines[i].Initialize(grid);
        }
    }
}
