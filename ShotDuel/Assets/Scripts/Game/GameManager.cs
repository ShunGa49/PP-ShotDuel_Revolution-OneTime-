using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Duelist;
using Random = UnityEngine.Random;  // ランダムが曖昧

/// <summary>
/// ゲームの進行状況を管理する。
/// </summary>

#region 必須コンポーネントを追加
[RequireComponent(typeof(AudioSource))]
#endregion

public class GameManager : MonoBehaviour
{
    // 状態遷移
    # region GAME_STATE
    public enum GAME_STATE
    {
        InReady = 0,        // 操作確認
        Initialize,         // ゲーム画面準備・ゲーム初期化

        InJingleStart,      // ジングルの再生
        InSelect,           // 選択ターン
        InExecute,          // 行動ターン
        InClearShell,       // 弾薬を減らす

        GameEndCheck,       // どちらかHP0で終了
        GameEnd,
        //
        Suicide,            // 自殺
};
    #endregion

    // 勝敗
    #region WIN_OR_LOSS
    public enum WIN_OR_LOSS
    {
        Draw = 0,
        Win,
        Loss,
    };
    #endregion

    #region [SerializeField]
    [SerializeField] private ButtonManager buttonManager;
    [Header("フェード時間")]
    [SerializeField] private float fadeInTime =1.0f;
    [Header("ジングル")]
    [SerializeField] private AudioClip se_select;
    [Header("ゲームオブジェクト")]
    [SerializeField] private Fade fadePanel = null;     // FadePanel
    [SerializeField] private UIManager uiManager;       // UIManager
    [SerializeField] private Player player;
    [SerializeField] private Enemy enemy;
    [Header("選択")]
    [SerializeField] private float defaultSpeed = 1.0f;                             // ゲーム開始時のスピード
    [SerializeField] private float speedUpRatio = 0.1f;                             // 一度でどれくらい速度上昇するか
    [SerializeField] private float speedUpRatioLimit = 2.0f;                        // 最大でどれくらい速度上昇するか
    [SerializeField, Range(1, 99)] private int speedUpTurnInterval = 3;             // 短縮ターン間隔
    [SerializeField] private bool canReselect = true;                               // 再選択可能か
    [Space]
    [Header("Playerステータス")]
    [SerializeField, Range(1, 99)] private int playerHp_max = 3;          // 最大体力
    [SerializeField, Range(0, 99)] private int playerShield_max = 7;      // 最大シールド
    [SerializeField, Range(1, 5)]  private int playerSgMag_max = 5;       // 最大装弾数
    [Header("Enemyステータス")]
    [SerializeField, Range(1, 99)] private int enemyHp_max = 3;          // 最大体力
    [SerializeField, Range(0, 99)] private int enemyShield_max = 7;      // 最大シールド
    [SerializeField, Range(1, 5)] private int enemySgMag_max = 5;       // 最大装弾数
    [Space]
    [Header("連続ガード制限")]
    [SerializeField] private int canShieldContinuously = 10;                        // 連続ガード可能回数
    [Header("弾")]
    [SerializeField] private bool canShowEnemyShell = false;                        // 敵の弾種類が見れるかどうか
    [SerializeField] private int[] shellDamage = new int[4] { 1,0,2,-1 };        // シェルダメージ
    [SerializeField, Range(0f, 100f)] private float[] shellProbability = new float[4] { 80.0f, 10.0f, 10.0f, 0.0f };        // シェル排出確率
    [Space]
    #endregion

    private GameObject settingData;
    private SettingData settingDataComponent;

    private GAME_STATE gameState;           // GameState
    private WIN_OR_LOSS winOrLoss;          // 勝敗
    private AudioSource audioSource;

    private bool isGameStart = false;
    private bool isGameEnd = false;
    private int gameTurn = 0;
    private float playTime = 0f;

    // ジングル類
    private float selectTime = 0f;          // 選択時間
    private float selectTimer = 0f;         // カウントダウンタイマー

    private bool isExeEnd=false;                    // 行動終了フラグ



    #region 読み取り専用専用変数
    // 状態
    public GAME_STATE GameState { get => gameState; }
    public WIN_OR_LOSS WinOrLoss { get => winOrLoss; }
    public bool IsGameStart { get => isGameStart; }
    public bool IsGameEnd { get => isGameEnd; }
    // プレイ中加算
    public int GameTurn { get => gameTurn; }
    public float PlayTime { get => playTime; }
    // 選択
    public float SelectTimer { get => selectTimer; }
    public float SelectTime { get => selectTime; }
    // 再選択可能
    public bool CanReselect { get => canReselect; }
    // ステータス
    public int PlayerHp_max { get => playerHp_max; }
    public int PlayerShield_max { get => playerShield_max; }
    public int PlayerSgMag_max { get => playerSgMag_max; }
    public int EnemyHp_max { get => enemyHp_max; }
    public int EnemyShield_max { get => enemyShield_max; }
    public int EnemySgMag_max { get => enemySgMag_max; }
    // 連続ガード
    public int CanShieldContinuously { get => canShieldContinuously; }
    // シェル
    public int[] ShellDamage { get => shellDamage; }
    public bool CanShowEnemyShell { get => canShowEnemyShell; }
    #endregion

