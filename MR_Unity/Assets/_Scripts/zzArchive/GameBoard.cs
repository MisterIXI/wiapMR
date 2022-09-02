using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

public class GameBoard : MonoBehaviourPun, IMixedRealityInputHandler
{
    void Start()
    {
        if (photonView.IsMine)
        {
            GetComponent<ObjectManipulator>().enabled = true;
        }
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (!photonView.IsMine)
        {
            photonView.RequestOwnership();
        }
    }

    public void OnInputUp(InputEventData eventData)
    {
        // implemented to satisfy interface
    }
}

