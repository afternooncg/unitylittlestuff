using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithShaderMain : MonoBehaviour
{
    public GameObject go;
    private MaterialPropertyBlock block;
    void Awake() { 
    
        block = new MaterialPropertyBlock();
        if (go ==null) return; 
        // 生成100个球
        for (int i = 0; i < 100; ++i)
        {
            //随机生成位置 
            Vector3 pos = new Vector3(Random.Range(-50f, 50f), 0, Random.Range(-50f, 50f)); 
            GameObject instante = Instantiate<GameObject>(go, pos, Quaternion.identity); 
            //通过MaterialPropertyBock 设置颜色
            
            block.SetColor("_Color", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f));
            /*
            block.SetFloat("_Phi", Random.Range(-10f, 10f));
            block.SetFloat("_Speed", Random.Range(-10f, 10f));
            block.SetFloat("_A", Random.Range(-10f, 10f));*/
            
            //通过通过MaterialPropertyBock 更改材质表现
            instante.GetComponent<MeshRenderer>().SetPropertyBlock(block);
        }
    } 
     

}
