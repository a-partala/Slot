using System.Collections.Generic;

public class ComboGroupsCollector
{
    private Symbol[] lineSymbols;
    private int minComboCount;

    public ComboGroupsCollector(Symbol[] lineSymbols, int minComboCount = 3)
    {
        this.lineSymbols = lineSymbols;
        this.minComboCount = minComboCount;
    }

    /// <summary>
    /// Allows to collect all combo groups on any-length lines
    /// </summary>
    /// <returns></returns>
    public List<ComboGroup> GetCombos()
    {
        List<ComboGroup> combos = new List<ComboGroup>();

        for (int i = 0; i < lineSymbols.Length; i++)
        {
            Symbol symbol = lineSymbols[i];
            bool canCreateNew = ProcessExistCombos(combos, lineSymbols[i]);

            if (canCreateNew)
            {
                //Too late to create brand new groups
                if(i >= lineSymbols.Length - 2)
                {
                    continue;
                }
                combos.Add(new ComboGroup(symbol));
            }
        }
        RemoveIncompleteWildGroup(combos);

        return combos;
    }

    private void RemoveIncompleteWildGroup(List<ComboGroup> combos)
    {
        foreach (ComboGroup group in combos)
        {
            if(group.MainType == Symbol.Type.Wild && group.Count < lineSymbols.Length)
            {
                combos.Remove(group);
                return;
            }
        }
    }

    /// <summary>
    /// Processes all combo groups and tries to add symbols to them
    /// </summary>
    /// <returns>True if there is space for new group</returns>
    private bool ProcessExistCombos(List<ComboGroup> combos, Symbol symbol)
    {
        bool isFullWildGroupThere = false;
        bool didSymbolAdd = false;
        bool coloredGroupExtended = false;

        for (int j = 0; j < combos.Count; j++)
        {
            var combo = combos[j];
            //Skip if current group is closed (valuable + isolated)
            if (combo.Closed)
            {
                continue;
            }

            //Try to update group by internal algorythm
            if (combo.TryToAddID(symbol, out bool wildGroupGotColor))
            {
                if(!wildGroupGotColor)
                {
                    coloredGroupExtended = true;
                }
                didSymbolAdd = true;
            }
            //Close group if it is isolated and at least minimal
            else if (combo.Count >= minComboCount)
            {
                combo.Close();
            }

            //Mark that there is open full  wild group
            if (combo.MainType == Symbol.Type.Wild)
            {
                isFullWildGroupThere = true;
            }
            //Delete small simple-colored group or wild group that duplicates area of simple group
            else if ((combo.Count < minComboCount && !didSymbolAdd) || 
                (wildGroupGotColor && coloredGroupExtended))
            {
                combos.RemoveAt(j);
                j--;
            }
        }

        //There is no full wild groups or symbol is totally free
        return (!isFullWildGroupThere && symbol.MyType == Symbol.Type.Wild) || !didSymbolAdd;
    }
}
