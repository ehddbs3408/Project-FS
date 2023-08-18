using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument document;

    public TextAsset _dialogue;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Mutipple UIManager instance");
        }
        Instance = this;
    }

    public void log(string message)
    {
        string[] data = _dialogue.text.Split('\n');
    }
}
