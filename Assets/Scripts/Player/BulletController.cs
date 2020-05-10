using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 5;
    public float timeToDestroy = 3f;
    public bool shotgunBullet = false;
    public int shotgunTargets = 3;
    private int passedTargets = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToDestroy -= Time.deltaTime;
        if(timeToDestroy <= 0f)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "LevelObject")
        {
            Destroy(this.gameObject);
        }
        if(other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().ReceiveDamage(damage);
            if (shotgunBullet)
            {
                passedTargets++;
                if(passedTargets >= shotgunTargets)
                {
                    Destroy(this.gameObject);
                }
            }
            else
            {
                Destroy(this.gameObject);
            }
            
        }
    }
}
