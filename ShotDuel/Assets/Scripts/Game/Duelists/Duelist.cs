using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// //"Player.cs" "Enemy.cs"の基底クラス。
/// </summary>

#region 必須コンポーネントを追加
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
#endregion

public class Duelist : MonoBehaviour
{
    #region COMMAND:選択肢
    public enum COMMAND
    {
    SUICIDE=-2,

    RANDOM = -1,
    NONE = 0,  // SELECTターン状態
    
    SHOT,
    EJECTED,
    RELOAD,
	SHIELD,
    };
    #endregion


    #region ANIM:アニメーション
    public enum ANIM
    {
        IDLE = 0,
        // 1-5
        SHOT,
        RELOAD,
        EJECTED,
        SHIELD,
        NONE,
        //6-8
        DAMAGED,
        DEAD,
        WIN,
        // 9
        SUICIDE
    };
    #endregion

    #region ANIM:アニメーションSE
    public enum ANIM_SE
    {
        SHOT = 0,
        RELOAD,
        EJECTED,
        SHIELD,
        DAMAGED,
        DEAD,
    };
    #endregion

    #region ANIM:アニメーションCV
    public enum ANIM_CV
    {
        HELLO=0,
        NONE,
        SHOT,
        EJECTED,
        SHIELD,
        DAMAGED,
        DEAD,
        WIN,

    };
    #endregion


    #region [SerializeField]
    [Header("他オブジェクト")]
    [SerializeField] protected UIManager uiManager;         // UIManager
    [SerializeField] protected GameManager gameManager;     // GameManager
    [Space]
    [Header("SE")]
    [SerializeField] protected AudioClip se_Shot;           // SE:「弾丸発射」
    [SerializeField] protected AudioClip se_Reload;         // SE:「再装填」
    [SerializeField] protected AudioClip se_Ejected;        // SE:「排莢」
    [SerializeField] protected AudioClip se_HitShield;      // SE:「排莢」
    [Header("ボイス")]
    [SerializeField] protected AudioClip cv_Hello;          // CV:「挨拶」
    [SerializeField] protected AudioClip cv_None;           // CV:「なにもしない」
    [SerializeField] protected AudioClip cv_Shot;           // CV:「射撃」
    [SerializeField] protected AudioClip cv_Ejected;        // CV:「排莢」
    [SerializeField] protected AudioClip cv_HitShield;      // CV:「被弾防御」
    [SerializeField] protected AudioClip cv_Damaged;        // CV:「被弾」
    [SerializeField] protected AudioClip cv_Dead;           // CV:「死亡」
    [SerializeField] protected AudioClip cv_Win;            // CV:「勝利」
    #endregion

    protected SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;
    protected Animator animator;

    protected int hp_max = 0;                                        // 最大体力（GMから取得）
    protected int hp_current=0;                                      // 現在体力
    protected int shield_max = 0;                                    // 最大シールド（GMから取得）
    protected int shield_current=0;                                  // 現在シールド

    protected int sgMag_max = 0;                                     // 最大装弾数（GMから取得）
    protected List<int> sgMag = new List<int>(){ };                  // チャンバー情報 5(0～4)
    protected COMMAND command = 0;                                   // 選んだ選択肢を入れる
    protected List<COMMAND> canCommand = new List<COMMAND>() { };    // 選択可能コマンドリスト

    protected int numberOfShield = 0;                                // 連続ガード実行回数

    protected bool isHitShield = false;                              // 盾防御フラグ
    protected bool isDamaged = false;                                // 被弾フラグ
    protected bool isAnimationEnd = false;                           // アニメーション終了

    protected int shotCount = 0;
    protected int ejectedCount = 0;
    protected int reloadCount = 0;
    protected int shieldCount = 0;
    protected int noneCount = 0;
    protected bool noDamage = true;

    #region Get&Set変数+α
    public int Hp_current
    {
        get { return hp_current; }
        set { hp_current = value; }
    }
    public int Shield_current
    {
        get { return shield_current; }
        set { shield_current = value; }
    }
    public bool IsHitShield
    {
        set { isHitShield = value; }
    }
    public bool IsAnimationEnd
    {
        get { return isAnimationEnd; }
        set { isAnimationEnd = value; }
    }
    public List<int> SgMag { get => sgMag; }
    public int SgMag_max { get => sgMag_max; }
    public COMMAND Command
    {
        get { return command; }
        set { command = value; }
    }

    public int NumberOfShield
    {
        get { return numberOfShield; }
        set { numberOfShield = value; }
    }
    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

    public void CanCommandClear() { canCommand.Clear(); }

    // リザルト関係
    #region リザルト画面にて
    public int ShotCount
    {
        get { return shotCount; }
        set { shotCount = value; }
    }
    public int EjectedCount
    {
        get { return ejectedCount; }
        set { ejectedCount = value; }
    }
    public int ReloadCount
    {
        get { return reloadCount; }
        set { reloadCount = value; }
    }
    public int ShieldCount
    {
        get { return shieldCount; }
        set { shieldCount = value; }
    }
    public int NoneCount
    {
        get { return noneCount; }
        set { noneCount = value; }
    }
    public bool NoDamage
    {
        get { return noDamage; }
        set { noDamage = value; }
    }

