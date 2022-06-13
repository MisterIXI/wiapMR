using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;

public class PlaceableObject : MonoBehaviourPun, IMixedRealityInputHandler
{
    private GameObject board;
    private Collider ownCollider;
    private SnapPoint snappedTo;
    private SnapPoint potentialSnapPoint;



    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SnapPoint")
        {
            potentialSnapPoint = collision.gameObject.GetComponent<SnapPoint>();
        }
    }

    public void OnCollisionLeave(Collision collision)
    {
        if (collision.gameObject.tag == "SnapPoint")
        {
            if (potentialSnapPoint == collision.gameObject.GetComponent<SnapPoint>())
            {
                potentialSnapPoint = null;
            }
        }
    }

    public bool IsSnapped()
    {
        return snappedTo != null;
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (IsSnapped())
        {
            photonView.RPC("Unsnap", RpcTarget.All);
        }
    }
    public void OnInputUp(InputEventData eventData)
    {
        if (snappedTo == null && potentialSnapPoint != null)
        {
            photonView.RPC("SnapTo", RpcTarget.All, potentialSnapPoint);
        }
    }

    [PunRPC]
    public void UnSnap()
    {
        this.snappedTo = null;
        this.transform.SetParent(board.transform);

    }
    
    [PunRPC]
    public void SnapTo(SnapPoint snapPoint)
    {
        snappedTo = snapPoint;
        this.transform.SetParent(snapPoint.transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.snappedTo = null;
    }

    // Update is called once per frame
    void Update()
    {
        // if (IsSnapped())
        // {
        //     transform.position = this.transform.parent.transform.position;
        // }
    }
}
