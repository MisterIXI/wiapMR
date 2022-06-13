using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private Color SP_COLOR_NORMAL = new Color(0.3820755f, 0.9336458f, 1f, 0.5333334f);
    private Color SP_COLOR_HIGHLIGHT = new Color(0.2978373f,0.9150943f,0.4001075f, 0.5333334f);
    private static List<SnapPoint> snapPoints = new List<SnapPoint>();
    private List<SnapPoint> connectedSnapPoints;
    private PlaceableObject placeableObject = null;
    private GameBoard gameboard = null;
    private MeshFilter meshFilter;
    private Material material;

    public
    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        material = GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HolographicPreviewStart(GameObject obj)
    {
        enabled = true;
        material.color = SP_COLOR_NORMAL;
        meshFilter.mesh = obj.GetComponent<MeshFilter>().mesh;
    }

    public void StopHolographicPreview()
    {
        enabled = false;
    }
    public static void HolographicPreviewAll(GameObject obj)
    {
        foreach (SnapPoint snapPoint in snapPoints)
        {
            snapPoint.HolographicPreviewStart(obj);
        }
    }

    public static void StopHolographicPreviewAll()
    {
        foreach (SnapPoint snapPoint in snapPoints)
        {
            snapPoint.StopHolographicPreview();
        }
    }
    public static void ClearSnapPointList()
    {
        snapPoints.Clear();
    }
}
