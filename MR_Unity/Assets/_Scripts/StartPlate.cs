using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;
public class StartPlate : MonoBehaviour
{
    public TextMeshPro InfoText;
    public GameObject ChessButton;
    public GameObject GoButton;

    void Start()
    {
        InfoText.text = Application.platform.ToString();
        if (Application.platform == RuntimePlatform.WSAPlayerARM)
        {
            //Hololens code
            InfoText.text = "Wait for other platform to spawn board..." + System.Environment.NewLine + "(Spawning not supported on Hololens)";
        }
        else
        {
            transform.Translate(new Vector3(0, 0.7f, 0));
            //Desktop code
            InfoText.text = "Waiting for other player...";
        }
    }
    public void OnJoinRoom()
    {
        // Debug.Log("OnJoinRoom called");
        if (Application.platform != RuntimePlatform.WSAPlayerARM)
        {
            // Debug.Log("OnJoinRoom called on Desktop");
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                // Debug.Log("OnJoinRoom called on Desktop and more than 1 player");
                EnableButtons();
            }
        }
    }
    public void EnableButtons()
    {
        if (Application.platform == RuntimePlatform.WSAPlayerARM)
        {
            //Hololens code
            InfoText.text = "Wait for other platform to spawn board...";
        }
        else
        {
            //Desktop code
            InfoText.text = "Player connected!" + System.Environment.NewLine + "Choose game:";
            ChessButton.GetComponent<Interactable>().enabled = true;
            ChessButton.GetComponentInChildren<TextMeshPro>().text = "Spawn Chess";
            GoButton.GetComponent<Interactable>().enabled = true;
            GoButton.GetComponentInChildren<TextMeshPro>().text = "Spawn Go";
        }
    }
    // Update is called once per frame
    public void GoClick()
    {
        GameObject.FindObjectOfType<PlayerManager>().gameObject.GetPhotonView().RPC("DisableStartMenu", RpcTarget.All);
        GameObject.FindObjectOfType<GameImporter>().SpawnGo();
    }

    public void ChessClick()
    {
        GameObject.FindObjectOfType<PlayerManager>().gameObject.GetPhotonView().RPC("DisableStartMenu", RpcTarget.All);
        GameObject.FindObjectOfType<GameImporter>().SpawnChess();
    }
}
