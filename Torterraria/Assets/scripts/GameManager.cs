using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int WIDTH = 100;
    public const int HEIGHT = 200;
    public static int cpt = 0;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Spawn").transform.position = new Vector3(-WIDTH + 5, 5, 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("Spawn").transform.position;
        //CreateGround();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
