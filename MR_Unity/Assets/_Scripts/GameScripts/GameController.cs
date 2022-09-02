using UnityEngine;

namespace WiapMR.GameScripts
{
    public class GameController : MonoBehaviour
    {
        public static bool GAME_SPAWNED = false;

        private void Start()
        {
            GAME_SPAWNED = true;
        }
    }
}