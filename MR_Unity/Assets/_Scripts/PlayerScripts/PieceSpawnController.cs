using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.UI;
using WiapMR.GameScripts;

namespace WiapMR.PlayerScripts
{
    /// <summary>
    /// This class is responsible for creating the gamepiece spawner buttons.
    /// </summary>
    public class PieceSpawnController : MonoBehaviour
    {
        // according to https://docs.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/features/ux-building-blocks/scrolling-object-collection?view=mrtkunity-2022-05
        public GameObject ButtonPrefab;
        public GameObject ScrollListPrefab;
        private readonly List<string> Pieces = new List<string>();
        private string[] _piecePaths;
        private GameObject _board;
        private GameObject _scrollList;

        public void CreatePieceList(GameObject board, GameData.GamePiece[] pieces)
        {
            GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
            _board = board;
            _scrollList = Instantiate(ScrollListPrefab, ScrollListPrefab.transform.position, _board.transform.rotation);
            _scrollList.transform.parent = _board.transform;
            _scrollList.transform.localScale = new Vector3(100, 100, 100);
            _scrollList.transform.Translate(-30, 0, 200);
            _scrollList.transform.Rotate(_scrollList.transform.right, 90);
            var buttonParent = _scrollList.transform.GetChild(0).GetChild(0);
            for (int i = 0; i < pieces.Length; i++)
            {
                this.Pieces.Add(pieces[i].Name);
                var newButton = Instantiate(ButtonPrefab);
                newButton.transform.parent = buttonParent;
                newButton.transform.Rotate(newButton.transform.right, 90);
                newButton.transform.localScale = new Vector3(7.5f, 7.5f, 7.5f);
                newButton.GetComponentInChildren<TMPro.TextMeshPro>().text = pieces[i].Name;//+ ">" + i;
                var buttonScript = newButton.GetComponent<ButtonConfigHelper>();
                int funcID = i;
                // Debug.Log("Iteration of i: " + funcID);
                buttonScript.OnClick.AddListener(() => { gi.SpawnGamePiece(funcID); });
            }
            _piecePaths = new string[this.Pieces.Count];
            for (int i = 0; i < this.Pieces.Count; i++)
            {
                _piecePaths[i] = pieces[i].Path;
            }
            buttonParent.GetComponent<GridObjectCollection>().UpdateCollection();
        }
    }
}