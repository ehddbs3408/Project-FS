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
    
    public List<ScenarioData> scenarioDatas = new List<ScenarioData>();

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
        StartScenario("");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A)) { LoadData(); }
    }

    public void StartScenario(string scenarioName)
    {
        string[] line = _dialogue.text.Split('\n');
        line[0] = "";
        foreach(string s in line)
        {
            if (s == "") continue;
            string[] scenario = s.Split(",");

            ScenarioData newData = new ScenarioData();
            newData.id = int.Parse(scenario[0]);
            newData.name = scenario[1];
            newData.text = scenario[2];
            newData.spriteNum = scenario[3];
            newData.backgroundNum = int.Parse(scenario[4] == "" ? "0" : scenario[4]);
            newData.navigationNum = int.Parse(scenario[5] == "" ? "0" : scenario[5]);
            newData.easeNum = int.Parse(scenario[6] == "" ? "0" : scenario[6]);
            newData.interfaceNum = int.Parse(scenario[7] == "" ? "0" : scenario[7]);
            newData.choice = int.Parse(scenario[8] == "" ? "0" : scenario[8]);
            newData.next = int.Parse(scenario[9] == "" ? "0" : scenario[9]);

            scenarioDatas.Add(newData);
        }
    }

    public void LoadData()
    {
        foreach(ScenarioData data in scenarioDatas)
        {
            Debug.Log($"{data.id} {data.name} {data.text} {data.spriteNum} {data.backgroundNum} {data.navigationNum} {data.easeNum} {data.interfaceNum} {data.choice} {data.next} ");
        }
    }
}
