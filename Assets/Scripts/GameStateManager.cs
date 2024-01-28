using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates { WaitingPlayerTurn, PlayerTurn, EnemysTurn, Won, Lose}

public static class GameStateManager
{
    public static GameStates state;
}
