using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHelp : MonoBehaviour
{
    public Canvas helpCanvas = null;

    private bool isShown = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !isShown)
        {
            isShown = true;
            helpCanvas.enabled = isShown;
        }
        else if (Input.GetKeyDown(KeyCode.F1) && isShown)
        {
            isShown = false;
            helpCanvas.enabled = isShown;
        }
    }
}
