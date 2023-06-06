using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    private static Player player;

    /// <summary>
    /// Set the reference to the player
    /// </summary>
    /// <param name="_player"></param>
    public static void SetPlayer(Player _player)
    {
        player = _player;
    }
    
    /// <summary>
    /// Returns the reference to the player
    /// </summary>
    /// <returns></returns>
    public static Player GetPlayer()
    {
        return player;
    }
}