    private void Awake()
    {
        settingData = GameObject.Find("SettingData");
        settingDataComponent = settingData.GetComponent<SettingData>();
        // SettingDataの数値で上書き
        GameManagerReset();
    }

    /// <summary>
    ///  Start()
    /// </summary>
#region start()
    void Start()
    {


        if (shellDamage.Length != shellProbability.Length)
        {
            Debug.LogWarning("shellDamage.Length と shellProbability.Length の要素数が一致しませんわ！");
        }
        audioSource = GetComponent<AudioSource>();

        player.AnimationPlayCV(ANIM_CV.HELLO);
        enemy.AnimationPlayCV(ANIM_CV.HELLO);

        // 初期GameStateを設定
        gameState = GAME_STATE.InReady;
        // タイムスケールの初期化
        Time.timeScale = defaultSpeed;
        // 選択時間の初期化
        selectTime = se_select.length;
        // ジングルの音の長さにする
        selectTime = se_select.length;

        // ステータスの更新
        uiManager.DuelistStatusUI();
    }
#endregion

    /// <summary>
    /// Update()　ゲームステート進行を管理する。
    /// </summary>
#region Update()
    void Update()
    {
        // 画面上側のUI
        uiManager.UpperSideUI();

        // プレイ時間を加算
        if (isGameStart)
        {
            playTime += Time.unscaledDeltaTime; // (Time.timeScaleの影響を受けない)
        }
        else if(isGameEnd)
        {
            uiManager.SwitchUI();
        }

        //	GameState処理
        switch (gameState)
        {
            // 操作確認等
            case GAME_STATE.InReady:
                if (GameReady()) // 関数を実行したら、戻り値trueで処理へ
                {
                    gameState = GAME_STATE.Initialize;
                }
                break;

            //	ゲーム画面準備・ゲーム初期化
            case GAME_STATE.Initialize:
                if (GameInitialize()) 
                {
                    gameState = GAME_STATE.InJingleStart;
                }
                break;
            //	ジングルが鳴る
            case GAME_STATE.InJingleStart:
                if (JingleStart())
                {
                    gameState = GAME_STATE.InSelect;
                }
                break;
            // 選択ターン
            case GAME_STATE.InSelect:
                if (SelectTurn())
                {
                    gameState = GAME_STATE.InExecute;
                }
                break;
            // 行動ターン
            case GAME_STATE.InExecute:
                if (ExecuteTurn())
                {
                    gameState = GAME_STATE.InClearShell;
                }
                break;
            // 弾薬を消去
            case GAME_STATE.InClearShell:
                if (ClearShell())
                {
                    gameState = GAME_STATE.GameEndCheck;
                }
                break;
            // ゲーム終了条件確認
            case GAME_STATE.GameEndCheck:
                GameEndCheck();
                break;

            // 自殺
            case GAME_STATE.Suicide:
                Suicide();
                break;
                
        }
    }
#endregion

    /// <summary>
    ///【各GameState処理】
    /// </summary>

    // ゲーム開始確認
#region GameReady()
    bool GameReady()
    {
        // ボタンを無効化
        buttonManager.AllToggteReset(false);

        Action on_completed = () =>
        {
            // ゲーム開始
            isGameStart = true;
        };
        fadePanel.FadeIn(fadeInTime, on_completed);

        if(isGameStart)
        {
            return true;
        }
        return false;
    }
#endregion

    //ゲームの初期化
#region GameInitialize()
    bool GameInitialize()
    {
        // ターン数を加算
        gameTurn++;
        // ボタンを有効化
        buttonManager.AllToggteReset(true);

        // 選択時間短縮
        if (gameTurn % speedUpTurnInterval == 0 && Time.timeScale <= speedUpRatioLimit)
        {
            Time.timeScale += speedUpRatio;
        }

        

        // 選択時間をリセット
        selectTimer = selectTime;
        // Player/Enemyの出し手を初期化
        player.Command = COMMAND.NONE;
        enemy.Command = COMMAND.RANDOM;
        // Player/Enemyのランダム選択可能コマンドを初期化
        player.CanCommandClear();
        enemy.CanCommandClear();
        // Player/Enemyのアニメーションを初期化
        player.SetAnim(ANIM.IDLE);
        enemy.SetAnim(ANIM.IDLE);
        // "false"に
        player.IsAnimationEnd = false;
        enemy.IsAnimationEnd = false;
        player.IsHitShield = false;
        enemy.IsHitShield = false;
        isExeEnd = false;

        return true;
    }
#endregion

