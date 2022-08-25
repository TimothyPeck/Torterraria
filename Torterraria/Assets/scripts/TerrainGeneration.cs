using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    public GameObject grassType = null;
    public GameObject dirtType = null;
    public GameObject stoneType = null;
    public GameObject ironType = null;
    private List<float> usedX = new List<float>();

    void Start()
    {
        // Modified version of https://www.youtube.com/watch?v=fHZGJuRfDUs
        //Goes from as far left to as far right as possible
        Transform groundTransform = GameObject.Find("Ground").transform;
        for (int i = -GameManager.WIDTH; i < GameManager.WIDTH; i++)
        {
            // Gets the position of the block and centers it on the location.
            int xPosition = Mathf.CeilToInt(i);
            int yPosition = Mathf.CeilToInt(-6f + Mathf.PerlinNoise(i / 30f, 0f) * 4f);
            // Ensures that there are no overlapping blocks.
            if (!usedX.Contains(xPosition))
            {
                // Goes from the floor to the lowest position
                for (int j = yPosition; j > -GameManager.HEIGHT; j--)
                {
                    GameObject bx;
                    // Randomly creates iron in the stone level.
                    if (j < -50 && j <= UnityEngine.Random.Range(-GameManager.HEIGHT, yPosition) * 10 && xPosition / 10 <= UnityEngine.Random.Range(-GameManager.WIDTH, GameManager.WIDTH) * 100)
                    {
                        bx = GameObject.Instantiate(ironType);
                        bx.name = "iron" + GameManager.cpt;
                    }
                    // If on ground level, make the block grass.
                    else if (j == yPosition)
                    {
                        bx = GameObject.Instantiate(grassType);
                        bx.name = "grass" + GameManager.cpt;
                    }
                    // If the block is 50 blocks lower than the ground then the block is dirt
                    else if (j < yPosition && j > yPosition - GameManager.HEIGHT / 4)
                    {
                        bx = GameObject.Instantiate(dirtType);
                        bx.name = "dirt" + GameManager.cpt;
                    }
                    // If the block is lower than that, make the block stone.
                    else
                    {
                        bx = GameObject.Instantiate(stoneType);
                        bx.name = "stone" + GameManager.cpt;
                    }
                    
                    // Adds to parent.
                    bx.transform.parent = groundTransform;
                    // Adds tag to make it breakable
                    bx.tag = "Ground";
                    GameManager.cpt++;
                    // Moves the block to the correct location.
                    bx.transform.position = new Vector3((float)xPosition, (float)j, 0f);
                }
                // Adds the used position so as to not use it again.
                usedX.Add(xPosition);
            }
        }
        GameObject bedrock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bedrock.transform.parent = groundTransform;
        bedrock.name = "bedrock" + GameManager.cpt;
        GameManager.cpt++;
        bedrock.GetComponent<MeshRenderer>().material.color = Color.black;
        bedrock.transform.localScale = new Vector3(GameManager.WIDTH, 1, 1);
        bedrock.transform.position = new Vector3(0, -GameManager.HEIGHT, 0f);

        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.transform.parent = groundTransform;
        ceiling.name = "ceiling" + GameManager.cpt;
        GameManager.cpt++;
        ceiling.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ceiling.GetComponent<Renderer>().enabled = false;
        ceiling.transform.localScale = new Vector3(GameManager.WIDTH, 1, 1);
        ceiling.transform.position = new Vector3(0, GameManager.HEIGHT, 0f);
    
        GameObject wallLeft=GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallLeft.transform.parent = groundTransform;
        wallLeft.name = "WallLeft";
        wallLeft.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        wallLeft.GetComponent<Renderer>().enabled = false;
        wallLeft.transform.localScale = new Vector3(1, 1000, 1);
        wallLeft.transform.position = new Vector3(-GameManager.WIDTH - 1, 0, 0);

        GameObject wallRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallRight.transform.parent = groundTransform;
        wallRight.name = "WallRight";
        wallRight.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        wallRight.GetComponent<Renderer>().enabled = false;
        wallRight.transform.localScale = new Vector3(1, 1000, 1);
        wallRight.transform.position = new Vector3(GameManager.WIDTH, 0, 0);
    }
}
