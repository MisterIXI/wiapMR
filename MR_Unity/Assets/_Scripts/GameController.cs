using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static bool GAME_SPAWNED = false;

    private void Start() {
        GAME_SPAWNED = true;
    }
    
}
