using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class DialogueEntry
{
    public string name;
    public List<string> lines;
    public bool pauseExecution = false;
}