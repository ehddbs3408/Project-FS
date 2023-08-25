using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIDialogue : UIBase
{
    private Transform _face;
    private Transform _dialogue;
    private Transform _scroll;
    #region UI
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _sentenceText;
    #endregion

    private GameObject _choiceTemp;
    private GameObject _spriteTemp;

    private string _currentScenarioName;
    private TextAsset _currentScenarioTextAsset;
    private List<ScenarioData> _scenarioDatas = new List<ScenarioData>();

    private int _currentScenarioLine = 0;
    public override void Init()
    {
        _parent = GameObject.Find("UIDialogue");

        _face = _parent.transform.Find("Face");
        _dialogue = _parent.transform.Find("Dialogue");
        _scroll = _parent.transform.Find("Scroll");

        Debug.Log(_dialogue.gameObject.name);

        _nameText = _dialogue.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>();
        _sentenceText = _dialogue.transform.Find("Text/Sentence").GetComponent<TextMeshProUGUI>();
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
            newData.choice = int.Parse(scenarioInfo[8] == "" ? "0" : scenarioInfo[8]);
            newData.next = int.Parse(scenarioInfo[9] == "" ? "0" : scenarioInfo[9]);

            _scenarioDatas.Add(newData);
        }

        NextStory();
        return;
    }
    public void NextStory()
    {
        if (_currentScenarioLine >= _scenarioDatas.Count) return;

        ScenarioData data = _scenarioDatas[_currentScenarioLine];
        _nameText.SetText(data.name);
        UIManager.Instance.StartCoroutine(WriteTextLine(data.text));
        _currentScenarioLine++;
    }
    public IEnumerator WriteTextLine(string text)
    {

        char[] texts = text.ToCharArray();
        Debug.Log($"strings.Length : {texts.Length} : {texts[0]}");
        string sentence = "";
        for(int i =0; i < texts.Length; i++)
        {
            sentence += texts[i];
            _sentenceText.SetText(sentence);
            yield return new WaitForSeconds(0.7f);
        }

        yield return null;
    }
    public void TestLoadData()
    {
        foreach (ScenarioData data in _scenarioDatas)
        {
            Debug.Log($"{data.id} {data.name} {data.text} {data.spriteNum} {data.backgroundNum} {data.navigationNum} {data.easeNum} {data.interfaceNum} {data.choice} {data.next} ");
        }
    }
}
