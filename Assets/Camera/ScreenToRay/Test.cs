using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // 碰撞开始
    void OnCollisionEnter(Collision collision) {
            var name = collision.collider.name;

            Debug.Log(  gameObject.name + " OnCollisionEnter " + name);
         }


    // 开始接触
    void OnTriggerEnter(Collider collider)
    {
        var name = collider.gameObject.name;
        Debug.Log(gameObject.name + " OnTriggerEnter" + name);
    }


}
