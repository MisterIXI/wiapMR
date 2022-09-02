using UnityEngine;

namespace WiapMR.GameScripts
{
    public class GameController : MonoBehaviour
    {
        private static bool _gameSpawned = false;

        private void Start()
        {
            _gameSpawned = true;
        }
    }
}