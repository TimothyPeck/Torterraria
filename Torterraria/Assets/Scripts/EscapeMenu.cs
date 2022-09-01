using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public GameObject menu;

    public Button buttonResume;
    public Button buttonRestart;
    public Button buttonQuit;

    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("escape"))
        {
            isActive = !isActive;
            menu.SetActive(isActive);
        }
    }

    public void Resume()
    {
        isActive = false;
        menu.SetActive(isActive);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
