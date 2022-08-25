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
        // Gets the last clicked object and checks if it's part of the ground.
        // If the object is part of the ground and the mouse button used to click on it is the left one, then the block is destroyed.
        GameObject lastClicked = clickableObj.GetLastClicked();
        int mouseButton = clickableObj.GetMouseButton();
        if (lastClicked != null)
        {
            if (lastClicked.tag == "Ground" && mouseButton == 0 && Vector2.Distance(lastClicked.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < 6)
            {
                DropBlock(lastClicked);

                //GameObject.Destroy(lastClicked);
            }
            else if (mouseButton == 1 && lastClicked.name == "backPlane")
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    Vector3 hitVector = hit.point;
                    print(Vector2.Distance(hitVector, GameObject.FindGameObjectWithTag("Player").transform.position));
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
                        cube.tag = "Ground";
                        cube.transform.localScale = new Vector3(1, 1, 1);
                        cube.transform.parent = GameObject.Find("Ground").transform;
                        hitVector.x = Mathf.RoundToInt(hitVector.x);
                        hitVector.y = Mathf.RoundToInt(hitVector.y);
                        hitVector.z = 0;
                        cube.transform.position = hitVector;
                    }
                }
            }
        }
        clickableObj.ResetLastClicked();
        clickableObj.ResetMouseButton();
    }

    void DropBlock(GameObject clickedObject)
    {
        clickedObject.tag = "Item";
        clickedObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }
}
