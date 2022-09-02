using System.Collections.Generic;
using UnityEngine;

namespace WiapMR.GameScripts
{
    public class SnapPoint : MonoBehaviour
    {
        private readonly Color _spColorNormal = new Color(0.3820755f, 0.9336458f, 1f, 0.5333334f);
        private readonly Color _spColorHighlight = new Color(0.2978373f, 0.9150943f, 0.4001075f, 0.5333334f);
        private static readonly List<SnapPoint> _snapPoints = default;
        private List<SnapPoint> _connectedSnapPoints;
        private Material _material;

        void Start()
        {
            _material = GetComponent<MeshRenderer>().material;
            _snapPoints.Add(this);
        }

        public void HighlightHologram()
        {
            if (gameObject.activeSelf)
                _material.SetColor("_Color", _spColorHighlight);
        }

        public void UnhighlightHologram()
        {
            if (gameObject.activeSelf)
                _material.SetColor("_Color", _spColorNormal);
        }

        public void HolographicPreviewStart(GameObject obj)
        {
            // Debug.Log("HolographicPreviewStart | Material: " + material);
            gameObject.SetActive(true);
            _material.color = _spColorNormal;
            // meshFilter.mesh = obj.GetComponent<MeshFilter>().mesh;
        }

        public void StopHolographicPreview()
        {
            gameObject.SetActive(false);
        }
        public static void HolographicPreviewAll(GameObject obj)
        {
            foreach (SnapPoint snapPoint in _snapPoints)
            {
                snapPoint.HolographicPreviewStart(obj);
            }
        }

        public static void StopHolographicPreviewAll()
        {
            foreach (SnapPoint snapPoint in _snapPoints)
            {
                snapPoint.StopHolographicPreview();
            }
        }
        public static void ClearSnapPointList()
        {
            _snapPoints.Clear();
        }
    }
}