using System.Collections.Generic;

public class ComboGroup
{
    public Symbol.Type MainType { get; private set; }
    private List<Symbol> symbols = new();
    public int Count { get => symbols.Count; }
    public bool Closed { get; private set; }

    public ComboGroup(Symbol symbol)
    {
        MainType = symbol.MyType;
        symbols.Add(symbol);
    }

    public List<Symbol> GetSymbols()
    {
        return symbols;
    }

    public bool TryToAddID(Symbol symbol, out bool wildGroupGotColor)
    {
        wildGroupGotColor = false;
        //Add symbol if it is wild or similar to others
        if (MainType == symbol.MyType || symbol.MyType == Symbol.Type.Wild)
        {
            symbols.Add(symbol);
        }
        //Change main type if it was wild and met real color
        else if (MainType == Symbol.Type.Wild)
        {
            wildGroupGotColor = true;
            MainType = symbol.MyType;
            symbols.Add(symbol);
        }
        //Nothing happened
        else
        {
            return false;
        }

        return true;
    }

    public void Close()
    {
        Closed = true;
    }
}
