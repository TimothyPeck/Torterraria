using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    public GameObject woodType = null;
    public GameObject stoneType = null;
    public GameObject ironType = null;
    public GameObject grassType = null;
    public GameObject dirtType = null;
    public GameObject enemy1Type = null;
    public GameObject enemy2Type = null;
    public GameObject enemy3Type = null;
    public GameObject enemy4Type = null;

    /// <summary>
    /// The platform type to be used
    /// </summary>
    public GameObject platformType = null;

    public GameObject canvas;

    private Dictionary<string, GameObject> RessourceTypes = new Dictionary<string, GameObject>();

    Dialogue dialogue = new Dialogue();

    // Start is called before the first frame update
    void Start()
    {
        // Adds the ressources type

        RessourceTypes.Add("wood", woodType);
        RessourceTypes.Add("iron", ironType);
        RessourceTypes.Add("stone", stoneType);
        RessourceTypes.Add("grass", grassType);
        RessourceTypes.Add("dirt", dirtType);
        RessourceTypes.Add("platform", platformType);
    }

    // Update is called once per frame
    void Update()
    {
        GameObject lastClicked = ClickableObj.GetLastClicked();

        int mouseButton = ClickableObj.GetMouseButton();

        //If the player has clicked an object
        if (lastClicked != null && canvas.GetComponent<Inventory>().CanvasObject.enabled == false)
        {
            if (lastClicked.name == "Boss" && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 8f)
            {
                int cpt = 0;

                foreach (var ressource in Inventory.ressourcesNameNumber)
                {
                    if (cpt == Inventory.selectedRessource)
                    {
                        if (ressource.Key == "legendarySword")
                        {
                            lastClicked.GetComponent<Enemy>().GettingAttacked(1);
                        }
                    }
                    cpt++;
                }
            }
            // Makes sure the shovel is selected to break super dirt
            if (lastClicked.tag == "SuperDirt")
            {
                if (Inventory.indexRessource == -1)
                {
                    dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere shovel won�t work here. Even worse with bare hands.", 5);
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
                int cpt = 0;

                foreach (var ressource in Inventory.ressourcesNameNumber)
                {
                    if (cpt == Inventory.selectedRessource)
                    {
                        if (ressource.Key == "legendaryShovel")
                        {
                            TerrainGeneration.filledPositions[(int)lastClicked.transform.position.x + GameManager.WIDTH][(int)lastClicked.transform.position.y + GameManager.HEIGHT] = false;

                            GameObject.Destroy(lastClicked.gameObject);
                        }
                        else
                        {
                            dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere shovel won�t work here. Even worse with bare hands.", 5);
                            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                        }
                    }
                    cpt++;
                }
            }
            // Makes sure the pickaxe is selected to break super stone
            else if (lastClicked.tag == "SuperStone")
            {
                if (Inventory.indexRessource == -1)
                {
                    dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere pickaxe won�t work here. Even worse with bare hands.", 5);
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
                int cpt = 0;

                foreach (var ressource in Inventory.ressourcesNameNumber)
                {
                    if (cpt == Inventory.selectedRessource)
                    {
                        if (ressource.Key == "legendaryPickaxe")
                        {
                            TerrainGeneration.filledPositions[(int)lastClicked.transform.position.x + GameManager.WIDTH][(int)lastClicked.transform.position.y + GameManager.HEIGHT] = false;

                            GameObject.Destroy(lastClicked.gameObject);
                        }
                        else
                        {
                            dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere pickaxe won�t work here. Even worse with bare hands.", 5);
                            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                        }
                    }
                    cpt++;
                }
            }
            // Makes sure the axe is selected to break super wood
            else if (lastClicked.tag == "SuperWood")
            {
                if (Inventory.indexRessource == -1)
                {
                    dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere axe won�t work here. Even worse with bare hands.", 5);
                    FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                }
                int cpt = 0;

                foreach (var ressource in Inventory.ressourcesNameNumber)
                {
                    if (cpt == Inventory.selectedRessource)
                    {
                        if (ressource.Key == "legendaryAxe")
                        {
                            TerrainGeneration.filledPositions[(int)lastClicked.transform.position.x + GameManager.WIDTH][(int)lastClicked.transform.position.y + GameManager.HEIGHT] = false;

                            GameObject.Destroy(lastClicked.gameObject);
                        }
                        else
                        {
                            dialogue.AddSentence(GameManager.PLAYER_NAME, "Looks like a mere axe won�t work here. Even worse with bare hands.", 5);
                            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
                        }
                    }
                    cpt++;
                }
            }
            // Gets the last clicked object and checks if it's tagged as Ground.
            // If the object is part of the ground and the mouse button used to click on it is the left one, then the block is destroyed.
            // Also checks if the object is close enough to be broken.
            else if (lastClicked.tag == "Ground" && mouseButton == 0 && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 8)
            {
                // Puts sound effects according to the action

                switch (lastClicked.tag)
                {
                    case "SuperStone":
                        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.mine);
                        break;
                    default:
                        break;
                }

                string[] name = lastClicked.name.Split("_");

                switch (name[0])
                {
                    case "dirt":
                    case "grass":
                        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.dig);
                        break;
                    case "iron":
                    case "stone":
                        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.mine);
                        break;
                    case "wood":
                    case "plank":
                    case "platform":
                        SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.cut);
                        break;
                    default:
                        break;
                }

                TerrainGeneration.filledPositions[(int)lastClicked.transform.position.x + GameManager.WIDTH][(int)lastClicked.transform.position.y + GameManager.HEIGHT] = false;

                DropBlock(lastClicked);
            }
            //Checks if the click object is an enemy, if yes deal damage.
            else if (lastClicked.name != "Boss" && lastClicked.tag == "Enemy" && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 5 && mouseButton == 0)
            {
                lastClicked.GetComponent<Enemy>().GettingAttacked(1);
            }

            // Checks that the clicked object is the back wall, places a block if the right mouse button is clicked.
            else if (mouseButton == 1 && lastClicked.name == "backPlane")
            {
                //Gets the position of the mouse click
                Vector3 mouse = Input.mousePosition;

                //Makes the screen point the direction for the ray
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);

                //If the ray hits something, this will have the position of the hit
                RaycastHit hit;

                //Sends the ray and checks for a contact.
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    // The hit location transformed into a 3D vector.
                    Vector3 hitVector = hit.point;
                    int playerX = Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.x);
                    int playerY = Mathf.RoundToInt(GameObject.FindGameObjectWithTag("Player").transform.position.y);
                    bool playerPosIsBlockPos = (playerY == Mathf.RoundToInt(hitVector.y) || playerY + 1 == Mathf.RoundToInt(hitVector.y)) && playerX == Mathf.RoundToInt(hitVector.x);
                    //Checks that the point is within the confines of the world and that the player is close enough,
                    // also checks that the coordinates of the block haven't been filled already.
                    if (hit.point.x < GameManager.WIDTH &&
                        hit.point.x > -GameManager.WIDTH &&
                        hit.point.y < GameManager.HEIGHT &&
                        hit.point.y > -GameManager.HEIGHT &&
                        Vector2.Distance(hitVector, GameObject.FindGameObjectWithTag("Player").transform.position) < 6 &&
                        TerrainGeneration.filledPositions[(int)hit.point.x + GameManager.WIDTH][(int)hit.point.y + GameManager.HEIGHT] == false &&
                        !playerPosIsBlockPos)
                    {
                        GameObject cube = null;

                        int index = -1;
                        int cpt = 0;

                        bool isdropped = false;

                        string key = null;

                        foreach (var ressource in Inventory.ressourcesNameNumber)
                        {
                            if (cpt == Inventory.selectedRessource)
                            {
                                string currentKey = RessourceTypes.FirstOrDefault(x => x.Key == ressource.Key).Key;

                                if (currentKey != null && Inventory.ressourcesNameNumber[ressource.Key] >= 1)
                                {
                                    cube = Instantiate(RessourceTypes[currentKey]);
                                    cube.name = cube.name.Split("_")[1].Split("(")[0] + "_a";

                                    key = ressource.Key;

                                    isdropped = true;

                                    index = cpt;

                                    SfxManager.instance.GetComponent<AudioSource>().PlayOneShot(SfxManager.instance.drop);
                                }
                            }
                            cpt++;
                        }

                        if (isdropped)
                        {
                            Inventory.ressourcesNameNumber[key]--; // = index - 1;

                            GameObject childPlane = canvas.GetComponent<Inventory>().tabSelection[index].transform.Find("Plane").gameObject;

                            UnityEngine.UI.Image slot = childPlane.GetComponentInChildren<UnityEngine.UI.Image>();

                            TMP_Text text = slot.GetComponentInChildren(typeof(TMP_Text)) as TMP_Text;

                            text.text = Inventory.ressourcesNameNumber[key].ToString();

                            isdropped = false;

                            GameManager.cpt++;

                            //Adds the tag to make it breakable
                            cube.tag = "Ground";

                            //Just to be safe, set the scale
                            cube.transform.localScale = new Vector3(1, 1, 1);

                            //Make the ground object the parent, in line with the other cubes
                            cube.transform.parent = GameObject.Find("Ground").transform;

                            //Rounds the position so that all the blocks are aligned correctly
                            hitVector.x = Mathf.RoundToInt(hitVector.x);
                            hitVector.y = Mathf.RoundToInt(hitVector.y);
                            hitVector.y += key == "platform" ? 0.45f : 0;
                            hitVector.z = 0;

                            //Moves the block to the position
                            cube.transform.position = hitVector;
                        }
                    }
                }
            }
        }

        //Resets the last clicked GameObject so as to not loop on it
        ClickableObj.ResetLastClicked();

        //Resets the last used mouse button, not used
        ClickableObj.ResetMouseButton();

        dialogue.empty();
    }

    /// <summary>
    /// Makes the object a pickupable item.
    /// </summary>
    /// <param name="clickedObject">The object that has been clicked</param>
    void DropBlock(GameObject clickedObject)
    {
        // If it's a platform, take the parent gameObject
        if (clickedObject.name == "front" || clickedObject.name == "top" || clickedObject.name == "bottom")
        {
            clickedObject = clickedObject.transform.parent.gameObject;
        }

        clickedObject.transform.position = new Vector3(clickedObject.transform.position.x, Mathf.FloorToInt(clickedObject.transform.position.y), -0.2f);

        // Leaves cannot be obtained and therefor cannot be dropped
        if (!clickedObject.name.Contains("leaves"))
        {
            //Changes the tag of the block so as to not break it again
            clickedObject.tag = "Item";

            //Makes the block smaller -> now obtainable.
            clickedObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
        else
        {
            GameObject.Destroy(clickedObject);
        }
    }
}
