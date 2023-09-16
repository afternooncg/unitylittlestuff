using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
[RequireComponent(typeof(MeshFilter))]

[RequireComponent(typeof(MeshRenderer))]*/
public class DrawPlaneMain : MonoBehaviour
{
    
    
    
    // Start is called before the first frame update
    void Start()
    {

        GameObject go = new GameObject();
        MeshRenderer render = go.AddComponent<MeshRenderer>();
        MeshFilter filter = go.AddComponent<MeshFilter>();
        
        Mesh mesh = new Mesh();
        
        Vector3[] vector3s = new Vector3[3];
        vector3s[0] = new Vector3(-1, 1, 0);
        vector3s[1] = new Vector3(1, 1, 0);
        vector3s[2] = new Vector3(-1, -1, 0);
        
        vector3s[0] = new Vector3(0, 0, 0);
        vector3s[1] = new Vector3(0, 1, 0);
        vector3s[2] = new Vector3(1, 0, 0);
        mesh.vertices = vector3s;
        
       // vector3s[3] = new Vector3(1, -1, 0);

        int[] tris = new int[3]{0,1,2}; // new int[6]{0,1,2,1,3,2};
        mesh.triangles = tris;
        
        Vector2[] uv = new Vector2[3];
        uv[0] = new Vector2(0, 1);
        uv[1] = new Vector2(1, 1);
        uv[2] = new Vector2(0, 0);
       // uv[3] = new Vector2(1, 0);
      //  mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.RecalculateTangents();
        filter.mesh = mesh;



    }


    
        /*

        private List<Vector3> points = new List<Vector3>();

        // Use this for initialization

        void Start()
        {

            points.Add(new Vector3(0, 0, 0));

            points.Add(new Vector3(0, 1, 0));

            points.Add(new Vector3(1, 0, 0));

            MeshDrawTriangle();

        }

        private void MeshDrawTriangle()
        {

            GameObject go = new GameObject();
            MeshRenderer render = go.AddComponent<MeshRenderer>();
            MeshFilter filter = go.AddComponent<MeshFilter>();
            
            //新建一个Mesh

            Mesh triangleMesh = new Mesh();

            //把列表的顶点坐标赋给Mesh的vertexs

            triangleMesh.vertices = points.ToArray();

            //设置三角形顶点数量

            int[] trianglePoints = new int[3];

            trianglePoints[0] = 0;

            trianglePoints[1] = 1;

            trianglePoints[2] = 2;

            //把三角形的数量给Mesh的三角形

            triangleMesh.triangles = trianglePoints;

            //设置三角形的相关参数

            triangleMesh.RecalculateBounds();

            triangleMesh.RecalculateNormals();

            triangleMesh.RecalculateTangents();

            //把三角形的Mesh赋给MeshFilter组件

            //GetComponent<MeshFilter>().mesh = triangleMesh;
            filter.mesh = triangleMesh;

        }*/
    
}
