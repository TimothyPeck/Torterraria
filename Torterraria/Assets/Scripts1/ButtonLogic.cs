using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonLogic : MonoBehaviour
{
    public Button buttonPlay;
    public Button buttonAbout;
    public Button buttonReturn;
    public Button buttonQuit;

    public GameObject square;

    // Start is called before the first frame update
    void Start()
    {
        square.SetActive(false);
    }

    /// <summary>
    /// Plays the game scene
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    /// <summary>
    /// Displays credits
    /// </summary>
    public void ShowAbout()
    {
        square.SetActive(true);
    }

    /// <summary>
    /// Quits credits
    /// </summary>
    public void QuitAbout()
    {
        square.SetActive(false);
    }

    /// <summary>
    /// Quit game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
