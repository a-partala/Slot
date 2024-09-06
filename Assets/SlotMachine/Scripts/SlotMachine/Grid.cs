using System;
using UnityEngine;

public class Grid
{
    [Serializable]
    public class Data
    {
        public int Width;
        public int Height;
        public Vector2 MinPos;
        public Vector2 MaxPos;
    }
    private Vector2 symbolScale;
    private Vector2 startPos;
    private Vector2 spacing;
    private Symbol[] extraSymbolsArr;
    private Symbol[,] symbolsMatrix;
    private Data data;

    public Grid(SymbolsCreator creator, Data data)
    {
        this.data = data;
        symbolScale = creator.GetSymbolPrefab().transform.localScale;
        AssignArrangement(data);
        AssignSymbols(creator, data);
    }

    public float GetReelLength()
    {
        return (GetPos(0, data.Height) - GetPos(0, -1)).y;
    }

    public Vector2Int GetBounds()
    {
        return new Vector2Int(data.Width, data.Height);
    }

    public Symbol[,] GetExtendedMatrix()
    {
        Symbol[,] exMatrix = new Symbol[data.Width, data.Height + 1];

        for (int i = 0; i < data.Width; i++)
        {
            for (int j = 0; j < data.Height; j++)
            {
                exMatrix[i, j] = symbolsMatrix[i, j];
            }
        }

        for (int i = 0; i < data.Width; i++)
        {
            exMatrix[i, data.Height] = extraSymbolsArr[i];
        }

        return exMatrix;
    }

    public Symbol GetSymbol(int i, int j)
    {
        if(i >= symbolsMatrix.GetLength(0) || 
            j >= symbolsMatrix.GetLength(1) ||
            i < 0 ||
            j < 0)
        {
            Debug.LogError("Incorrect matrix ids");
            return null;
        }

        return symbolsMatrix[i, j];
    }

    public Vector2 GetPos(int i, int j)
    {
        Vector2Int ids = new Vector2Int(i, j);
        return startPos + Vector2.Scale(symbolScale + spacing, ids);
    }

    private void AssignArrangement(Data data)
    {
        startPos = data.MinPos;
        Vector2 distance = data.MaxPos - data.MinPos;
        Vector2Int matrixSize = new Vector2Int(data.Width - 1, data.Height - 1);
        Vector2 freeSpace = distance - Vector2.Scale(symbolScale, matrixSize);
        spacing = new Vector2(freeSpace.x / matrixSize.x, freeSpace.y / matrixSize.y);
    }

    private void AssignSymbols(SymbolsCreator creator, Data data)
    {
        symbolsMatrix = creator.CreateSymbolsMatrix(data.Width, data.Height);
        extraSymbolsArr = creator.CreateSymbolsArray(data.Width);

        for (int i = 0; i < data.Width; i++)
        {
            for (int j = 0; j < data.Height; j++)
            {
                symbolsMatrix[i, j].transform.localPosition = GetPos(i, j);
            }
        }

        for (int i = 0; i < data.Width; i++)
        {
            extraSymbolsArr[i].transform.localPosition = GetPos(i, data.Height);
        }
    }
}
