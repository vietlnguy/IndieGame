using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class DialogueEntry
{
    public string name;
    public List<Line> lines;
    public bool pauseExecution = false;
}