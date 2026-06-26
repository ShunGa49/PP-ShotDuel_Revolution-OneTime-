using UnityEngine;

public class SettingData : MonoBehaviour
{
    [Header("ゲーム速度")]
    [SerializeField] private float defaultSpeed = 1.0f;                              // ゲーム開始時のスピード
    [SerializeField] private float speedUpRatio = 0.1f;                              // 一度でどれくらい速度上昇するか
    [SerializeField] private float speedUpRatioLimit = 2.0f;                         // 最大でどれくらい速度上昇するか
    [SerializeField] private int speedUpTurnInterval = 3;                            // 短縮ターン間隔
    [Header("bool")]
    [SerializeField] private bool canReselect = true;                                // 再選択可能か
    [SerializeField] private bool canShowEnemyShell = false;                         // 敵の弾種類が見れるかどうか
    [Header("共通")]
    [SerializeField] private int canShieldContinuously = 3;                          // 連続ガード可能回数
    [Header("シェル")]
    [SerializeField] private int[] shellDamage = new int[4] { 1, 0, 2, -1 };                         // シェルダメージ
    [SerializeField] private float[] shellProbability = new float[4] { 80.0f, 10.0f, 10.0f, 0.0f };  // シェル排出確率
    [Header("Player")]
    [SerializeField] private int playerHp_max = 3;           // 最大体力
    [SerializeField] private int playerShield_max = 7;       // 最大シールド
    [SerializeField] private int playerSgMag_max = 5;        // 最大装弾数
    [Header("Enemy")]
    [SerializeField] private int enemyHp_max = 3;            // 最大体力
    [SerializeField] private int enemyShield_max = 7;        // 最大シールド
    [SerializeField] private int enemySgMag_max = 5;         // 最大装弾数

    // Default設定
    #region
    // 選択時間
    private float defaultSpeed_Default;                              // ゲーム開始時のスピード
    private float speedUpRatio_Default;                              // 一度でどれくらい速度上昇するか
    private float speedUpRatioLimit_Default;                         // 最大でどれくらい速度上昇するか
    private int speedUpTurnInterval_Default;                         // 短縮ターン間隔
    // Bool
    private bool canReselect_Default;                                // 再選択可能か
    private bool canShowEnemyShell_Default;                          // 敵の弾種類が見れるかどうか
    // 共通
    private int canShieldContinuously_Default;                       // 連続ガード可能回数
    private int[] shellDamage_Default;
    private float[] shellProbability_Default;
    // Player
    private int playerHp_max_Default;                                // 最大体力
    private int playerShield_max_Default;                            // 最大シールド
    private int playerSgMag_max_Default;                             // 最大装弾数
    // Enemy
    private int enemyHp_max_Default;                                 // 最大体力
    private int enemyShield_max_Default;                             // 最大シールド
    private int enemySgMag_max_Default;                              // 最大装弾数
    #endregion

    //Get&Set
    #region
    public float DefaultSpeed
    {
        get { return defaultSpeed; }
        set { defaultSpeed = value; }
    }
    public float SpeedUpRatio
    {
        get { return speedUpRatio; }
        set { speedUpRatio = value; }
    }
    public float SpeedUpRatioLimit
    {
        get { return speedUpRatioLimit; }
        set { speedUpRatioLimit = value; }
    }
    public int SpeedUpTurnInterval
    {
        get { return speedUpTurnInterval; }
        set { speedUpTurnInterval = value; }
    }

    public bool CanReselect
    {
        get { return canReselect; }
        set { canReselect = value; }
    }
    public bool CanShowEnemyShell
    {
        get { return canShowEnemyShell; }
        set { canShowEnemyShell = value; }
    }
    
    
    public int CanShieldContinuously
    {
        get { return canShieldContinuously; }
        set { canShieldContinuously = value; }
    }

    public int[] ShellDamage
    {
        get { return shellDamage; }
        set { shellDamage = value; }
    }
    public float[] ShellProbability
    {
        get { return shellProbability; }
        set { shellProbability = value; }
    }

    public int PlayerHp_max
    {
        get { return playerHp_max; }
        set { playerHp_max = value; }
    }
    public int PlayerShield_max
    {
        get { return playerShield_max; }
        set { playerShield_max = value; }
    }
    public int PlayerSgMag_max
    {
        get { return playerSgMag_max; }
        set { playerSgMag_max = value; }
    }

    public int EnemyHp_max
    {
        get { return enemyHp_max; }
        set { enemyHp_max = value; }
    }
    public int EnemyShield_max
    {
        get { return enemyShield_max; }
        set { enemyShield_max = value; }
    }
    public int EnemySgMag_max
    {
        get { return enemySgMag_max; }
        set { enemySgMag_max = value; }
    }
    #endregion

    //Get
    #region
    public float DefaultSpeed_Default
    {
        get { return defaultSpeed_Default; }
    }
    public float SpeedUpRatio_Default
    {
        get { return speedUpRatio_Default; }
    }
    public float SpeedUpRatioLimit_Default
    {
        get { return speedUpRatioLimit_Default; }
    }
    public int SpeedUpTurnInterval_Default
    {
        get { return speedUpTurnInterval_Default; }
    }

    public bool CanReselect_Default
    {
        get { return canReselect_Default; }
    }
    public bool CanShowEnemyShell_Default
    {
        get { return canShowEnemyShell_Default; }
    }


    public int CanShieldContinuously_Default
    {
        get { return canShieldContinuously_Default; }
    }

    public int[] ShellDamage_Default
    {
        get { return shellDamage_Default; }
    }
    public float[] ShellProbability_Default
    {
        get { return shellProbability_Default; }
    }

    public int PlayerHp_max_Default
    {
        get { return playerHp_max_Default; }
    }
    public int PlayerShield_max_Default
    {
        get { return playerShield_max_Default; }
    }
    public int PlayerSgMag_max_Default
    {
        get { return playerSgMag_max_Default; }
    }

    public int EnemyHp_max_Default
    {
        get { return enemyHp_max_Default; }
    }
    public int EnemyShield_max_Default
    {
        get { return enemyShield_max_Default; }
    }
    public int EnemySgMag_max_Default
    {
        get { return enemySgMag_max_Default; }
    }
    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);  // シーンの外に出ても破壊されない
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultSpeed_Default = defaultSpeed;
        speedUpRatio_Default = speedUpRatio;
        speedUpRatioLimit_Default = speedUpRatioLimit;
        speedUpTurnInterval_Default = speedUpTurnInterval;
        canReselect_Default = canReselect;
        canShowEnemyShell_Default = canShowEnemyShell;
        canShieldContinuously_Default = canShieldContinuously;

        shellDamage_Default = (int[])shellDamage.Clone();
        shellProbability_Default = (float[])shellProbability.Clone();

        playerHp_max_Default = playerHp_max;
        playerShield_max_Default = playerShield_max;
        playerSgMag_max_Default = playerSgMag_max;
        enemyHp_max_Default = enemyHp_max;
        enemyShield_max_Default = enemyShield_max;
        enemySgMag_max_Default = enemySgMag_max;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
