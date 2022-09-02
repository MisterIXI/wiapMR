using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiapMR.PUN;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Photon.Pun;
using WiapMR.GameScripts;

namespace WiapMR.PlayerScripts
{
    public class GamePieceLoader : MonoBehaviour
    {
        [PunRPC]
        public void LoadGamePiece(int pieceID)
        {
            GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
            GameData.GamePiece pieceData = gi.GameData.GamePieces[pieceID];
            string[] meshData = gi.GamePieceData[pieceData.Path];
            transform.parent = gi.GameBoard.transform;

            ObjectLoader loader = gameObject.AddComponent<ObjectLoader>();
            loader.Load(meshData);
            var spawner = GameObject.FindObjectOfType<PieceSpawnController>().transform;
            transform.position = spawner.position + (spawner.up * 1.5f);

            Material currentMat = gameObject.GetComponent<MeshRenderer>().material;
            currentMat.color = ImporterHelper.ConvertColor(pieceData.Color);
            currentMat.SetFloat("_Metallic", pieceData.Metallic);
            currentMat.SetFloat("_Glossiness", pieceData.Smoothness);
            Destroy(gameObject.GetComponent<BoxCollider>());
            gameObject.AddComponent<BoxCollider>();
            if (currentMat.color.a != 1)
            {
                currentMat.SetFloat("_Mode", 3);
            }
            // ImporterHelper.ScaleUp(gameObject, new Vector3(0.1f, 0.1f, 0.1f));
            var startPosTransform = GameObject.FindObjectOfType<ScrollingObjectCollection>().GetComponentInChildren<ClippingBox>().transform;
            var startPos = startPosTransform.position - startPosTransform.forward * 0.5f;
            transform.position = startPos;
            gameObject.name = pieceData.Name;
            // without this line the shader will only show the correct color until something changes
            // with it, it seems to reload the variables and renders correctly
            currentMat.shader = Shader.Find("Standard");
            // transform.localScale = transform.localScale * 1f;
            // Debug.Log("piece Bounds before: " + gameObject.GetComponent<MeshRenderer>().bounds.ToString());
            var boxSize = gameObject.GetComponent<BoxCollider>().size;
            var scaleFactor = 5f / Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
            // Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
            // transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            // Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
            // StartCoroutine("testDebug");
            StartCoroutine("delayedScaling");
            // Debug.Log("piece Bounds after: " + gameObject.GetComponent<MeshRenderer>().bounds.ToString());
        }
        IEnumerator delayedScaling()
        {
            yield return new WaitForSeconds(0.1f);
            var boxSize = gameObject.GetComponent<BoxCollider>().size;
            var scaleFactor = 11f / Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
            Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
            var myMesh = GetComponent<MeshFilter>().mesh;
            var baseVertices = myMesh.vertices;
            var vertices = new Vector3[baseVertices.Length];
            for (int i = 0; i < baseVertices.Length; i++)
            {
                vertices[i] = baseVertices[i] * scaleFactor;
            }
            myMesh.vertices = vertices;
            myMesh.RecalculateBounds();

            var colliders = GetComponents<BoxCollider>();
            foreach (var coll in colliders)
            {
                Destroy(coll);
            }
            gameObject.AddComponent<BoxCollider>();
            transform.localScale = new Vector3(2, 2, 2);
            // transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        }
        IEnumerator testDebug()
        {
            bool run = false;
            while (run)
            {
                yield return new WaitForSeconds(0.5f);
                var boxSize = gameObject.GetComponent<BoxCollider>().size;
                var maxSize = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
                if (maxSize <= 5f)
                {
                    run = false;
                }
                else
                {
                    var scaleFactor = 5f / Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
                    Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
                    // transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
                }
            }
        }
    }
}