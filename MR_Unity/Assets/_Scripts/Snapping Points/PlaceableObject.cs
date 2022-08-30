using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;

public class PlaceableObject : MonoBehaviourPun, IMixedRealityInputHandler
{
    private GameObject board;
    private Collider ownCollider;
    private bool snapped;
    private GameObject snappedTo;
    private SnapPoint potentialSnapPoint;
    private bool _isGrabbing = false;


    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collision with SnapPoint (Enter): " + collider.gameObject.tag + " | " + _isGrabbing);
        if (collider.gameObject.tag == "SnapPoint" && _isGrabbing)
        {
            Debug.Log("Collision with SnapPoint (ACTUALLY_ENTER)");
            if (potentialSnapPoint != null)
            {
                potentialSnapPoint.UnhighlightHologram();
            }
            potentialSnapPoint = collider.gameObject.GetComponent<SnapPoint>();
            potentialSnapPoint.HighlightHologram();
        }
    }


    void OnTriggerExit(Collider collider)
    {
        // Debug.Log("Collision with SnapPoint (Leave)");
        if (collider.gameObject.tag == "SnapPoint" && _isGrabbing)
        {
            if (potentialSnapPoint == collider.gameObject.GetComponent<SnapPoint>())
            {
                potentialSnapPoint.UnhighlightHologram();
                potentialSnapPoint = null;
            }
        }
    }

    public bool IsSnapped()
    {
        return snapped;
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (IsSnapped())
        {
            photonView.RPC("UnSnap", RpcTarget.All);
        }
        SnapPoint.HolographicPreviewAll(gameObject);
        _isGrabbing = true;
    }
    public void OnInputUp(InputEventData eventData)
    {
        if (!snapped && potentialSnapPoint != null)
        {
            photonView.RPC("SnapTo", RpcTarget.All);
            transform.position = potentialSnapPoint.transform.position;
        }
        SnapPoint.StopHolographicPreviewAll();
        _isGrabbing = false;
    }

    [PunRPC]
    public void UnSnap()
    {
        this.snapped = false;

    }

    [PunRPC]
    public void SnapTo()
    {
        snapped = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        this.snapped = false;
        board = GameObject.FindGameObjectWithTag("GameBoard");
    }

    // Update is called once per frame
    void Update()
    {
        // if (IsSnapped())
        // {
        //     transform.position = snappedTo.transform.position;
        // }
    }
}
