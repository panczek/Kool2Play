using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShooting : MonoBehaviour
{
    //Guns variables
    public Transform spawnPoint1;//gun1
    public Transform spawnPoint2;//gun2
    public Transform spawnPoint3; //grenade spawning
    public GameObject bulletParent;

    public GameObject pistolBullets;
    public GameObject ShotgunBulltes;
    public GameObject grenadeArea;
    public GameObject grenade;
    public GameObject shootingEffect;

    private GameObject grenadeAreaObject;
    private bool spawnGrenadeArea = true;
    public int grenadeLinePointsNo = 10;

    public float pistolCooldown = 0.2f;
    public float shotgunCooldown = 1.2f;
    public float grenadeCooldown = 5f;

    private float pistolActualCooldown = 0;
    private float shotgunActualCooldown = 0;
    public float grenadeActualCooldown = 0;
    private int pistolNo = 1;//gun shoot from
    public int shotgunBulletsNo = 8;
    public float shotgunBulletSpred = 0.1f;

    public LineRenderer grenataLine;
    public Slider grenadeTimeSlider;

    public AudioClip[] pistolSounds;
    public AudioClip[] shotgunSounds;
    public AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))// && m_EquipedGun == 0)
        {
            ShootPistol();
        }
        else if (Input.GetButtonDown("Fire2"))// && m_EquipedGun == 1)
        {
            ShootShotgun();
        }
        if (Input.GetKey(KeyCode.E))
        {
            ThrowGrenade(false);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            ThrowGrenade(true);
        }
        pistolActualCooldown = CooldownReduction(pistolActualCooldown);
        shotgunActualCooldown = CooldownReduction(shotgunActualCooldown);
        grenadeActualCooldown = CooldownReduction(grenadeActualCooldown);

        grenadeTimeSlider.value = (grenadeCooldown - grenadeActualCooldown) / grenadeCooldown;


    }
    void ShootPistol()
    {
        
        if (pistolActualCooldown <= 0f)
        {
            Transform spawn = spawnPoint1;
            //change spawn points
            if (pistolNo == 1)
            {
                pistolNo = 2;
            }else if(pistolNo == 2)
            {
                spawn = spawnPoint2;
                pistolNo = 1;
            }
            GameObject bullet = Instantiate(pistolBullets, spawn.position, spawn.rotation);
            bullet.transform.parent = bulletParent.transform;
            //bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward, ForceMode.Impulse);
            pistolActualCooldown = pistolCooldown;
            SpawnShootingEffect(spawn);
            int random = UnityEngine.Random.Range(0, pistolSounds.Length);
            playerAudio.PlayOneShot(pistolSounds[random]);
        }
        
    }
    void ShootShotgun()
    {
        //my implementation
        if(shotgunActualCooldown <= 0f)
        {
            for (int i = 0; i < 2; i++)
            {
                Transform spawn = spawnPoint1;
                if (i == 1)
                {
                    spawn = spawnPoint2;
                }
                SpawnShootingEffect(spawn);
                for (int j = 0; j < shotgunBulletsNo; j++)
                {
                    //random bullet directions
                    float r1 = UnityEngine.Random.Range(-shotgunBulletSpred, shotgunBulletSpred);
                    float r2 = UnityEngine.Random.Range(-shotgunBulletSpred, shotgunBulletSpred);
                    float r3 = UnityEngine.Random.Range(-shotgunBulletSpred, shotgunBulletSpred);

                    Quaternion rotation = new Quaternion(spawn.rotation.x + 0f, spawn.rotation.y + r1, spawn.rotation.z + r2, spawn.rotation.w + r3);
                    GameObject bullet = Instantiate(ShotgunBulltes, spawn.position, rotation);
                    bullet.transform.parent = bulletParent.transform;
                }
            }
            shotgunActualCooldown = shotgunCooldown;
            int random = UnityEngine.Random.Range(0, shotgunSounds.Length);
            playerAudio.PlayOneShot(shotgunSounds[random]);
        }
        //project description implementation
        /*
          Instantiate cone shape - only collider
          return list of objects from the collision
          for loop every enemy from list and make a ray check
          if ray check true - hit enemy
         
         */

    }
    void SpawnShootingEffect(Transform spawnPoint)
    {
        Instantiate(shootingEffect, spawnPoint.position, spawnPoint.rotation);
    }
    float CooldownReduction(float value)
    {
        
        if (value > 0f)
        {
            value -= Time.deltaTime;
        }
        return value;
    }
    void ThrowGrenade(bool throwG)
    {
        if(grenadeActualCooldown <= 0f)
        {
            grenataLine.enabled = true;
            
            List<Vector3> grenadePath = new List<Vector3>(grenadeLinePointsNo);//path for the grenade 
            Vector3 myPosiiton = transform.position;
            Plane playerPlane = new Plane(Vector3.up, myPosiiton);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 targetPoint = new Vector3();
            float hitDis = 0f;
            if (playerPlane.Raycast(ray, out hitDis))
            {
                targetPoint = ray.GetPoint(hitDis);
            }


            Vector3 lineVector = targetPoint - myPosiiton;//vector from player pos to mouse pos
            Vector3 step = lineVector / (float)grenadeLinePointsNo;//single line segment
            grenataLine.positionCount = grenadeLinePointsNo;
            Vector3 linePoint;
            float DistanceBetweenPoints = Vector3.Distance(targetPoint, myPosiiton);

            grenadePath.Clear();
            float hightValue = Mathf.Lerp(1f, 9f, DistanceBetweenPoints / 20f);//hight factor - smaller = lower lobe / higher = taller lobe
            
            for (int i = 0; i < grenadeLinePointsNo; i++)
            {
                linePoint = myPosiiton + step * i;
                double y = Mathf.Sin(Mathf.Deg2Rad * ((i / (float)grenadeLinePointsNo) * 180f));//hight of the curve
                linePoint.y = (float)y * hightValue;
                
                grenataLine.SetPosition(i, linePoint);
                grenadePath.Add(linePoint);
            }
            

            if (spawnGrenadeArea)
            {
                grenadeAreaObject = Instantiate(grenadeArea, targetPoint, Quaternion.identity);
                spawnGrenadeArea = false;
            }
            if (grenadeAreaObject != null)
            {
                grenadeAreaObject.transform.position = targetPoint;
            }
            if (throwG)
            {
                SpawnGrenade(grenadePath);
            }
        }
       
        
    }
    void SpawnGrenade(List<Vector3> path)
    {
        grenataLine.enabled = false;
        Destroy(grenadeAreaObject);
        spawnGrenadeArea = true;
        GameObject gren = Instantiate(grenade, spawnPoint3.position, Quaternion.identity);
        gren.transform.parent = bulletParent.transform;
        path.RemoveAt(0);
        gren.GetComponent<GranadeLogic>().path = path;
        grenadeActualCooldown = grenadeCooldown;
    }
}
