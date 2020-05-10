using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    
    public float fSpeed = 8f;
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 mousePos;
    public Camera playerCamera;

    public GameObject HealthBarImage;
    public Image HealtBar;
    private List<GameObject> HealthBars = new List<GameObject>();
    public int health;
    private int maxHealth;
    Plane playerPlane;
    Ray ray; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        maxHealth = health;
        SpawnHealthBar();
        playerPlane = new Plane(Vector3.up, transform.position);
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.z = Input.GetAxis("Vertical");
        movement.y = 0f;

        rb.MovePosition(rb.position + movement * fSpeed * Time.deltaTime);

        ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDis = 0f;

        if (playerPlane.Raycast(ray, out hitDis))
        {
            Vector3 targetPoint = ray.GetPoint(hitDis);
            Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
            targetRotation.x = 0f;
            targetRotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 7f * Time.deltaTime);
        }
    }
    void SpawnHealthBar()
    {
        
        for(int i = 0; i < maxHealth; i++)
        {
            Image healthBar = GameObject.Instantiate(HealtBar, Vector3.zero, Quaternion.identity, HealthBarImage.transform);
            healthBar.GetComponent<RectTransform>().anchoredPosition = new Vector3(0 + i * 46, 0, 0);
            healthBar.name = i.ToString();
            HealthBars.Add(healthBar.gameObject);
        }
        
    }
    void UpdateHealthBar()
    {
        
        for(int i = 0; i < maxHealth + 1; i++)
        {
            if(health < i)
            {
                HealthBars[i - 1].SetActive(false);
            }
        }
    }
    public void ReceiveDamage(int damage)
    {
        health -= damage;
        UpdateHealthBar();
        if(health <= 0)
        {
            GameObject manager = GameObject.FindGameObjectWithTag("GameController");
            manager.GetComponent<GameManager>().EndGame();
            Destroy(this.gameObject);
        }
    }
    
   

}
