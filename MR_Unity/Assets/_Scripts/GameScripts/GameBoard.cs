using Photon.Pun;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Input;

namespace WiapMR.GameScripts
{
    public class GameBoard : MonoBehaviourPun, IMixedRealityInputHandler
    {
        private bool _isGrabbing = false;
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
}
