using System;
using UnityEditor;

namespace LittleDemo.Bezier
{


using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class BezierDraw : MonoBehaviour
{
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;

    private int layerOrder = 0;
    private int _segmentNum = 50;


    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        lineRenderer.sortingLayerID = layerOrder;
    }

    void Update()
    {
        DrawCurve();
    }

    void DrawCurve()
    {
        for (int i = 1; i <= _segmentNum; i++)
        {
            float t = i / (float)_segmentNum;
            int nodeIndex = 0;
            Vector3 pixel = BezierUtils.CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position,
                controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position);
            lineRenderer.positionCount = i;
            lineRenderer.SetPosition(i - 1, pixel);
        }
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < _segmentNum; i++)
        {
            
            Gizmos.DrawSphere(lineRenderer.GetPosition(i),0.2f);
        }
#if UNITY_EDITOR
        Handles.Label(Vector3.zero, "P0");
        Handles.Label(Vector3.one*10, "P1");
        Handles.SphereHandleCap(0, Vector3.one*10, Quaternion.identity, .1f, EventType.Repaint);
        Gizmos.DrawLine(Vector3.zero,Vector3.one*10);
#endif
    }
}
}
