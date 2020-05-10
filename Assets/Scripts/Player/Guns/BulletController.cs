using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Parent script for different bullets
public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 5;
    public float timeToDestroy = 3f;

    

    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if(timeToDestroy <= 0f)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
}
