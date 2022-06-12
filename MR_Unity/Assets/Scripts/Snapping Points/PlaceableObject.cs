using UnityEngine;
using Photon.Pun;
using Microsoft.MixedReality.Toolkit.Input;

public class PlaceableObject : MonoBehaviourPun, IMixedRealityInputHandler
{
    private bool isSnapping;
    private SnapPoint snappedTo;
    private bool isGrabbed;
    public void SnapTo(SnapPoint snapPoint)
    {
        this.snappedTo = snapPoint;
        this.isSnapping = true;
        this.transform.SetParent(snapPoint.transform);
    }

    public void UnSnap()
    {
        this.snappedTo = null;
        this.isSnapping = false;
        this.transform.parent = null;
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
        return this.isSnapping;
    }

    public void OnInputDown(InputEventData eventData)
    {
        this.isGrabbed = true;
        if (IsSnapped()) {
            this.UnSnap();
        }
    }
    public void OnInputUp(InputEventData eventData)
    {
        this.isGrabbed = false;
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
        if (IsSnapped())
        {
            transform.position = this.transform.parent.transform.position;
        }
    }
}
