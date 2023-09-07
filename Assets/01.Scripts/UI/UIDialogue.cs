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

    private GameObject _choiceBoxTemp;

    private Image _backGroundImage;
    private Image _charImage;

    private string _currentScenarioName;
    private TextAsset _currentScenarioTextAsset;
    private Dictionary<int,ScenarioData> _scenarioData = new Dictionary<int,ScenarioData>();

    private Coroutine _coroutine;
    private string _curSentence = "";
    private bool _isTextLine = false;
    private bool _isChoise = false;


    private int _currentScenarioLine = 1;
    public override void Init()
    {
        _parent = GameObject.Find("UIDialogue");

        _face = _parent.transform.Find("Face");
        _dialogue = _parent.transform.Find("Dialogue");
        _scroll = _parent.transform.Find("Scroll");
        _content = _scroll.transform.Find("Viewport/Content");

        _backGroundImage = _parent.transform.Find("BackGround").GetComponent<UnityEngine.UI.Image>();
        _charImage = _face.transform.Find("Sprite").GetComponent<UnityEngine.UI.Image>();

        Debug.Log(_dialogue.gameObject.name);

        _nameText = _dialogue.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>();
        _sentenceText = _dialogue.transform.Find("Text/Sentence").GetComponent<TextMeshProUGUI>();

        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence,"0"));

        _choiceBoxTemp = GameManager.Instance.ResourceManager_.Load<GameObject>($"Prefab/ChoiceBox");
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
            newData.spriteNum = scenarioInfo[3];
            newData.backgroundNum = int.Parse(scenarioInfo[4] == "" ? "0" : scenarioInfo[4]);
            newData.navigationNum = int.Parse(scenarioInfo[5] == "" ? "0" : scenarioInfo[5]);
            newData.easeNum = int.Parse(scenarioInfo[6] == "" ? "0" : scenarioInfo[6]);
            newData.interfaceNum = int.Parse(scenarioInfo[7] == "" ? "0" : scenarioInfo[7]);
            newData.choice = scenarioInfo[8];
            newData.next = int.Parse(scenarioInfo[9] == "" ? "0" : scenarioInfo[9]);

            _scenarioData.Add(newData.id,newData);
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
        if (_currentScenarioLine > _scenarioData.Count) return;

       
        ScenarioData data = _scenarioData[_currentScenarioLine];
        _nameText.SetText(data.name);
        _curSentence = data.text;
        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence,data.choice));
        Debug.Log($"start TextLine {_currentScenarioLine}");

        _backGroundImage.sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/BackGround/{data.backgroundNum}");
        _charImage.sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/Char/{data.spriteNum}");

        if(data.next != 0)
        {
            Debug.Log($"Next TextLine {data.next}");
            _currentScenarioLine = data.next;
            return;
        }
        _currentScenarioLine++;

    }
    public void OnChoice(string choiceText)
    {
        _content.gameObject.SetActive(true);
        string[] choices = choiceText.Split(":"); 

        foreach (string choice in choices)
        {
            string[] ss = choice.Split("=");
            GameObject go = GameObject.Instantiate(_choiceBoxTemp);
            TextMeshProUGUI text = go.transform.Find("Text").GetComponent<TextMeshProUGUI>();
            text.SetText(ss[0]);

            Button button = go.AddComponent<Button>();
            button.onClick.AddListener(() => {
                int a = int.Parse(ss[1]);
                _currentScenarioLine = a;
                _isChoise = false;
                NextStory();
                
                DeletChoiceBox();
            });

            go.transform.SetParent(_content);
            go.SetActive(true);
        }
    }
    public void DeletChoiceBox()
    {
        for(int i = 0;i<_content.childCount;i++)
        {
            GameManager.Instance.ResourceManager_.Destroy(_content.GetChild(i).gameObject);
        }
    }
    public IEnumerator WriteTextLine(string text,string choiceData)
    {
        if (text == null || text == "") yield break;

        _isTextLine =  true;

        char[] texts = text.ToCharArray();
        //Debug.Log($"strings.Length : {texts.Length} : {texts[0]}");
        string sentence = "";
        for(int i =0; i < texts.Length; i++)
        {
            sentence += texts[i];
            _sentenceText.SetText(sentence);
            yield return new WaitForSeconds(0.05f);
        }

        if (choiceData != "0")
        {
            _isChoise = true;
            yield return new WaitForSeconds(0.8f);
            OnChoice(choiceData);
        }

        _isTextLine = false;
        yield return null;
    }

}
