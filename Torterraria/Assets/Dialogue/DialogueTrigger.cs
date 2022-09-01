using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private XmlDocument xmldoc;
    public void TriggerDialogue()
    {
        dialogue.AddSentence("Me", "I seem to have been trapped in here, I need to find a way out.");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
        dialogue.AddSentence("Mysterious villain", "Hello John, as you can see I have trapped you in this small room from which you will never escape.");
        dialogue.AddSentence("Mysterious villain", "In time you will understand why I have trapped you here, but for now I shall let you rot in the prison cell");
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