    #endregion

    #endregion

    #region Start
    public virtual void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 選択肢の初期化
        command = COMMAND.RANDOM;
    }
#endregion

    /// <summary>
    /// 選択可能なコマンドの中からランダムに選択する
    /// </summary>
#region RandomCommand()
    public void RandomCommand(Duelist me,Duelist you)
    {
        // 選択可能なコマンドをリストに追加
        if (me.SgMag.Count > 0) //（装填数が１発以上）
        {
            if (me.SgMag[0] != 0)
            {
                // [SHOT]を有効化
                me.canCommand.Add(COMMAND.SHOT);
            }
            else
            {
                // [EJECT]を有効化
                me.canCommand.Add(COMMAND.EJECTED);
            }
        }
        if (me.SgMag.Count < me.sgMag_max) //（装弾数以下）
        {
            // [RELOAD]を有効化
            me.canCommand.Add(COMMAND.RELOAD);
        }
        if (me.Shield_current > 0 && you.SgMag.Count > 0 && me.numberOfShield < gameManager.CanShieldContinuously) //（シールド１以上 かつ 装填1発以上）
        {
            // 
            if (!gameManager.CanShowEnemyShell || (gameManager.CanShowEnemyShell && you.SgMag[0] > 0) )
            {
                // [SHIELD]を有効化
                me.canCommand.Add(COMMAND.SHIELD);
            }
        }
        if (me.canCommand.Count > 0)
        {
            // 実行可能なコマンドの中からランダムで
            me.command = me.canCommand[Random.Range(0, me.canCommand.Count)];
        }
        else
        {
            // 実行可能な行動がない場合
            me.command = COMMAND.NONE;
        }
    }
#endregion


    // アニメーションを切り替え
#region SetAnim(ANIM animName)
    public void SetAnim(ANIM animName)
    {
        // 行動選択にあわせてアニメーションを発行
        animator.SetInteger("AnimNo", (int)animName);
    }
#endregion


    /// <summary> //////////////////////////////
    /// アニメーションウィンドウ上で呼ぶ関数 ///
    /// </summary> /////////////////////////////
    
    // ダメージアニメーションを再生するか確認
#region DamagedAnimCheck()
    public void DamagedAnimCheck()
    {
        // "true"ならダメージアニメーションに切り替え
        if (isDamaged)
        {
            if (hp_current <= 0)
            {
                // HPが0になる
                SetAnim(ANIM.DEAD);
            }
            else
            {
                // HPが1以上残っている
                SetAnim(ANIM.DAMAGED);
            }
        }
    }
#endregion

    // アニメーション再生終了時に呼び出し
#region AnimationEnd()
    public void AnimationEnd()
    {
        isDamaged = false;
        isAnimationEnd = true;
    }
#endregion

    // アニメーションSE再生時に呼び出し
#region AnimationPlaySE()
    public void AnimationPlaySE(ANIM_SE seName)
    {
        switch (seName)
        {
            case ANIM_SE.SHOT:
                audioSource?.PlayOneShot(se_Shot);
                break;
            case ANIM_SE.RELOAD:
                audioSource?.PlayOneShot(se_Reload);
                break;
            case ANIM_SE.EJECTED:
                audioSource?.PlayOneShot(se_Ejected);
                break;
            case ANIM_SE.SHIELD:
                if (isHitShield) { audioSource?.PlayOneShot(se_HitShield); }
                break;

            default:
                Debug.LogWarning("未定義のサウンド名: " + seName);
                break;
        }
    }
#endregion

    // アニメーションVOICE再生時に呼び出し
#region AnimationPlaySE()
    public void AnimationPlayCV(ANIM_CV cvName)
    {
        switch (cvName)
        {
            case ANIM_CV.HELLO:
                audioSource?.PlayOneShot(cv_Hello);
                break;
            case ANIM_CV.NONE:
                audioSource?.PlayOneShot(cv_None);
                break;
            case ANIM_CV.SHOT:
                audioSource?.PlayOneShot(cv_Shot);
                break;
            case ANIM_CV.EJECTED:
                audioSource?.PlayOneShot(cv_Ejected);
                break;
            case ANIM_CV.SHIELD:
                if (isHitShield) { audioSource?.PlayOneShot(cv_HitShield); }
                break;
            case ANIM_CV.DAMAGED:
                audioSource?.PlayOneShot(cv_Damaged);
                break;
            case ANIM_CV.DEAD:
                audioSource?.PlayOneShot(cv_Dead);
                break;
            case ANIM_CV.WIN:
                audioSource?.PlayOneShot(cv_Win);
                break;

            default:
                Debug.LogWarning("未定義のボイス名: " + cvName);
                break;
        }
    }
#endregion
}