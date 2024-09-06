using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpinHandler : MonoBehaviour
{
    [SerializeField] private SpinConfig config;
    [SerializeField] private List<AudioSource> sources;
    public UnityEvent OnRollStart;
    public UnityEvent OnRollEnd;
    private Grid grid;
    Coroutine[] spinRoutines;
    private bool isInitialized = false;
    private bool IsSpinning = false;

    public void Initialize(Grid grid)
    {
        if(grid == null)
        {
            Debug.LogError("Grid in SpinHandler is null");
            return;
        }
        this.grid = grid;
        var fullMatrix = grid.GetExtendedMatrix();
        spinRoutines = new Coroutine[fullMatrix.GetLength(0)];

        isInitialized = true;
    }

    /// <summary>
    /// Starts spin coroutine
    /// </summary>
    public Coroutine StartSpin(Action callback = null)
    {
        if(IsSpinning)
        {
            Debug.LogWarning("Slot is already spinning");
            return null;
        }
        if (!isInitialized)
        {
            Debug.LogError("SpinHandler is not initialized");
            return null;
        }
        IsSpinning = true;
        OnRollStart?.Invoke();

        return StartCoroutine(SpinRoutine(() =>
        {
            IsSpinning = false;
            callback?.Invoke();
            OnRollEnd?.Invoke();
        }));
    }

    /// <summary>
    /// Spins all the reels
    /// </summary>
    private IEnumerator SpinRoutine(Action callback = null)
    {
        var fullMatrix = grid.GetExtendedMatrix();

        for (int i = 0; i < fullMatrix.GetLength(0); i++)
        {
            Symbol[] column = new Symbol[fullMatrix.GetLength(1)];
            for (int j = 0; j < fullMatrix.GetLength(1); j++)
            {
                column[j] = fullMatrix[i, j];
            }
            sources[i].Play();
            spinRoutines[i] = StartCoroutine(SpinColumnRoutine(column, sources[i].Stop));

            yield return new WaitForSeconds(config.PauseBetweenColSpins);
        }

        for (int i = 0; i < spinRoutines.Length; i++)
        {
            if (spinRoutines[i] == null)
            {
                continue;
            }

            yield return spinRoutines[i];
        }

        callback?.Invoke();
    }

    /// <summary>
    /// Spins one reel including extra symbol
    /// </summary>
    /// <param name="column">Symbols of the reel including one extra</param>
    private IEnumerator SpinColumnRoutine(Symbol[] column, Action callback = null)
    {
        Vector2[] anchorPositions = new Vector2[column.Length];

        for (int i = 0; i < column.Length; i++)
        {
            anchorPositions[i] = column[i].transform.localPosition;
        }
        float reelLength = grid.GetReelLength();
        Vector2 shiftValue;
        Vector2 targetShift = -Vector2.up * reelLength * config.FullSpinCycles;

        float expiredTime = 0;
        float duration = config.Duration;
        float t = 0;

        SymbolsRandomizer typesRandomizer = new();
        while (t < 1f)
        {
            expiredTime += Time.deltaTime;
            t = expiredTime / duration;
            shiftValue = Vector2.Lerp(Vector2.zero, targetShift, t);

            for (int i = 0; i < column.Length; i++)
            {
                Vector2 totalPos = anchorPositions[i] + shiftValue;

                float bottomOverflow = grid.GetPos(0, -1).y - totalPos.y;
                if (bottomOverflow > 0)
                {
                    anchorPositions[i].y = grid.GetPos(0, column.Length - 1).y - shiftValue.y - bottomOverflow;
                    totalPos = anchorPositions[i] + shiftValue;
                    column[i].SetType(typesRandomizer.GetRandom());
                }
                column[i].transform.localPosition = totalPos;
            }

            yield return null;
        }
        for (int i = 0; i < column.Length; i++)
        {
            column[i].transform.localPosition = new Vector2(anchorPositions[i].x, grid.GetPos(0, i).y);
        }
        callback?.Invoke();
    }
}
