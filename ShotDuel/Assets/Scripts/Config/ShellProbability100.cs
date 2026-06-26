using UnityEngine;
using UnityEngine.UI;

public class ShellProbability100 : MonoBehaviour
{
    [SerializeField] private int id = -1; // 0～
    [SerializeField] private Text shellProbabilityTxt;
    private GameObject settingData;
    private SettingData settingDataComponent;

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingData = GameObject.Find("SettingData");
        settingDataComponent = settingData.GetComponent<SettingData>();
    }

    // Update is called once per frame
    void Update()
    {
        float total = 0.0f;
        for (int i = 0; i < settingDataComponent.ShellProbability.Length; i++)
        {
            total += settingDataComponent.ShellProbability[i];
        }
        // パーセンテージ計算
        float percentage = (settingDataComponent.ShellProbability[id] / total) * 100f;

        if(total <= 0.0f || settingDataComponent.ShellProbability[id] <= 0)
        {
            // テキストに反映しない
            shellProbabilityTxt.text = "0%";
        }
        else
        {
            // テキストに反映
            shellProbabilityTxt.text = percentage.ToString("F1") + "%";
        }
        
    }
}
