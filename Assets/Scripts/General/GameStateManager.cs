using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    MainMenu,
    InGame,
    Photo,
    Inventory
}

public static class GameStateManager
{
    public static GameState CurrentGameState { get; private set; }

    public static UnityEvent<GameState> OnStateChange;

    public static void SetGameState(GameState state)
    {
        CurrentGameState = state;
        OnStateChange?.Invoke(state);
        Debug.Log("Game state changed to: " + state);
    }
}
