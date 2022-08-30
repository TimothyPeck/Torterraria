using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;

public class TerrainGeneration : MonoBehaviour
{
    /// <summary>
    /// The grass type to be used
    /// </summary>
    public GameObject grassType = null;
    /// <summary>
    /// The dirt type to be used
    /// </summary>
    public GameObject dirtType = null;
    /// <summary>
    /// The stone type to be used
    /// </summary>
    public GameObject stoneType = null;
    /// <summary>
    /// The iron type to be used
    /// </summary>
    public GameObject ironType = null;
    /// <summary>
    /// The platform type to be used
    /// </summary>
    public GameObject platformType = null;

    public GameObject superDirtType = null;
    public GameObject superStoneType = null;
    public GameObject superWoodType = null;
    /// <summary>
    /// The trees to be used
    /// </summary>
    public GameObject[] trees;
    /// <summary>
    /// The x coordinates that have already been done, avoids overlaps
    /// </summary>
    private List<float> usedX = new List<float>();
    /// <summary>
    /// The coordinates at which the trees will appear
    /// </summary>
    private List<int> treesX = new List<int>();
    /// <summary>
    /// The minimum spacing between the trees
    /// </summary>
    private int treeSpacing = 5;
    /// <summary>
    /// 
    /// </summary>
    public static bool[][] filledPositions = new bool[2 * GameManager.WIDTH][];

