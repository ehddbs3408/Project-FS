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

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.A))
        {
            UIDialogue_.FadeIn(1);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            UIDialogue_.FadeOut(1);
        }
    }

}
