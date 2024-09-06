using System.Collections.Generic;
using UnityEngine;
using static Symbol;

[CreateAssetMenu(fileName = "Symbols Config")]
public class SymbolsConfig : ScriptableObject
{
    [SerializeField] private Symbol symbolPrefab;
    [SerializeField] private Data[] symbolDatas;
    // Using a basic dictionary for stability. SerializedDictionary could improve performance but may introduce bugs.
    private Dictionary<Type, Data> dataMap = new();
    private bool isMapAssigned = false;

    public Data GetData(Type type)
    {
        //Rebuild the map if new data is added to symbolDatas
        if (!isMapAssigned || dataMap.Count < symbolDatas.Length)
        {
            AssignMap();
        }

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

    private void AssignMap()
    {
        dataMap.Clear();
        for (int i = 0; i < symbolDatas.Length; i++)
        {
            Data data = symbolDatas[i];
            if (dataMap.ContainsKey(data.Type))
            {
                continue;
            }

            dataMap.Add(data.Type, data);
        }

        isMapAssigned = true;
    }
}
