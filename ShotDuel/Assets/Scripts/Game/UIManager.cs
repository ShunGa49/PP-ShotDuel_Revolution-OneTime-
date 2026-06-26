using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class UIManager : MonoBehaviour
{
    #region [SerializeField]
    [Header("他オブジェクト取得")]
    [SerializeField] private GameManager gameManager;       // GameManager
    [SerializeField] private Player player;                 // Player
    [SerializeField] private Enemy enemy;                   // Enemy
    [Space]
    [Header("UI関係")]
    [Header("上部表示UI")]
    [SerializeField] private Text turnTxt;                  // ターン数
    [SerializeField] private Text gameStateTxt;             // ゲームの進行状況
    [SerializeField] private Text playTimeTxt;              // ゲームのプレイ時間
    [SerializeField] private Text timeScaleTxt;             // ゲームのタイムスケール
    [Header("SELECTターン時間")]
    [SerializeField] private Text selectTimerTxt;           // SELECTターン時間（Text）
    [SerializeField] private Slider selectTimerSlider;      // SELECTターン時間（Slider）
    [Space]
    [Header("プレイヤー")]
    [SerializeField] private Text playerMagTxt;             // 装填数
    [SerializeField] private Text playerHpTxt;              // 体力
    [SerializeField] private Text playerShieldTxt;          // シールド
    [SerializeField] private GameObject[] playerShotShels;  // プレイヤーのショットシェル[5]
    [SerializeField] private Text playerNumberOfShieldTxt;  // 連続ガード回数
    [Header("エネミー")]
    [SerializeField] private Text enemyMagTxt;              // 装填数
    [SerializeField] private Text enemyHpTxt;               // 体力
    [SerializeField] private Text enemyShieldTxt;           // シールド
    [SerializeField] private GameObject[] enemyShotShels;   // エネミーのショットシェル[5]
    [SerializeField] private Text enemyNumberOfShieldTxt;   // 連続ガード回数
    [Space]
    [Header("シェルの色")]
    [SerializeField] private Color shellSecretColor = new Color32(255, 255, 255, 255);
    [SerializeField] private Color[] shellColor
        = new Color[4] { new Color(255, 0, 0, 255), new Color(0, 255, 0, 255), new Color(0, 0, 0, 255), new Color(0, 0, 255, 255) };
    [Space]
    [Header("UIグループ")]
    [SerializeField] private GameObject inGame;
    [SerializeField] private GameObject inResult;
    #endregion

    // Start
    void Start()
    {
        if ( shellColor.Length < gameManager.ShellDamage.Length)
        {
            Debug.LogWarning("shellColor.Length が gameManager.ShellDamage.Length より少ないですわ！");
        }
    }

    // Update
    public void Update()
    {
    }

    public void SwitchUI()
    {
        inGame.SetActive(false);
        inResult.SetActive(true);
    }

    /// <summary>
    /// ゲーム中、常に実行
    /// </summary>
#region GameStateUI()
    public void UpperSideUI()
    {
        // ターン数表示
        turnTxt.text = "TURN " + gameManager.GameTurn.ToString("D2");

        // TurnName
        switch (gameManager.GameState)
        {
            #region ゲームステート名を表示
            case GAME_STATE.InReady:
                gameStateTxt.text = "Ready?";
                break;
            case GAME_STATE.Initialize:
                gameStateTxt.text = "Initialize…";
                break;
            case GAME_STATE.InSelect:
                gameStateTxt.text = "SelectTurn!";
                break;
            case GAME_STATE.InExecute:
                gameStateTxt.text = "Execute!";
                break;
            case GAME_STATE.GameEndCheck:
                gameStateTxt.text = "GameEndCheck";
                break;
                #endregion
        }

        // プレイタイム加算
        playTimeTxt.text =((int)gameManager.PlayTime / 60).ToString("D2") + ":" + ((int)gameManager.PlayTime % 60).ToString("D2");
        //
        timeScaleTxt.text = "x" + Time.timeScale.ToString("N2");
    }
#endregion

    /// <summary>
    /// デュエリストのステータス
    /// </summary>
#region
    public void DuelistStatusUI()
    {
        // Player
        playerMagTxt.text = ( player.SgMag.Count + "/" + player.SgMag_max ).ToString();
        playerHpTxt.text = player.Hp_current.ToString();
        playerShieldTxt.text = player.Shield_current.ToString();
        playerNumberOfShieldTxt.text = ("連続ガード：" + player.NumberOfShield + " / " + gameManager.CanShieldContinuously ).ToString();
        // Enemy
        enemyMagTxt.text = ( enemy.SgMag.Count.ToString() + "/" + enemy.SgMag_max ).ToString();
        enemyHpTxt.text = enemy.Hp_current.ToString();
        enemyShieldTxt.text = enemy.Shield_current.ToString();
        enemyNumberOfShieldTxt.text = ("連続ガード：" + enemy.NumberOfShield + " / " + gameManager.CanShieldContinuously ).ToString();
    }
#endregion

    // 選択時間のカウントダウン表示
#region SelectTimeSliderUI()
    public void SelectTimeSlider()
    {
        // スライダーの最大値を更新
        selectTimerSlider.minValue = 0.0f;
        selectTimerSlider.maxValue = gameManager.SelectTime;

        // スライダー
        selectTimerSlider.value = gameManager.SelectTime - gameManager.SelectTimer;

        // カウントダウン中
        if (gameManager.SelectTimer > 0f)
        {
            selectTimerTxt.text = gameManager.SelectTimer.ToString("N0");
        }
        // カウントダウン終了
        else
        {
            selectTimerTxt.text = "FIRE!";
        }
    }
#endregion

    /// <summary>
    /// ショットシェルを表示
    /// </summary>
#region
    public void DrawShotShellsCall()
    {
        // 処理を呼び出し
        DrawShotShells(player.SgMag, player.SgMag_max, playerShotShels, true);
        DrawShotShells(enemy.SgMag, enemy.SgMag_max, enemyShotShels, gameManager.CanShowEnemyShell);
    }
    #region 実際の処理
    public void DrawShotShells( List<int> sgMag, int sgMag_max, GameObject[] shotShellsUI , bool canShowEnemyShell)
    {
        // 追加（true）
        for (int i = 0; i < sgMag.Count; i++)
        {
            shotShellsUI[i].SetActive(true);

            bool found = false;

            if (canShowEnemyShell)  // 弾の種類を表示するか
            {
                shotShellsUI[i].GetComponentInChildren<Text>().text = sgMag[i].ToString();

                for (int j = 0; j < gameManager.ShellDamage.Length; j++)
                {
                    if (sgMag[i] == gameManager.ShellDamage[j])
                    {
                        shotShellsUI[i].GetComponent<Image>().color = shellColor[j];
                        found = true;
                        break;
                    }
                }
            }
            if (!found)
            {
                shotShellsUI[i].GetComponent<Image>().color = shellSecretColor;
            }

        }
        // 削除（false）
        for (int i = sgMag_max - 1; i >= sgMag.Count; i--)
        {
            shotShellsUI[i].SetActive(false);
        }
    }
    #endregion
#endregion
}
