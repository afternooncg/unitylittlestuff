using System;
using UnityEngine;
using System.Collections;

public class ScreenPointToRay_ts : MonoBehaviour
{
    public Vector3 planeCenter;

    Ray ray;
    RaycastHit hit;
    Vector3 v3 = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
    Vector3 hitpoint = Vector3.zero;
    void Update()
    {
        //射线沿着屏幕X轴从左向右循环扫描
        v3.x = v3.x >= Screen.width ? 0.0f : v3.x + 1.0f;
        //生成射线
        ray = GetComponent<Camera>().ScreenPointToRay(v3);
        if (Physics.Raycast(ray, out hit, 10.0f))
        {
            //绘制线，在Scene视图中可见
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            //输出射线探测到的物体的名称
            //Debug.Log("射线探测到的物体名称：" + hit.transform.name);
        }

        Debug.DrawLine(Vector3.zero, planeCenter, Color.black);
        Debug.DrawLine(planeCenter, planeCenter+Vector3.forward, Color.red);


        Debug.Log(-Vector3.Dot(Vector3.forward, planeCenter));
    }

    private void OnDrawGizmos()
    {

    }
}