using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;


public class GameBoard : MonoBehaviourPun, IMixedRealityInputHandler
{
    private bool isGrabbing = false;
    void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<ObjectManipulator>().enabled = true;
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if(!photonView.IsMine)
        {
             photonView.RequestOwnership();
        }
    }

    public void OnInputUp(InputEventData eventData)
    {

    }
}
