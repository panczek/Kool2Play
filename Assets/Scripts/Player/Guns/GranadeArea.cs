using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GranadeArea : MonoBehaviour
{
    public Material changeMaterial;
    private List<GameObject> enemiesInside = new List<GameObject>();
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<EnemyController>() != null)
        {
            other.GetComponent<Renderer>().material = changeMaterial;
            enemiesInside.Add(other.gameObject);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyController>() != null)
        {
            other.GetComponent<Renderer>().material = other.GetComponent<EnemyController>().basicMaterial;
            if (enemiesInside.Contains(other.gameObject))
            {
                enemiesInside.Remove(other.gameObject);
            }
            
        }
        
    }
    private void OnDestroy()
    {
        foreach(GameObject enemy in enemiesInside)
        {
            if(enemy != null)
            {
                if (enemy.GetComponent<EnemyController>() != null)
                {
                    enemy.GetComponent<Renderer>().material = enemy.GetComponent<EnemyController>().basicMaterial;
                }
            }
            
        }
    }
}
