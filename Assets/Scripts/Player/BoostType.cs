using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BoostType : MonoBehaviour
{
    public PlayerBoosts.BoostType typeOfBoost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerBoosts>() != null)
        {
            PlayerBoosts pBoost = other.GetComponent<PlayerBoosts>();
            pBoost.EndBoost();
            pBoost.Boost = typeOfBoost;
            pBoost.StartBoost();
            Destroy(this.gameObject);
        }
    }
}
