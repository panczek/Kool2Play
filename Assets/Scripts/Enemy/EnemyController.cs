using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float health;
    public float maxHealth = 30;

    public float speed = 2f;
    private float doubleSpeed;
    private float normalSpeed;

    public GameObject player;
    public int damage = 1;
    private Slider slider;
    private GameManager gameManager;

    public Material basicMaterial; //copy of the enemy material
    public bool onFire = false;
    public GameObject fire;
    public GameObject dieExplosion;
    private bool fireSpawn = false;
    private GameObject fireplace;
    private float fireCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        doubleSpeed = speed * 2f;
        normalSpeed = speed;
        slider = GetComponentInChildren<Slider>();
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        SetSlider();
        basicMaterial = this.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            float step = speed * Time.deltaTime;
            Transform playerPos = player.transform;
            Vector3 targetDirection = playerPos.position - transform.position;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, step, 0.0f);

            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, step);
            transform.rotation = Quaternion.LookRotation(newDirection);
            if (Vector3.Distance(transform.position, playerPos.position) < 2f)
            {
                if (player != null)
                {
                    Instantiate(dieExplosion, transform.position, Quaternion.identity);
                    player.GetComponent<PlayerController>().ReceiveDamage(damage);
                }
                gameManager.EnemiesKilled--;
                Destroy(this.gameObject);
            }
            if(Vector3.Distance(transform.position, playerPos.position) < 10f)
            {
                speed = doubleSpeed;
            }
            else
            {
                speed = normalSpeed;
            }
        }
        
        if (onFire)
        {
            
            if (!fireSpawn)
            {
                fireplace = Instantiate(fire, transform.position, Quaternion.identity);
                
                fireSpawn = true;
            }
            fireplace.transform.parent = this.transform;
            fireCounter += Time.deltaTime;
            if(fireCounter > 1f)
            {
                fireCounter = 0;
                ReceiveDamage(1);
            }
        }
    }

    public void ReceiveDamage(float damage)
    {
        health -= damage;
        SetSlider();
        if (health <= 0)
        {
            Instantiate(dieExplosion, transform.position, Quaternion.identity);
            
            Destroy(this.gameObject);
        }
    }
    
    void SetSlider()
    {
        slider.value = health / maxHealth;
    }
    private void OnDestroy()
    {

        gameManager.EnemiesKilled++;

    }

}
