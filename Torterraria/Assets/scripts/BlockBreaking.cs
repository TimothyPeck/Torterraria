using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBreaking : MonoBehaviour
{
    public GameObject dirtType = null;
    public GameObject stoneType = null;
    public GameObject ironType = null;
    public GameObject woodType = null;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
        GameObject lastClicked = clickableObj.GetLastClicked();
        int mouseButton = clickableObj.GetMouseButton();
        //If the player has clicked an object
        if (lastClicked != null)
        {
            // Gets the last clicked object and checks if it's part of the ground.
            // If the object is part of the ground and the mouse button used to click on it is the left one, then the block is destroyed.
            // Also checks if the object is close enough to be broken.
            if (lastClicked.tag == "Ground" && mouseButton == 0 && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 8)
            {
                DropBlock(lastClicked);

                //GameObject.Destroy(lastClicked);
            }
            //Checks if the click object is and enemy, if yes deal damage.
            else if (lastClicked.tag == "Enemy" && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 5 && mouseButton == 0)
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
                    //Checks that the point is within the confines of the world and that the player is close enough
                    if (hit.point.x < GameManager.WIDTH && hit.point.x > -GameManager.WIDTH && hit.point.y < GameManager.HEIGHT && hit.point.y > -GameManager.HEIGHT && Vector2.Distance(hitVector, GameObject.FindGameObjectWithTag("Player").transform.position) < 6)
                    {
                        GameObject cube;
                        cube = GameObject.Instantiate(woodType);
                        //if (inventory.selectedItem == BlockTypes.DIRT)
                        //{
                        //    cube = GameObject.Instantiate(dirtType);
                        //}
                        //else if (inventory.selectedItem == BlockTypes.STONE)
                        //{
                        //    cube = GameObject.Instantiate(stoneType);
                        //}else if (inventory.selectedItem == BlockTypes.WOOD)
                        //{
                        //    cube = GameObject.Instantiate(woodType);
                        //}
                        //else if (inventory.selectedItem == BlockTypes.IRON)
                        //{
                        //    cube = GameObject.Instantiate(ironType);
                        //}
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
                        hitVector.z = 0;
                        //Moves the block to the position
                        cube.transform.position = hitVector;
                    }
                }
            }
        }
        //Resets the last clicked GameObject so as to not loop on it
        clickableObj.ResetLastClicked();
        //Resets the last used mouse button, not used
        clickableObj.ResetMouseButton();
    }

    void DropBlock(GameObject clickedObject)
    {
        //Changes the tag of the block so as to not break it again
        clickedObject.tag = "Item";
        //Makes the block smaller -> now obtainable.
        clickedObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
