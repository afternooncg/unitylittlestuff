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


}
}
