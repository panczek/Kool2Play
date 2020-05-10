using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeLogic : MonoBehaviour
{
    public List<Vector3> path;
    public float explosionRadius = 7f;
    public GameObject explosionEffect;
    public float damage = 15f;
    public float speed = 10f;

    void Update()
    {
        
        float step = speed * Time.deltaTime;
        if (path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[0], step);
            if (Vector3.Distance(transform.position, path[0]) < 0.1f)
            {
                path.RemoveAt(0);
            }
        }
        if(path.Count == 0)
        {
            Explode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LevelObject")
        {
            Explode();
        }
    }
    void Explode()
    {
        //play explosion sound
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        manager.GetComponent<GameManager>().PlayExplosionSounds();
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        //hit enemies in the radius
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in enemies)
        {
            if(Vector3.Distance(transform.position, enemy.transform.position) < explosionRadius)
            {
                enemy.GetComponent<EnemyController>().ReceiveDamage(damage);
                enemy.GetComponent<EnemyController>().onFire = true;
            }
        }
        Destroy(this.gameObject);
    }

}
