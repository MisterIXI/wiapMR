using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    private Color SP_COLOR_NORMAL = new Color(0.3820755f, 0.9336458f, 1f, 0.5333334f);
    private Color SP_COLOR_HIGHLIGHT = new Color(0.2978373f, 0.9150943f, 0.4001075f, 0.5333334f);
    private static List<SnapPoint> snapPoints = new List<SnapPoint>();
    private List<SnapPoint> connectedSnapPoints;
    private PlaceableObject placeableObject = null;
    private GameBoard gameboard = null;
    private MeshFilter meshFilter;
    private Material material;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        material = GetComponent<MeshRenderer>().material;
        snapPoints.Add(this);
        // gameObject.SetActive(false);
    }


    public void HighlightHologram()
    {
        if (gameObject.activeSelf)
            material.SetColor("_Color", SP_COLOR_HIGHLIGHT);
    }

    public void UnhighlightHologram()
    {
        if (gameObject.activeSelf)
            material.SetColor("_Color", SP_COLOR_NORMAL);
    }

    public void HolographicPreviewStart(GameObject obj)
    {
        Debug.Log("HolographicPreviewStart | Material: " + material);
        gameObject.SetActive(true);
        material.color = SP_COLOR_NORMAL;
        meshFilter.mesh = obj.GetComponent<MeshFilter>().mesh;
    }

    public void StopHolographicPreview()
    {
        gameObject.SetActive(false);
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
