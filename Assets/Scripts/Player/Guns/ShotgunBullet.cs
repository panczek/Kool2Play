using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShotgunBullet : BulletController
{
    //shotgun bullet values
    public int shotgunTargets = 3;
    private int passedTargets = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "LevelObject")
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().ReceiveDamage(damage);
            passedTargets++;
            if (passedTargets >= shotgunTargets)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
