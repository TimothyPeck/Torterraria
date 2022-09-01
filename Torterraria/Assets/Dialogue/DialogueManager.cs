using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text diagText;
    public Animator animator;

    private Queue<string> dialogues;
    private Queue<string> diag_names;
    private Queue<int> diag_times;


    // Start is called before the first frame update
    void Start()
    {
        dialogues = new Queue<string>();
        diag_names = new Queue<string>();
        diag_times = new Queue<int>();
        animator.SetBool("IsOpen", false);
    }

    /// <summary>
    /// Begins the dialogue
    /// Extracts the dialogue names, texts, and times from the dialogue object into it's own queues, then calls DisplayDialogue()
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        
        string[] names = dialogue.names.ToArray();
        string[] sentences = dialogue.sentences.ToArray();
        int[] times = dialogue.times.ToArray();

        dialogues.Clear();
        diag_names.Clear();
        diag_times.Clear();

        foreach(string diag in sentences)
        {
            dialogues.Enqueue(diag);
        }

        foreach(string name in names)
        {
            diag_names.Enqueue(name);
        }

        foreach(int time in times)
        {
            diag_times.Enqueue(time);
        }

        DisplayDialogue();
    }

    /// <summary>
    /// Starts the coroutine that displays dialogue
    /// </summary>
    public void DisplayDialogue()
    {
        if (dialogues.Count == 0)
        {
            EndDialogue();
            return;
        }

        StartCoroutine(showMessageForSeconds());
    }

    /// <summary>
    /// Show a dialogue for the given time using Unity's built-in WaitForSecondsRealTime function
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator showMessageForSeconds()
    {
        string sentence = dialogues.Dequeue();
        string name = diag_names.Dequeue();
        int time = diag_times.Dequeue();
        //StartCoroutine(TypeSentence(sentence));
        nameText.text = name;
        diagText.text = sentence;
        yield return new WaitForSecondsRealtime(time);
        DisplayDialogue();
    }

    /// <summary>
    /// Types a sentence letter by letter, may cause problems with overlapping sentences or duplicating letters. 
    /// Use at your own risk.
    /// </summary>
    /// <param name="sentence">The sentence to be displayed</param>
    /// <returns>IEnumerator</returns>
    IEnumerator TypeSentence(string sentence)
    {
        diagText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            diagText.text += letter;
            yield return null;
        }
    }
    /// <summary>
    /// Hides the dialogue box
    /// </summary>
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
