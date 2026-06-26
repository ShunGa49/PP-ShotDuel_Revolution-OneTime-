using UnityEngine;
using UnityEngine.UI;
using static Duelist;

public class CommandButton : MonoBehaviour
{
    [Header("取得")]
    [SerializeField] private ButtonManager buttonManager;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Player player;
    [SerializeField] private Text labelText;
    [SerializeField] private Text commandExplanationText;
    [Header("ID")]
    [SerializeField] private int id = 0;
    [Header("コマンド説明")]
    [SerializeField] private string explanationString = "ここに説明文を記入。";
    [Header("SE")]
    [SerializeField] private AudioClip se_OnPointerEnter;
    [SerializeField] private AudioClip se_OnClick;

    public int ID { get { return id; } }
    public void ToggleSetOff() { toggle.isOn = false; }

    private bool isPointer = false;

    private string shotExplanationString = "explanationStringを取得";
    private string ejectExplanationString = "１番目にある空包を捨てる。被弾しても弾は捨てれる。";

    private Toggle toggle;
    private Animator animator;
    private AudioSource audioSource;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //  ButtonManagerの設定確認
        if (buttonManager == null)
        {
            Debug.LogError("buttonManager が null ですわ！ buttonManager を Inspector で設定してくださる？");
        }
        //  IDの設定確認
        if (id == 0)
        {
            Debug.LogError("id が設定されていなくってよ！ Inspector で id を設定してくださる？");
        }

        shotExplanationString = explanationString;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        // トグルボタンの取得
        toggle = GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        // 有効
        if (toggle.interactable)
        {
            // 選択中
            if (toggle.isOn)
            {
                labelText.color = Color.black;
            }
            else
            {
                labelText.color = Color.white;
            }

            // マウスが乗っている
            if (isPointer)
            {
                commandExplanationText.text = explanationString;
                animator.SetBool("IsUp", true);
            }
            else
            {
                animator.SetBool("IsUp", false);
            }
        }
        else
        {
            animator.SetBool("IsUp", false);
            // 無効時のテキスト色
            labelText.color = Color.black;
        }
        
    }

    /// <summary>
    /// ボタン状態のリセット
    /// </summary>
    public void Reset()
    {
        switch (id)
        {
            case 1:
                if (player.SgMag.Count > 0)
                {
                    if (player.SgMag[0] == 0)
                    {
                        labelText.text = "排莢";
                        explanationString = ejectExplanationString;
                    }
                    else
                    {
                        labelText.text = "発砲";
                        explanationString = shotExplanationString;
                    }

                    toggle.interactable = true; // 有効状態
                    toggle.isOn = false;        // クリックしていない
                }
                break;
            case 2:
                if (player.SgMag.Count < player.SgMag_max)
                {
                    toggle.interactable = true; // 有効状態
                    toggle.isOn = false;        // クリックしていない
                }
                break;
            case 3:
                if (player.Shield_current > 0 && player.NumberOfShield < gameManager.CanShieldContinuously)
                {
                    toggle.interactable = true; // 有効状態
                    toggle.isOn = false;        // クリックしていない
                }
                break;
            default:
                break;
        }
    }
    
    /// <summary>
    /// "true"なら有効化
    /// </summary>
    public void SetInteractable(bool enable)
    {
        if (enable) 
        {
            // 有効化
            toggle.interactable = true;
        }
        else
        {
            // 無効化
            toggle.interactable = false;
        }
    }

    /// <summary>
    /// クリック時。
    /// </summary>
    public void OnClick(Toggle toggle)
    {
        //  このイベントはボタンを押すだけでなく、スクリプトで[isOn]の値を変更しても呼ばれるので注意
        if (toggle.isOn)
        {
            audioSource.PlayOneShot(se_OnClick);

            // 他の押していないものを無効化
            buttonManager.AllToggteDisable_withoutID(id);

            switch (id)
            {
                case 1:
                    if (player.SgMag[0] != 0)
                    {
                        player.Command = COMMAND.SHOT;
                    }
                    else
                    {
                        player.Command = COMMAND.EJECTED;
                    }
                    break;
                case 2:
                    player.Command = COMMAND.RELOAD;
                    break;
                case 3:
                    player.Command = COMMAND.SHIELD;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// マウスが乗っているかを判定。
    /// </summary>
    public void OnPointerEnter()
    {
        isPointer = true;

        if (toggle.interactable)
        {
            audioSource.PlayOneShot(se_OnPointerEnter);
        }
    }
    public void OnPointerExit()
    {
        isPointer = false;
    }
}
