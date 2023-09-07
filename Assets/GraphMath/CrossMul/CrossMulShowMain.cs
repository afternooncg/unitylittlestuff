using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CrossMulShowMain : MonoBehaviour
{
    public Transform A;
    public Transform B;

    private Vector3 worldCenter = Vector3.zero;
    private Vector3 c = Vector3.zero;

    void Update()
    {
        Vector3 a = A.position - worldCenter;
        Vector3 b = B.position - worldCenter;

        //point:要围绕的点；axiw:要围绕的轴，如x,y,z angel:旋转的角度
        A.RotateAround(worldCenter, Vector3.forward, 30.0f * Time.deltaTime);

        c = Vector3.Cross(a, b);

#if UNITY_EDITOR
        Debug.DrawLine(worldCenter, A.position, Color.white);
        Debug.DrawLine(worldCenter, B.position, Color.black);
        Debug.DrawLine(worldCenter, c - worldCenter, Color.red);
#endif
    }

    private void OnDrawGizmos()
    {
        Handles.Label(c, c.ToString());
    }
}