using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;

public class PlaceableObject : MonoBehaviourPun, IMixedRealityInputHandler
{
    private GameObject board;
    private Collider ownCollider;
    private SnapPoint snappedTo;
    public void SnapTo(SnapPoint snapPoint)
    {
        snappedTo = snapPoint;
        this.transform.SetParent(snapPoint.transform);
    }

    public void UnSnap()
    {
        this.snappedTo = null;
        this.transform.SetParent(board.transform);

    }

    public void OnCollisionStay(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag.Equals("Snappoint"))
        {
            Debug.Log("Collision with Snappoint");
            if (!isGrabbed)
            {
                Debug.Log("Snapping now.");
                this.SnapTo(obj.GetComponent<SnapPoint>());
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
            this.UnSnap();
        }
    }
    public void OnInputUp(InputEventData eventData)
    {
        if(GetComponent<)
    }

    // Start is called before the first frame update
    void Start()
    {
        
        this.isSnapping = false;
        this.snappedTo = null;
        this.isGrabbed = false;
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
