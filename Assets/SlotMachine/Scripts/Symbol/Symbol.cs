using System;
using TMPro;
using UnityEngine;

public class Symbol : MonoBehaviour
{
    //values must be sequential and start from 0 for proper randomization
    public enum Type
    {
        Wild = 0,
        I = 1,
        II = 2,
        III = 3,
        IV = 4,
        V = 5,
        VI = 6,
        VII = 7,
        VIII = 8,
        Bonus = 9,
    }

    [Serializable]
    public class Data
    {
        public Sprite Sprite = null;
        public float Multiplier = 1;
    }

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject glow;
    [SerializeField] private SymbolsConfig config;
    [SerializeField] private TextMeshPro comboLevelTMP;
    private int comboLevel = 0;

    public Type MyType { get; private set; }

    public void SetType(Type type)
    {
        MyType = type;
        Data data;

        if(config == null)
        {
            data = default(Data);
            Debug.LogError("Symbols config is not assigned");
        }
        else
        {
            data = config.GetData(type);
        }

        spriteRenderer.sprite = data.Sprite;
    }

    public void ResetComboLevel()
    {
        comboLevel = 0;
        UpdateComboIndicators();
    }

    public void IncreaseComboLevel()
    {
        comboLevel++;
        UpdateComboIndicators();
    }

    private void UpdateComboIndicators()
    {
        glow.SetActive(comboLevel > 0);
        comboLevelTMP.text = comboLevel > 1 ? comboLevel.ToString() : "";
    }
}
