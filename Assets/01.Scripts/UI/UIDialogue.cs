using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogue : UIBase
{
    private GameObject _face;
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
    private Dictionary<string,GameObject> _characters = new Dictionary<string, GameObject>();

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

    private Color _backSpriteColor = new Color(0.6f, 0.6f, 0.6f);


    private int _currentScenarioLine = 1;
    public override void Init()
    {
        _parent = GameObject.Find("UIDialogue");

        _face = GameObject.Find("Face");
        _dialogue = _parent.transform.Find("Dialogue");
        _scroll = _parent.transform.Find("Scroll");
        _content = _scroll.transform.Find("Viewport/Content");

        _backGroundImage = _parent.transform.Find("BackGround").GetComponent<UnityEngine.UI.Image>();
        _fade = _parent.transform.Find("Fade").GetComponent<Image>();

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

            int i = 0;
            ScenarioData newData = new ScenarioData();
            newData.id = int.Parse(scenarioInfo[i++]);
            newData.name = scenarioInfo[i++];
            newData.classStr = scenarioInfo[i++];
            newData.text = scenarioInfo[i++];
            newData.sprite = scenarioInfo[i++];
            newData.backgroundNum = int.Parse(scenarioInfo[i] == "" ? "0" : scenarioInfo[i]);
            i++;
            newData.soundStr = scenarioInfo[i++];
            newData.navigation = scenarioInfo[i++];
            newData.effectNum = scenarioInfo[i++];
            newData.interfaceNum = int.Parse(scenarioInfo[i] == "" ? "0" : scenarioInfo[i]);
            i++;
            newData.choice = scenarioInfo[i++];
            newData.next = int.Parse(scenarioInfo[i] == "" ? "0" : scenarioInfo[i]);

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
        _nameText.SetText($"{data.name}  <size=70%><#80ffff>{data.classStr}</color></size>");
        _curSentence = data.text;
        _curChoiceData = data.choice;
        _coroutine = UIManager.Instance.StartCoroutine(WriteTextLine(_curSentence,data.choice));

        _backGroundImage.sprite = GameManager.Instance.ResourceManager_.Load<Sprite>($"Image/BackGround/{data.backgroundNum}");
        SetSprite(data.sprite, data.interfaceNum);
        Navigation(data.navigation, data.sprite);

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
            case "4":
                UIManager.Instance.StartCoroutine(WaitDialogue(float.Parse(strs[1])));
                break;
        }
    }
    public IEnumerator WaitDialogue(float duration)
    {
        _isStopDialogue = true;
        yield return new WaitForSeconds(duration);
        _isStopDialogue = false;
    }
    public void SetSprite(string sprite,int interfaceNum)
    {
        //BackSpriteColor();
        if (_characters.ContainsKey(sprite) == false)
        {
            GameObject go = GameObject.Instantiate(GameManager.Instance.ResourceManager_.Load<GameObject>($"Prefab/Lieve2D/{sprite}"));
            _characters.Add(sprite, go);
        }
        _characters[sprite].SetActive(true);
    }
    public void DeletSprite(string sprite,int interfaceNum)
    {
        if (_characters.ContainsKey(sprite) == false) return;

        _characters[sprite].SetActive(false);
    }
    public void Navigation(string navigation, string sprite)
    {
        if (navigation != "0")
        {
            string[] pos = navigation.Split('x');
            _characters[sprite].transform.DOMove(new Vector2(float.Parse(pos[0]), float.Parse(pos[1])), float.Parse(pos[2]));
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
        
        text = text.Replace("\\n", "\n");
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