    // ジングル再生開始
#region JingleStart()
    bool JingleStart()
    {
        // ジングルを1度だけ鳴らす
        //audioSource.PlayOneShot(se_select);

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        audioSource.clip = se_select;
        audioSource.Play();
        return true;
    }
#endregion

    // 選択ターン
#region SelectTurn()
    bool SelectTurn()
    {
        // カウントダウン開始
        selectTimer -= Time.deltaTime;
        // スライダー表示
        uiManager.SelectTimeSlider();

        // カウントダウン終了
        if (selectTimer <= 0f)
        {
            // ボタンを無効化
            buttonManager.AllToggteReset(false);
            // 次のゲームステートへ
            return true;
        }
        return false;
    }
#endregion

    // 行動ターン
#region ExecuteTurn()
    bool ExecuteTurn()
    {
        if (!isExeEnd)
        {
            // ランダム行動の場合
            if (player.Command == COMMAND.RANDOM)
            {
                player.RandomCommand(player,enemy);
            }
            if (enemy.Command == COMMAND.RANDOM)
            {
                enemy.RandomCommand(enemy,player);
            }
            // 判定
            Exe(player, enemy);
            Exe(enemy, player);
            isExeEnd = true;
        }
        // 両者のアニメーションを終了したら次ステートへ
        if (player.IsAnimationEnd && enemy.IsAnimationEnd)
        {
            return true;
        }
        return false;
    }
    // 処理を実行
    #region Exe()
    void Exe(Duelist me,Duelist you)
    {
        switch (me.Command)
        {
            case COMMAND.NONE:
                me.SetAnim(ANIM.NONE);
                me.NumberOfShield = 0;                        // シールド実行回数を初期化
                me.NoneCount++;
                if (you.Command == COMMAND.SHOT)
                {
                    me.Hp_current -= you.SgMag[0];                 // ダメージを受ける（HP）
                    me.IsDamaged = true;                          // 被弾フラグ
                }
                break;

            case COMMAND.SHOT:
                me.SetAnim(ANIM.SHOT);
                me.ShotCount++;
                me.NumberOfShield = 0;                        // シールド実行回数を初期化
                // 相撃ち
                if (you.Command == COMMAND.SHOT)
                {
                    me.Hp_current -= you.SgMag[0];                 // ダメージを受ける（HP）
                    me.IsDamaged = true;                          // 被弾フラグ
                }
                break;
            case COMMAND.RELOAD:
                me.SetAnim(ANIM.RELOAD);
                me.ReloadCount++;
                me.NumberOfShield = 0;                        // シールド実行回数を初期化
                if (you.Command != COMMAND.SHOT)
                {
                    // チューブ装填
                    if (me.SgMag.Count > 0)
                    {
                        me.SgMag.Insert(1, RandomAddShell());    // (2発目、弾の種類)
                    }
                    // チャンバー装填
                    else
                    {
                        me.SgMag.Insert(0, RandomAddShell());   // (1発目、弾の種類)
                    }
                }
                else
                {
                    me.Hp_current -= you.SgMag[0];                 // ダメージを受ける（HP）
                    me.IsDamaged = true;                           // 被弾フラグ
                }
                break;
            case COMMAND.EJECTED:
                me.EjectedCount++;
                me.SetAnim(ANIM.EJECTED);
                me.NumberOfShield = 0;                        // シールド実行回数を初期化
                if (you.Command == COMMAND.SHOT)
                {
                    me.Hp_current -= you.SgMag[0];                 // ダメージを受ける（HP）
                    me.IsDamaged = true;                           // 被弾フラグ
                }
                break;
            case COMMAND.SHIELD:
                me.ShieldCount++;
                me.SetAnim(ANIM.SHIELD);
                me.NumberOfShield++;                          // シールド実行回数追加
                if (you.Command == COMMAND.SHOT)
                {
                    if (me.Shield_current - you.SgMag[0] >= 0)
                    {
                        me.Shield_current -= you.SgMag[0];         // ダメージを受ける（シールド）
                        me.IsHitShield = true;
                    }
                    // ダメージがシールド以上
                    else
                    {
                        me.Shield_current = 0;
                        me.Hp_current -= (Mathf.Abs(me.Shield_current - you.SgMag[0])); // ダメージを受ける（HP）
                        me.IsDamaged = true;                       // 被弾フラグ
                    }
                }
                break;
        }

        if (me.IsDamaged)
        {
            // 一度でもダメージを受けたらfalse
            me.NoDamage =false;
        }

    }
    #endregion
    // ランダムな種類の弾を追加
    #region RandomAddShell()
    public int RandomAddShell()
    {
        // 合計確率を計算
        float total = 0.0f;
        for (int i = 0; i < shellProbability.Length; i++)
        {
            total += shellProbability[i];
        }
        // ランダム値を生成
        float rand = Random.Range(0.0f, total);

        // 確率を順番に足し合わせていった合計（累積確率）
        float cumulative = 0.0f;

        for (int i = 0; i < shellProbability.Length; i++)
        {
            cumulative += shellProbability[i];

            if (rand < cumulative)
            {
                // 値を返す
                return shellDamage[i];
            }
        }
        return 0;
    }
    #endregion
#endregion

