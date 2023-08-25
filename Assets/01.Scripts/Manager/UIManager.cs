using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TextAsset document;

    public UIDialogue UIDialogue_ = new UIDialogue();

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Mutipple UIManager instance");
        }
        Instance = this;
    }
    private void Start()
    {
        UIDialogue_.Init();

        UIDialogue_.StartScenario(document);
    }

    public void Next()
    {
        UIDialogue_.NextStory();
    }

}
