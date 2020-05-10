using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holdRotation : MonoBehaviour
{
    Quaternion rotation;
    public GameObject player;
    private void Awake()
    {
        rotation = transform.rotation;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = rotation;
    }
    private void LateUpdate()
    {
        if(player != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }
    }
}
