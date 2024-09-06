using AYellowpaper.SerializedCollections;
using UnityEngine;
using static Symbol;

[CreateAssetMenu(fileName = "Symbols Config")]
public class SymbolsConfig : ScriptableObject
{
    [SerializeField] private Symbol symbolPrefab;
    [SerializeField] private SerializedDictionary<Type, Data> dataMap = new();

    public Data GetData(Type type)
    {
        if(!dataMap.ContainsKey(type))
        {
            return new();
        }

        return dataMap[type];
    }

    public Symbol GetSymbolPrefab()
    {
        return symbolPrefab;
    }
}
