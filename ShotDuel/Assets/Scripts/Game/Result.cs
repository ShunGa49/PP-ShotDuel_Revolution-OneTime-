using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class Result : MonoBehaviour
{
    [Header("取得")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;
    [SerializeField] private GameObject inResult;
    [SerializeField] private GameObject winOrLoss;
    [SerializeField] private GameObject[] resultText;
    [SerializeField] private GameObject loadSceneButtons;
    [Header("設定")]
    [SerializeField] private string[] resultString = new string[3] { "勝者なし", "圧倒的勝利", "惜敗" };
    [SerializeField] private string noDamageString = "ノーダメージで勝利 素晴らしい";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inResult.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.IsGameStart && gameManager.IsGameEnd) 
        {
            inResult.SetActive(true);
            ShowResult();
            ShowScore();
        }
    }

    /// <summary>
    /// アニメーター上で呼び出し
    /// </summary>

    void ShowResult()
    {
        winOrLoss.GetComponent<Animator>().SetBool("IsDraw", true);
        Text resultTxt = winOrLoss.GetComponent<Text>();
        switch (gameManager.WinOrLoss)
        {
            case WIN_OR_LOSS.Draw:
                resultTxt.text = resultString[(int)WIN_OR_LOSS.Draw];
                break;
            case WIN_OR_LOSS.Win:
                resultTxt.text = resultString[(int)WIN_OR_LOSS.Win];
                break;
            case WIN_OR_LOSS.Loss:
                resultTxt.text = resultString[(int)WIN_OR_LOSS.Loss];
                break;
        }
    }

    void ShowScore()
    {
        resultText[0].GetComponent<Text>().text = "TURN: " + gameManager.GameTurn + "　TIME:" + ((int)gameManager.PlayTime / 60).ToString("D2") + ":" + ((int)gameManager.PlayTime % 60).ToString("D2"); ;
        resultText[1].GetComponent<Text>().text = "SHOT: " + player.ShotCount + "　RELOAD: " + player.ReloadCount + "　EJECTED: " + player.EjectedCount + "　SHIELD: " + player.ShieldCount;
        if (player.NoDamage  && gameManager.WinOrLoss == WIN_OR_LOSS.Win)
        {
            resultText[2].GetComponent<Text>().text = noDamageString;
        }
        else
        {
            resultText[2].GetComponent<Text>().text = "";
        }

        for (int i = 0; i<resultText.Length; i++)
        {
            resultText[i].GetComponent<Animator>().SetBool("IsDraw", true);
        }

        loadSceneButtons.SetActive(true);
    }
}
