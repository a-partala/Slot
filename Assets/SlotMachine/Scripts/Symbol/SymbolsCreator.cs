using UnityEngine;

public class SymbolsCreator : MonoBehaviour
{
    [SerializeField] private SymbolsConfig symbolsConfig;
    [SerializeField] private Transform defaultParent;

    public Symbol GetSymbolPrefab()
    {
        if (symbolsConfig == null)
        {
            Debug.LogError("Symbols config in symbols creator is not assigned");
            return null;
        }
        return symbolsConfig.GetSymbolPrefab();
    }

    public Symbol CreateSymbol(Transform overrideParent = null)
    {
        var prefab = GetSymbolPrefab();
        if (prefab == null)
        {
            Debug.LogError($"Symbol prefab in Symbols Config is not assigned");
            return null;
        }

        Symbol s = Instantiate(prefab, GetParent(overrideParent));

        return s;
    }

    public Symbol[] CreateSymbolsArray(int amount, Transform overrideParent = null)
    {
        if(amount <= 0)
        {
            Debug.LogError("Incorrect symbols array creation request");
            return new Symbol[0];
        }

        Symbol[] symbols = new Symbol[amount];
        Transform parent = GetParent(overrideParent);
        for (int i = 0; i < amount; i++)
        {
            symbols[i] = CreateSymbol(parent);
        }

        return symbols;
    }

    public Symbol[,] CreateSymbolsMatrix(int width, int height, Transform overrideParent = null)
    {
        if (width <= 0 || height <= 0)
        {
            Debug.LogError("Incorrect symbols matrix creation request");
            return new Symbol[0, 0];
        }

        Symbol[,] symbolsMatrix = new Symbol[width, height];
        Transform parent = GetParent(overrideParent);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                symbolsMatrix[i, j] = CreateSymbol(parent);
            }
        }

        return symbolsMatrix;
    }

    private Transform GetParent(Transform overrideParent = null)
    {
        if (overrideParent == null)
        {
            overrideParent = defaultParent;

            if (defaultParent == null)
            {
                Debug.LogWarning("Default parent for symbols is not assigned");
            }
        }

        return overrideParent;
    }
}
