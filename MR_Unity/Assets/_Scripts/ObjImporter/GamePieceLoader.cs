using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiapMR.PUN;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;
using Photon.Pun;
public class GamePieceLoader : MonoBehaviour
{
    [PunRPC]
    public void LoadGamePiece(int pieceID)
    {
        GameImporter gi = GameObject.FindObjectOfType<GameImporter>();
        GameData.GamePiece pieceData = gi.GameData.gamePieces[pieceID];
        string[] meshData = gi.GamePieceData[pieceData.path];
        transform.parent = gi.GameBoard.transform;

        ObjectLoader loader = gameObject.AddComponent<ObjectLoader>();
        loader.Load(meshData);
        var spawner = GameObject.FindObjectOfType<PieceSpawnController>().transform;
        transform.position = spawner.position + (spawner.up * 1.5f);

        Material currentMat = gameObject.GetComponent<MeshRenderer>().material;
        currentMat.color = ImporterHelper.ConvertColor(pieceData.color);
        currentMat.SetFloat("_Metallic", pieceData.metallic);
        currentMat.SetFloat("_Glossiness", pieceData.smoothness);
        Destroy(gameObject.GetComponent<BoxCollider>());
        gameObject.AddComponent<BoxCollider>();
        if (currentMat.color.a != 1)
        {
            currentMat.SetFloat("_Mode", 3);
        }
        ImporterHelper.ScaleUp(gameObject, new Vector3(0.1f, 0.1f, 0.1f));
        var startPosTransform = GameObject.FindObjectOfType<ScrollingObjectCollection>().GetComponentInChildren<ClippingBox>().transform;
        var startPos = startPosTransform.position - startPosTransform.forward * 0.5f;
        transform.position = startPos;
        gameObject.name = pieceData.name;
        // without this line the shader will only show the correct color until something changes
        // with it, it seems to reload the variables and renders correctly
        currentMat.shader = Shader.Find("Standard");
        transform.localScale = transform.localScale * 1f;
        // Debug.Log("piece Bounds before: " + gameObject.GetComponent<MeshRenderer>().bounds.ToString());
        var boxSize = gameObject.GetComponent<BoxCollider>().size;
        var scaleFactor = 3f / Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
        // Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
        // transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        // Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
        StartCoroutine("testDebug");
        // Debug.Log("piece Bounds after: " + gameObject.GetComponent<MeshRenderer>().bounds.ToString());
    }

    IEnumerator testDebug()
    {
        bool run = true;
        while (run)
        {
            yield return new WaitForSeconds(0.5f);
            var boxSize = gameObject.GetComponent<BoxCollider>().size;
            var maxSize = Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
            if (maxSize <= 3f)
            {
                run = false;
            }
            else
            {
                var scaleFactor = 3f / Mathf.Max(boxSize.x, boxSize.y, boxSize.z);
                Debug.Log("Factor: " + scaleFactor + " Box Collider size: " + gameObject.GetComponent<BoxCollider>().size);
                transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
        }
    }
}
