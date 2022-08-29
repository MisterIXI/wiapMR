using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class localSync : MonoBehaviour
{
    public GameObject head;
    public GameObject board;
    public uint rayID;
    void Update()
    {
        Vector3 relativePos = head.transform.position - board.transform.position;
        // apply board.transform.rotation to relativePos
        transform.position = board.transform.rotation * relativePos + board.transform.position;
        transform.rotation = board.transform.rotation;
        transform.localScale = board.transform.localScale;
    }

    // draw forward vector of this object
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 startPos;
        Vector3 endPos;
        switch (rayID)
        {
            case 0:
                startPos = Vector3.zero;
                endPos = board.transform.position;
                break;
            case 1:
                startPos = Vector3.zero;
                endPos = head.transform.position;
                break;
            case 2:
                startPos = board.transform.position;
                endPos = (head.transform.position - board.transform.position);
                break;
            case 3:
                startPos = board.transform.position;
                endPos = board.transform.rotation * (head.transform.position - board.transform.position);
                break;
            default:
                startPos = Vector3.zero;
                endPos = Vector3.zero;
                break;
        }
        Gizmos.DrawRay(startPos, endPos);
        // draw ray from center of board transform to center of head transform
        // Gizmos.DrawRay(board.transform.position, head.transform.position - board.transform.position);

    }
}
