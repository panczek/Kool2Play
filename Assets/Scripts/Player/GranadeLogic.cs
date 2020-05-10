using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeLogic : MonoBehaviour
{
    public List<Vector3> path;
    public float explosionRadius = 7f;
    public GameObject explosionEffect;
    public float damage = 15f;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
        float step = 10f * Time.deltaTime;
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
        GameObject manager = GameObject.FindGameObjectWithTag("GameController");
        manager.GetComponent<GameManager>().PlayExplosionSounds();
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
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
