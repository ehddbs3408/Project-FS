using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
public class ExelToDialogJson : EditorWindow
{
    #region Editor
    [MenuItem("Tools/ExcelToClassConverter")]
    public static void ShowWindow()
    {
        GetWindow<ExelToDialogJson>("ExcelToClassConverter");
    }

    private void OnGUI()
    {
        GUILayout.Label("ExcelToClass", EditorStyles.boldLabel);

        inputURL = EditorGUILayout.TextField("URL(Until \"Range=\"):", inputURL);
        rangeStart = EditorGUILayout.TextField("Excel Range Start:", rangeStart);
        rangeEnd = EditorGUILayout.TextField("Excel Range End:", rangeEnd);
        fileName = EditorGUILayout.TextField("File Name:", fileName);

        if (GUILayout.Button("Converter"))
        {

        }
    }
    #endregion


    #region ExcelToClass
    private string inputURL = "https://docs.google.com/spreadsheets/d/1gya2C8tkrLr5HymQ4eccOAdZdZ1fhmYBe2AaSXMTee0/export?format=tsv&range=";
    private string URL;

    string pth = "Assets/01.Scripts/Data";
    StreamWriter sw;

    public string rangeStart = "A1";
    public string rangeEnd = "B2";
    public string fileName = "WeaponData";
    private async void DownloadExcel()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        await www.SendWebRequest();
        SetItemSO(www.downloadHandler.text);
    }

    void SetItemSO(string tsv)
    {
        string[] row = tsv.Split(Environment.NewLine);
        int rowSize = row.Length;
        int columnSize = row[0].Split('\t').Length;

    }
    #endregion
}
#endif