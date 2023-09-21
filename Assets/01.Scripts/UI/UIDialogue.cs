using DG.Tweening;
using System;
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
    private Image _fade;
    private GameObject _charTemp;
    private Dictionary<int,Image> _sprites = new Dictionary<int,Image>();

    private string _currentScenarioName;
    private TextAsset _currentScenarioTextAsset;
    private Dictionary<int,ScenarioData> _scenarioData = new Dictionary<int,ScenarioData>();

    private Coroutine _coroutine;
    private string _curSentence = "";
    private string _curChoiceData = "";
    private bool _isStopDialogue = false;
    private bool _isTextLine = false;
    private bool _isChoise = false;
    private bool _isOnChoicePanel = false;


    private int _currentScenarioLine = 30;
    public override void Init()
    {
        _parent = GameObject.Find("UIDialogue");

        _face = _parent.transform.Find("Face");
        _dialogue = _parent.transform.Find("Dialogue");
        _scroll = _parent.transform.Find("Scroll");
        _content = _scroll.transform.Find("Viewport/Content");

        _backGroundImage = _parent.transform.Find("BackGround").GetComponent<UnityEngine.UI.Image>();
        _fade = _parent.transform.Find("Fade").GetComponent<UnityEngine.UI.Image>();

        _nameText = _dialogue.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>();
        _sentenceText = _dialogue.transform.Find("Text/Sentence").GetComponent<TextMeshProUGUI>();

        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence,"0"));

        _charTemp = GameManager.Instance.ResourceManager_.Load<GameObject>($"Prefab/Sprite");
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
            newData.sprite = scenarioInfo[3];
            newData.backgroundNum = int.Parse(scenarioInfo[4] == "" ? "0" : scenarioInfo[4]);
            newData.soundStr = scenarioInfo[5];
            newData.navigation = scenarioInfo[6];
            newData.effectNum = scenarioInfo[7];
            newData.interfaceNum = int.Parse(scenarioInfo[8] == "" ? "0" : scenarioInfo[8]);
            newData.choice = scenarioInfo[9];
            newData.next = int.Parse(scenarioInfo[10] == "" ? "0" : scenarioInfo[10]);

            _scenarioData.Add(newData.id,newData);
        }

        NextStory();
        return;
    }
    public void NextStory()
    {
        if (_isStopDialogue) return;
        if(_isTextLine)
        {
            _sentenceText.SetText(_curSentence);
            UIManager.Instance.StopCoroutine(_coroutine);
            _isTextLine = false;
            return;
        }
        if (_isChoise)
        {
            if(_isOnChoicePanel == false)
                OnChoice(_curChoiceData);
            return;
        }
        if (_currentScenarioLine > _scenarioData.Count) return;

       
        ScenarioData data = _scenarioData[_currentScenarioLine];
        _nameText.SetText(data.name);
        _curSentence = data.text;
        _curChoiceData = data.choice;
        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence,data.choice));

        _backGroundImage.sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/BackGround/{data.backgroundNum}");
        SetSprite(data.sprite, data.interfaceNum);
        Navigation(data.navigation, data.interfaceNum);

        SetEffect(data.effectNum,data.sprite,data.interfaceNum);
        if (data.next != 0)
        {
            _currentScenarioLine = data.next;
            return;
        }
        if(data.choice != "0")
        {
            _isChoise = true;
        }
        _currentScenarioLine++;

    }
    public void SetEffect(string effectName,string sprite,int interfaceNum)
    {
        string[] strs = effectName.Split(':');
        switch (strs[0])
        {
            case "0":
                break;
            case "1":
                FadeIn(float.Parse(strs[1]));
                break;
            case "2":
                FadeOut(float.Parse(strs[1]));
                break;
            case "3":
                DeletSprite(sprite, interfaceNum);
                break;
        }
    }
    public void SetSprite(string sprite,int interfaceNum)
    {
        if(_sprites.ContainsKey(interfaceNum) == false)
        {
            GameObject go = GameObject.Instantiate(_charTemp, _face);
            _sprites.Add(interfaceNum, go.transform.GetComponent<Image>());
        }
        _sprites[interfaceNum].sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/Char/{sprite}");
    }
    public void DeletSprite(string sprite,int interfaceNum)
    {
        if (_sprites.ContainsKey(interfaceNum) == false) return;

        GameObject go = _sprites[interfaceNum].transform.gameObject;
        _sprites.Remove(interfaceNum);
        GameObject.Destroy(go);
    }
    public void Navigation(string navigation, int interfaceNum)
    {
        if (navigation != "0")
        {
            string[] pos = navigation.Split('x');
            _sprites[interfaceNum].rectTransform.DOAnchorPos(new Vector2(int.Parse(pos[0]), int.Parse(pos[1])), float.Parse(pos[2]));
        }
    }
    public void OnChoice(string choiceText)
    {
        _isOnChoicePanel = true;
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
                _isOnChoicePanel = false;
                NextStory();
                
                DeletChoiceBox();
            });

            go.transform.SetParent(_content);
            go.SetActive(true);
        }
    }
    public void FadeIn(float duration)
    {
        _isStopDialogue = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(_fade.DOColor(new Color(0, 0, 0, 1), duration));
        seq.AppendCallback(() =>
        {
            _isStopDialogue = false;
            NextStory();
        });
    }
    public void FadeOut(float duration)
    {
        _isStopDialogue = true;
        Sequence seq = DOTween.Sequence();
        seq.Append(_fade.DOColor(new Color(0, 0, 0, 0), duration));
        seq.AppendCallback(() =>
        {
            _isStopDialogue = false;
            NextStory();
        });
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

        _isTextLine = false;
        yield return null;
    }

}
