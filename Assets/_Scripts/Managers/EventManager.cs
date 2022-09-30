using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    #region Delegates
    public delegate void onDirectionSent(InputManager.DirectionType direction);
    public delegate void onLevelEnded(GameManager.GameState gameState);
    public delegate void onUIButtonClicked(bool isNext);
    public delegate void onStacksMerged(int value, Vector3 pos);
    #endregion

    #region Events
    public event onDirectionSent OnDirectionSent;
    public event onLevelEnded OnLevelEnded;
    public event onUIButtonClicked OnUIButtonClicked;
    public event onStacksMerged OnStacksMerged;

    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void DirectionSent(InputManager.DirectionType direction)
    {
        OnDirectionSent(direction);
    }

    public void LevelEnded(GameManager.GameState gameState)
    {
        OnLevelEnded(gameState);
    }

    public void UIButtonClicked(bool isNext)
    {
        OnUIButtonClicked(isNext);
    }

    public void StacksMerged(int val, Vector3 pos)
    {
        OnStacksMerged(val, pos);
    }

}
