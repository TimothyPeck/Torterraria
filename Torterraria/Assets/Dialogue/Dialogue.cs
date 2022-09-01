using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public List<string> sentences = new List<string>();
    public List<string> names = new List<string>();
    public List<int> times = new List<int>();

    /// <summary>
    /// Add a sentence to be displayed as dialogue, defaults to a 5 seond display time
    /// </summary>
    /// <param name="name">The name to be used as the speaker</param>
    /// <param name="sentence">The sentence to be displayed</param>
    public void AddSentence(string name, string sentence)
    {
        this.names.Add(name);
        this.sentences.Add(sentence);
        this.times.Add(5);
    }

    /// <summary>
    /// See <see cref="AddSentence(string, string)"/> but with variable time
    /// </summary>
    /// <param name="name"><see cref="AddSentence(string, string)"/></param>
    /// <param name="sentence"><see cref="AddSentence(string, string)"/></param>
    /// <param name="time">The time for which the message will be displayed, in seconds</param>
    public void AddSentence(string name, string sentence, int time)
    {
        this.names.Add(name);
        this.sentences.Add(sentence);
        this.times.Add(time);
    }

    /// <summary>
    /// Empties the dialogue thus removing the risk of duplicates.
    /// </summary>
    internal void empty()
    {
        sentences.Clear();
        names.Clear();
        times.Clear();
    }
}
