using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBullet : BulletController
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelObject")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().ReceiveDamage(damage);
            Destroy(this.gameObject);

        }
    }
}
