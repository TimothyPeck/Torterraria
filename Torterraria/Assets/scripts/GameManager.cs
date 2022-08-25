using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const int WIDTH = 100;
    public const int HEIGHT = 200;
    public GameObject dirtType = null;
    public GameObject stoneType = null;
    public static int cpt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
        //CreateGround();
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
            if (lastClicked.tag == "Ground" && mouseButton == 0)
            {
                GameObject.Destroy(lastClicked);
            }
            if (mouseButton == 1)
            {
                Vector3 mouse = Input.mousePosition;
                Ray castPoint = Camera.main.ScreenPointToRay(mouse);
                RaycastHit hit;
                if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
                {
                    GameObject cube;
                    Vector3 hitVector = hit.point;
                    if (hitVector.y >= -150)
                    {
                        cube = GameObject.Instantiate(dirtType);
                        cube.name = "dirt" + cpt;
                    }
                    else
                    {
                        cube = GameObject.Instantiate(stoneType);
                        cube.name = "stone" + cpt;
                    }
                    cpt++;
                    cube.tag = "Ground";
                    cube.transform.localScale = new Vector3(1, 1, 1);
                    cube.transform.parent = GameObject.Find("Ground").transform;
                    hitVector.x=Mathf.RoundToInt(hitVector.x);
                    hitVector.y = Mathf.RoundToInt(hitVector.y) + 1;
                    hitVector.z = 0;
                    cube.transform.position = hitVector;
                }
            }
        }
        clickableObj.ResetLastClicked();
        clickableObj.ResetMouseButton();
    }
}