    // 消費した弾薬を減らす
#region ClearShell()
    bool ClearShell()
    {
        // 弾薬を減らす
        if (player.Command == COMMAND.SHOT || player.Command == COMMAND.EJECTED)
        {
            player.SgMag.RemoveAt(0);                       // 0番目の要素を削除する（チェンバー弾）
        }
        if (enemy.Command == COMMAND.SHOT || enemy.Command == COMMAND.EJECTED)
        {
            enemy.SgMag.RemoveAt(0);                        // 0番目の要素を削除する（チェンバー弾）
        }
        // ショットシェルの表示
        uiManager.DrawShotShellsCall();
        // ステータスの更新
        uiManager.DuelistStatusUI();

        return true;
    }
#endregion

    // ゲームの勝敗を判定
#region GameEndCheck()
    void GameEndCheck()
    {
        // 両者死亡
        if (player.Hp_current <= 0 && enemy.Hp_current <= 0)
        {
            winOrLoss = WIN_OR_LOSS.Draw;
            isGameStart = false;
            isGameEnd = true;
            gameState = GAME_STATE.GameEnd;
        }
        // Playerのみ死亡（Enemy生存）
        else if (player.Hp_current <= 0 && enemy.Hp_current > 0)
        {
            // 勝利アニメーション（Enemy）
            if (!player.IsSuicide)
            {
                enemy.SetAnim(ANIM.WIN);
            }
            
            winOrLoss = WIN_OR_LOSS.Loss;
            isGameStart = false;
            isGameEnd = true;
            gameState = GAME_STATE.GameEnd;
        }
        // Enemyのみ死亡（Playerが生存）
        else if (player.Hp_current > 0 && enemy.Hp_current <= 0)
        {
            // 勝利アニメーション（Player）
            player.SetAnim(ANIM.WIN);
            
            winOrLoss = WIN_OR_LOSS.Win;
            isGameStart = false;
            isGameEnd = true;
            gameState = GAME_STATE.GameEnd;
        }
        // 両者生存（ゲーム続行）
        else
        {
            gameState = GAME_STATE.Initialize;
        }
    }
#endregion



    // 棄権・自決
#region PlayerSuicide
    public void Suicide()
    {
        gameState = GAME_STATE.Suicide;

        isGameStart = false;
        // 自害
        player.SetAnim(ANIM.SUICIDE);
        enemy.SetAnim(ANIM.SUICIDE);

        if (player.IsSuicide)
        {
            player.NoDamage = false;
            isGameEnd = true;
            // ステータスの更新
            uiManager.DuelistStatusUI();

            //敵の勝利
            gameState = GAME_STATE.GameEndCheck;
        }
    }
    #endregion

    // GameManagerの値をリセット
#region
    void GameManagerReset()
    {
        defaultSpeed = settingDataComponent.DefaultSpeed;
        speedUpRatio = settingDataComponent.SpeedUpRatio;
        speedUpRatioLimit = settingDataComponent.SpeedUpRatioLimit;
        speedUpTurnInterval = settingDataComponent.SpeedUpTurnInterval;

        canReselect = settingDataComponent.CanReselect;
        canShowEnemyShell = settingDataComponent.CanShowEnemyShell;
        canShieldContinuously = settingDataComponent.CanShieldContinuously;

        shellDamage = settingDataComponent.ShellDamage;
        shellProbability = settingDataComponent.ShellProbability;

        playerHp_max = settingDataComponent.PlayerHp_max;
        playerShield_max = settingDataComponent.PlayerShield_max;
        playerSgMag_max = settingDataComponent.PlayerSgMag_max;

        enemyHp_max = settingDataComponent.EnemyHp_max;
        enemyShield_max = settingDataComponent.EnemyShield_max;
        enemySgMag_max = settingDataComponent.EnemySgMag_max;
    }
#endregion
}