    void Start()
    {
        //Fills the array of used blocks before use.
        for (int i = -GameManager.WIDTH; i < GameManager.WIDTH; i++)
        {
            filledPositions[i + GameManager.WIDTH] = new bool[2 * GameManager.HEIGHT];
            for (int j = -GameManager.HEIGHT; j < GameManager.HEIGHT; j++)
            {
                filledPositions[i + GameManager.WIDTH][j + GameManager.HEIGHT] = false;
            }
        }

        // Modified version of https://www.youtube.com/watch?v=fHZGJuRfDUs
        //Goes from as far left to as far right as possible
        Transform groundTransform = GameObject.Find("Ground").transform;
        // Gets random positions for trees.
        for(int i = 0; i < GameManager.WIDTH / treeSpacing; i++)
        {
            treesX.Add(GetRandomNumber(-GameManager.WIDTH + treeSpacing * 2, GameManager.WIDTH - treeSpacing));
        }
        //From max left ot max right
        for (int i = -GameManager.WIDTH; i < GameManager.WIDTH; i++)
        {
            // Gets the position of the block and centers it on the location.
            int xPosition = Mathf.CeilToInt(i);
            int yPosition = Mathf.CeilToInt(-6f + Mathf.PerlinNoise(i / 30f, 0f) * 8f);

            if (xPosition < -75 && xPosition > -80)
            {
                for (int j = GameManager.HEIGHT - 1; j > -GameManager.HEIGHT; j--)
                {
                    GameObject bx;
                    bx = GameObject.Instantiate(superDirtType);
                    bx.name = "superDirt_" + GameManager.cpt;
                    GameManager.cpt++;
                    bx.transform.parent = groundTransform;
                    bx.tag = "SuperDirt";
                    bx.transform.position = new Vector3((float)xPosition, (float)j, 0f);

                    filledPositions[xPosition + GameManager.WIDTH][j + GameManager.HEIGHT] = true;
                }
            }
            else if (xPosition < 2 && xPosition > -2)
            {
                for (int j = GameManager.HEIGHT - 1; j > -GameManager.HEIGHT; j--)
                {
                    GameObject bx;
                    bx = GameObject.Instantiate(superStoneType);
                    bx.name = "superStone_" + GameManager.cpt;
                    GameManager.cpt++;
                    bx.transform.parent = groundTransform;
                    bx.tag = "SuperStone";
                    bx.transform.position = new Vector3((float)xPosition, (float)j, 0f);

                    filledPositions[xPosition + GameManager.WIDTH][j + GameManager.HEIGHT] = true;
                }
            }
            else if (xPosition >75 && xPosition < 80)
            {
                for (int j = GameManager.HEIGHT - 1; j > -GameManager.HEIGHT; j--)
                {
                    GameObject bx;
                    bx = GameObject.Instantiate(superWoodType);
                    bx.name = "superWood_" + GameManager.cpt;
                    GameManager.cpt++;
                    bx.transform.parent = groundTransform;
                    bx.tag = "SuperWood";
                    bx.transform.position = new Vector3((float)xPosition, (float)j, 0f);

                    filledPositions[xPosition + GameManager.WIDTH][j + GameManager.HEIGHT] = true;
                }
            }
            // Ensures that there are no overlapping blocks.
            else if (!usedX.Contains(xPosition))
            {
                // Goes from the floor to the lowest position
                for (int j = yPosition; j > -GameManager.HEIGHT; j--)
                {
                    GameObject bx;
                    // Randomly creates iron in the stone level.
                    if (j < -50 && j <= UnityEngine.Random.Range(-GameManager.HEIGHT, yPosition) * 10 && xPosition / 10 <= UnityEngine.Random.Range(-GameManager.WIDTH, GameManager.WIDTH) * 100)
                    {
                        bx = GameObject.Instantiate(ironType);
                        bx.name = "iron_" + GameManager.cpt;
                    }
                    // If on ground level, make the block grass.
                    else if (j == yPosition)
                    {
                        bx = GameObject.Instantiate(grassType);
                        bx.name = "grass_" + GameManager.cpt;
                    }
                    // If the block is 50 blocks lower than the ground then the block is dirt
                    else if (j < yPosition && j > yPosition - GameManager.HEIGHT / 4)
                    {
                        bx = GameObject.Instantiate(dirtType);
                        bx.name = "dirt_" + GameManager.cpt;
                    }
                    // If the block is lower than that, make the block stone.
                    else
                    {
                        bx = GameObject.Instantiate(stoneType);
                        bx.name = "stone_" + GameManager.cpt;
                    }

                    // Adds to parent.
                    bx.transform.parent = groundTransform;
                    // Adds tag to make it breakable
                    bx.tag = "Ground";
                    GameManager.cpt++;
                    // Moves the block to the correct location.
                    bx.transform.position = new Vector3((float)xPosition, (float)j, 0f);

                    filledPositions[xPosition + GameManager.WIDTH][yPosition + GameManager.HEIGHT] = true;
                }
                // Adds the used position so as to not use it again.
                usedX.Add(xPosition);

                //Generates a tree at the given position
                if (treesX.Contains(xPosition))
                {
                    GameObject tree = GameObject.Instantiate(trees[UnityEngine.Random.Range(0, trees.Length)]);
                    tree.name = "tree" + GameManager.cpt;
                    tree.transform.parent = groundTransform;
                    tree.transform.localScale = new Vector3(1, 1, 0.1f);
                    tree.transform.position = new Vector3(xPosition, yPosition + 1, 0.5f);
                }
            }
        }

        // Makes a plane as the background to detect where the player clicks when a block is not available.
        // Allows the player to place a block on nothing.
        GameObject back = GameObject.CreatePrimitive(PrimitiveType.Plane);
        back.GetComponent<Renderer>().enabled = false;
        back.transform.parent = groundTransform;
        back.transform.position = new Vector3(0, 0, 0.6f);
        back.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        back.transform.localScale = new Vector3(40f, 21f, 21f);
        back.name = "backPlane";

        // Creates the bedrock layer at the bottom of the map
        GameObject bedrock = GameObject.CreatePrimitive(PrimitiveType.Cube);
        bedrock.transform.parent = groundTransform;
        bedrock.name = "bedrock";
        bedrock.GetComponent<MeshRenderer>().material.color = Color.black;
        bedrock.transform.localScale = new Vector3(2 * GameManager.WIDTH, 1, 1);
        bedrock.transform.position = new Vector3(0, -GameManager.HEIGHT, 0f);

        // Creates an invisible ceiling at the top of the map so the player cannot escape.
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.transform.parent = groundTransform;
        ceiling.name = "ceiling" + GameManager.cpt;
        GameManager.cpt++;
        ceiling.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ceiling.GetComponent<Renderer>().enabled = false;
        ceiling.transform.localScale = new Vector3(GameManager.WIDTH * 2, 1, 1);
        ceiling.transform.position = new Vector3(0, GameManager.HEIGHT, 0f);
    
        // Creates an invisible wall on the left hand side of the map so that the player may never leave.
        GameObject wallLeft=GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallLeft.transform.parent = groundTransform;
        wallLeft.name = "WallLeft";
        wallLeft.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        wallLeft.GetComponent<Renderer>().enabled = false;
        wallLeft.transform.localScale = new Vector3(1, 1000, 1);
        wallLeft.transform.position = new Vector3(-GameManager.WIDTH - 1, 0, 0);
        wallLeft.GetComponent<BoxCollider>().material = GameObject.Find("block_grass").gameObject.GetComponent<BoxCollider>().material;

        // Ditto but right hand side
        GameObject wallRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wallRight.transform.parent = groundTransform;
        wallRight.name = "WallRight";
        wallRight.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        wallRight.GetComponent<Renderer>().enabled = false;
        wallRight.transform.localScale = new Vector3(1, 1000, 1);
        wallRight.transform.position = new Vector3(GameManager.WIDTH, 0, 0);
        wallRight.GetComponent<BoxCollider>().material = GameObject.Find("block_grass").gameObject.GetComponent<BoxCollider>().material;
    }

    /// <summary>
    /// Returns a unique random number in a range, uses <see cref="UnityEngine.Random.Range(int, int)"/>
    /// </summary>
    /// <param name="min">The minimum, inclusive</param>
    /// <param name="max">The maximum, inclusive</param>
    /// <returns></returns>
    int GetRandomNumber(int min, int max)
    {
        int value = Random.Range(min, max);

        // Annoying way to stop trees from spawning in one another
        while (treesX.Contains(value) ||
            treesX.Contains(value + 1) ||
            treesX.Contains(value + 2) ||
            treesX.Contains(value + 3) ||
            treesX.Contains(value + 4) ||
            treesX.Contains(value + 5) ||
            treesX.Contains(value - 1) ||
            treesX.Contains(value - 2) ||
            treesX.Contains(value - 3) ||
            treesX.Contains(value - 4) ||
            treesX.Contains(value - 5))
        {
            value = Random.Range(min, max);
        }

        return value;
    }

}
