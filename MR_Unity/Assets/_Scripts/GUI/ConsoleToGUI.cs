using UnityEngine;
using TMPro;

namespace WiapMR.GUI
{
    // taken from http://answers.unity.com/answers/927240/view.html
    /// <summary>
    /// This class with modified code taken from unityanswers puts the unity console into the game. Useful for debugging and similar.
    /// </summary>
    public class ConsoleToGUI : MonoBehaviour
    {
        public TextMeshPro TextMesh;
        string _myLog = "*begin log";
        string _filename = "";
        bool _doShow = true;
        int _kChars = 700;
        void OnEnable() { Application.logMessageReceived += Log; }
        void OnDisable() { Application.logMessageReceived -= Log; }
        void Update() { if (Input.GetKeyDown(KeyCode.Space)) { _doShow = !_doShow; } }
        public void Log(string logString, string stackTrace, LogType type)
        {
            // for onscreen...
            _myLog = _myLog + "\n" + logString;
            if (_myLog.Length > _kChars) { _myLog = _myLog.Substring(_myLog.Length - _kChars); }

            // for the file ...
            if (_filename == "")
            {
                string d = System.Environment.GetFolderPath(
                   System.Environment.SpecialFolder.Desktop) + "/YOUR_LOGS";
                System.IO.Directory.CreateDirectory(d);
                string r = Random.Range(1000, 9999).ToString();
                _filename = d + "/log-" + r + ".txt";
            }
            try { System.IO.File.AppendAllText(_filename, logString + "\n"); }
            catch { }
        }

        void OnGUI()
        {
            if (!_doShow) { return; }
            // GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity,
            //    new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
            // GUI.TextArea(new Rect(10, 10, 540, 370), myLog);
            if (TextMesh != null)
            {
                TextMesh.text = _myLog;
            }
        }
    }
}