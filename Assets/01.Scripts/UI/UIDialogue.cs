using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : UIBase
{
    private Transform _face;
    private Transform _dialogue;
    private Transform _scroll;
    private Transform _content;
    #region UI
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _sentenceText;
    #endregion

    private Image _image;

    private string _currentScenarioName;
    private TextAsset _currentScenarioTextAsset;
    private List<ScenarioData> _scenarioDatas = new List<ScenarioData>();

    private Coroutine _coroutine;
    private string _curSentence = "";
    private bool _isTextLine = false;
    private bool _isChoise = false;


    private int _currentScenarioLine = 0;
    public override void Init()
    {
        _parent = GameObject.Find("UIDialogue");

        _face = _parent.transform.Find("Face");
        _dialogue = _parent.transform.Find("Dialogue");
        _scroll = _parent.transform.Find("Scroll");
        _content = _scroll.transform.Find("Viewport/Content");
        _content.gameObject.SetActive(false);

        _image = _face.transform.Find("Sprite").GetComponent<UnityEngine.UI.Image>();

        Debug.Log(_dialogue.gameObject.name);

        _nameText = _dialogue.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>();
        _sentenceText = _dialogue.transform.Find("Text/Sentence").GetComponent<TextMeshProUGUI>();

        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence));
    }

    public void StartScenario(TextAsset scenario)
    {
        _currentScenarioTextAsset = scenario;

        string[] line = scenario.text.Split('\n');
        line[0] = "";
        foreach (string s in line)
        {
            if (s == "") continue;
            string[] scenarioInfo = s.Split(",");

            ScenarioData newData = new ScenarioData();
            newData.id = int.Parse(scenarioInfo[0]);
            newData.name = scenarioInfo[1];
            newData.text = scenarioInfo[2];
            newData.spriteNum = int.Parse(scenarioInfo[3] == "" ? "0" : scenarioInfo[3]);
            newData.backgroundNum = int.Parse(scenarioInfo[4] == "" ? "0" : scenarioInfo[4]);
            newData.navigationNum = int.Parse(scenarioInfo[5] == "" ? "0" : scenarioInfo[5]);
            newData.easeNum = int.Parse(scenarioInfo[6] == "" ? "0" : scenarioInfo[6]);
            newData.interfaceNum = int.Parse(scenarioInfo[7] == "" ? "0" : scenarioInfo[7]);
            newData.choice = scenarioInfo[8];
            newData.next = int.Parse(scenarioInfo[9] == "" ? "0" : scenarioInfo[9]);

            _scenarioDatas.Add(newData);
        }

        NextStory();
        return;
    }
    public void NextStory()
    {
        if(_isTextLine)
        {
            _sentenceText.SetText(_curSentence);
            UIManager.Instance.StopCoroutine(_coroutine);
            _isTextLine = false;
            return;
        }
        if (_isChoise) return;
        if (_currentScenarioLine >= _scenarioDatas.Count) return;

        ScenarioData data = _scenarioDatas[_currentScenarioLine];
        _nameText.SetText(data.name);
        _curSentence = data.text;
        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence));
       

        Debug.Log($"spriteNum : {data.spriteNum}");
        _image.sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/Char/{data.spriteNum}");
        if (data.choice != "")
        {
            OnChoice(data.choice);
            _isChoise = true;
        }
        _currentScenarioLine++;

    }
    public void OnChoice(string choiceText)
    {
        string[] choices = choiceText.Split(":"); 

    }
    public IEnumerator WriteTextLine(string text)
    {
        if (text == null || text == "") yield break;

        _isTextLine =  true;

        char[] texts = text.ToCharArray();
        Debug.Log($"strings.Length : {texts.Length} : {texts[0]}");
        string sentence = "";
        for(int i =0; i < texts.Length; i++)
        {
            sentence += texts[i];
            _sentenceText.SetText(sentence);
            yield return new WaitForSeconds(0.05f);
        }

        _isTextLine = false;
        yield return null;
    }

}
