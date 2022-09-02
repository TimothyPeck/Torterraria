using UnityEngine;
using UnityEngine.UI;

public class ShowHelp : MonoBehaviour
{
    public Canvas helpCanvas = null;

    public Button buttonHelp;
    public Button buttonReturn;

    // Start is called before the first frame update
    void Start()
    {
        helpCanvas.enabled = false;
    }

    /// <summary>
    /// When button help or close is pressed
    /// </summary>
    public void NeedHelp()
    {
        helpCanvas.enabled = !helpCanvas.enabled; 
    }
}
