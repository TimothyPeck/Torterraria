/**
 * Dorian Tan and Timothy Peck
 * Rattus 2021
 * He-Arc P2 SP 2021
 */

// Reusing old scripts, Tim

using UnityEngine;

public class ClickableObj : MonoBehaviour
{
    private static GameObject lastClicked;

    private static int mouseButton = -1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseButton = 0;
            FindClickedObj();
        }
        if (Input.GetMouseButtonDown(1))
        {
            mouseButton = 1;
            FindClickedObj();
        }
    }

    void FindClickedObj()
    {
        RaycastHit hit;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            if (hit.transform.gameObject != null)
            {
                lastClicked = (hit.transform.gameObject);
            }
        }
    }

    public static GameObject GetLastClicked()
    {
        return lastClicked;
    }
    public static int GetMouseButton()
    {
        return mouseButton;
    }

    public static void ResetLastClicked()
    {
        lastClicked = null;
    }

    public static void ResetMouseButton()
    {
        mouseButton = -1;
    }
}