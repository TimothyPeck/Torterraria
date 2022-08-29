using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int WIDTH = 100;
    public const int HEIGHT = 200;
    public static int cpt = 0;
    public static bool[][] filledPositions = new bool[2 * WIDTH][];
    //public static Dictionary<int, int> filledPositions = new Dictionary<int, int>();
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Spawn").transform.position = new Vector3(-WIDTH + 5, 5, 0);
        GameObject.FindGameObjectWithTag("Player").transform.position = GameObject.Find("Spawn").transform.position;
        //CreateGround();
    }

    public static void FillBlocksArray()
    {
        for (int i = -WIDTH; i < WIDTH; i++)
        {
            filledPositions[i + WIDTH] = new bool[2 * HEIGHT];
            for (int j = -HEIGHT; j < HEIGHT; j++)
            {
                filledPositions[i + WIDTH][j + HEIGHT] = false;
            }
        }
    }
}
