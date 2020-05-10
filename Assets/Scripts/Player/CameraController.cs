using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    private GameObject m_player;
    // Start is called before the first frame update
    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_player != null)
        {
            transform.position = new Vector3(m_player.transform.position.x, 17.5f, m_player.transform.position.z - 10f);
        }
        
    }
}
