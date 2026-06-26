using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private InputField[] inputField_shellDamage;
    [SerializeField] private InputField inputField_playerHp;
    [SerializeField] private InputField inputField_playerShield;
    [SerializeField] private InputField inputField_enemyHp;
    [SerializeField] private InputField inputField_enemyShield;
    [SerializeField] private InputField inputField_canShieldContinuously;
    private GameObject settingData;
    private SettingData settingDataComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        settingData = GameObject.Find("SettingData");
        settingDataComponent = settingData.GetComponent<SettingData>();

        // SettingDataの情報をUIに反映させる
        SetUIParameter();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnsureAtLeastOneShellProbability()
    {
        bool allZero = true;

        for (int i = 0; i < settingDataComponent.ShellProbability.Length; i++)
        {
            if (settingDataComponent.ShellProbability[i] > 0f)
            {
                allZero = false;
                break;
            }
        }

        if (allZero)
        {
            settingDataComponent.ShellProbability[0] = 1f;
        }
        SetUIParameter();
    }


    /// <summary>
    /// ボタンから呼び出される関数
    /// </summary>
    public void SetDefault()
    {
        settingDataComponent.DefaultSpeed = settingDataComponent.DefaultSpeed_Default;
        settingDataComponent.SpeedUpRatio = settingDataComponent.SpeedUpRatio_Default;
        settingDataComponent.SpeedUpRatioLimit = settingDataComponent.SpeedUpRatioLimit_Default;
        settingDataComponent.SpeedUpTurnInterval = settingDataComponent.SpeedUpTurnInterval_Default;
        settingDataComponent.CanReselect = settingDataComponent.CanReselect_Default;
        settingDataComponent.CanShowEnemyShell = settingDataComponent.CanShowEnemyShell_Default;
        settingDataComponent.CanShieldContinuously = settingDataComponent.CanShieldContinuously_Default;

        settingDataComponent.ShellDamage = (int[])settingDataComponent.ShellDamage_Default.Clone();
        settingDataComponent.ShellProbability = (float[])settingDataComponent.ShellProbability_Default.Clone();

        settingDataComponent.PlayerHp_max = settingDataComponent.PlayerHp_max_Default;
        settingDataComponent.PlayerShield_max = settingDataComponent.PlayerShield_max_Default;
        settingDataComponent.PlayerSgMag_max = settingDataComponent.PlayerSgMag_max_Default;
        settingDataComponent.EnemyHp_max = settingDataComponent.EnemyHp_max_Default;
        settingDataComponent.EnemyShield_max = settingDataComponent.EnemyShield_max_Default;
        settingDataComponent.EnemySgMag_max = settingDataComponent.EnemySgMag_max_Default;

        // SettingDataの情報をUIに反映させる
        SetUIParameter();
    }
    public void ApplyPlayerSettings()
    {
        settingDataComponent.EnemyHp_max = settingDataComponent.PlayerHp_max;
        settingDataComponent.EnemyShield_max = settingDataComponent.PlayerShield_max;
        settingDataComponent.EnemySgMag_max = settingDataComponent.PlayerSgMag_max;

        // SettingDataの情報をUIに反映させる
        SetUIParameter();
    }
    public void ApplyEnemySettings()
    {
        settingDataComponent.PlayerHp_max = settingDataComponent.EnemyHp_max;
        settingDataComponent.PlayerShield_max = settingDataComponent.EnemyShield_max;
        settingDataComponent.PlayerSgMag_max = settingDataComponent.EnemySgMag_max;

        // SettingDataの情報をUIに反映させる
        SetUIParameter();
    }

    //  SettingDataの情報をUIに反映させる
    private void SetUIParameter()
    {
        // toggle //
        {
            var toggle = GameObject.Find("CanReselect").GetComponent<UnityEngine.UI.Toggle>();
            toggle.isOn = settingDataComponent.CanReselect;
        }
        {
            var toggle = GameObject.Find("CanShowEnemyShell").GetComponent<UnityEngine.UI.Toggle>();
            toggle.isOn = settingDataComponent.CanShowEnemyShell;
        }

        // Slider //
        // スピード
        {
            var slider = GameObject.Find("DefaultSpeed").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.DefaultSpeed;
        }
        {
            var slider = GameObject.Find("SpeedUpRatio").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.SpeedUpRatio;
        }
        {
            var slider = GameObject.Find("SpeedUpRatioLimit").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.SpeedUpRatioLimit;
        }
        {
            var slider = GameObject.Find("SpeedUpTurnInterval").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.SpeedUpTurnInterval;
        }

        //  排出確率
        {
            var slider = GameObject.Find("ShellProbability0").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.ShellProbability[0];
        }
        {
            var slider = GameObject.Find("ShellProbability1").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.ShellProbability[1];
        }
        {
            var slider = GameObject.Find("ShellProbability2").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.ShellProbability[2];
        }
        {
            var slider = GameObject.Find("ShellProbability3").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.ShellProbability[3];
        }

        // マガジンサイズ
        {
            var slider = GameObject.Find("PlayerSgMag").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.PlayerSgMag_max;
        }
        {
            var slider = GameObject.Find("EnemySgMag").GetComponent<UnityEngine.UI.Slider>();
            slider.value = settingDataComponent.EnemySgMag_max;
        }

        // InputField //
        // ダメージ
        inputField_shellDamage[0].text = string.Format("{0}", settingDataComponent.ShellDamage[0]);
        inputField_shellDamage[1].text = string.Format("{0}", settingDataComponent.ShellDamage[1]);
        inputField_shellDamage[2].text = string.Format("{0}", settingDataComponent.ShellDamage[2]);
        inputField_shellDamage[3].text = string.Format("{0}", settingDataComponent.ShellDamage[3]);
        // 体力
        inputField_playerHp.text = string.Format("{0}", settingDataComponent.PlayerHp_max);
        inputField_enemyHp.text = string.Format("{0}", settingDataComponent.EnemyHp_max);
        // シールド
        inputField_playerShield.text = string.Format("{0}", settingDataComponent.PlayerShield_max);
        inputField_enemyShield.text = string.Format("{0}", settingDataComponent.EnemyShield_max);
        inputField_canShieldContinuously.text = string.Format("{0}", settingDataComponent.CanShieldContinuously);
    }

    /// <summary>
    /// トグルボタンから呼び出される関数
    /// </summary>
#region
    public void SetCanReselect(bool value)
    {
        var toggle = GameObject.Find("CanReselect").GetComponent<UnityEngine.UI.Toggle>();
        settingDataComponent.CanReselect = toggle.isOn;
    }
    public void SetCanShowEnemyShell(bool value)
    {
        var toggle = GameObject.Find("CanShowEnemyShell").GetComponent<UnityEngine.UI.Toggle>();
        settingDataComponent.CanShowEnemyShell = toggle.isOn;
    }
    #endregion

    /// <summary>
    /// スライダーから呼び出される関数
    /// </summary>
#region
    // 選択時間
    #region
    public void SetDefaultSpeed()
    {
        var slider = GameObject.Find("DefaultSpeed").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.DefaultSpeed = slider.value;
    }
    public void SetSpeedUpRatio()
    {
        var slider = GameObject.Find("SpeedUpRatio").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.SpeedUpRatio = slider.value;
    }
    public void SetSpeedUpRatioLimit()
    {
        var slider = GameObject.Find("SpeedUpRatioLimit").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.SpeedUpRatioLimit = slider.value;
    }
    public void SetSpeedUpTurnInterval()
    {
        var slider = GameObject.Find("SpeedUpTurnInterval").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.SpeedUpTurnInterval = (int)slider.value;
    }
    #endregion

    // 排出確率
    #region
    public void SetShellProbability0()
    {
        var slider = GameObject.Find("ShellProbability0").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.ShellProbability[0] = slider.value;
    }
    public void SetShellProbability1()
    {
        var slider = GameObject.Find("ShellProbability1").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.ShellProbability[1] = slider.value;
    }
    public void SetShellProbability2()
    {
        var slider = GameObject.Find("ShellProbability2").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.ShellProbability[2] = slider.value;
    }
    public void SetShellProbability3()
    {
        var slider = GameObject.Find("ShellProbability3").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.ShellProbability[3] = slider.value;
    }
    #endregion

    // Player
    public void SetPlayerSgMagMax()
    {
        var slider = GameObject.Find("PlayerSgMag").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.PlayerSgMag_max = (int)slider.value;
    }
    // Enemy
    public void SetEnemySgMagMax()
    {
        var slider = GameObject.Find("EnemySgMag").GetComponent<UnityEngine.UI.Slider>();
        settingDataComponent.EnemySgMag_max = (int)slider.value;
    }
    #endregion

    /// <summary>
    /// 入力フォームから呼び出される関数
    /// </summary>
#region
    // ダメージ
    #region
    public void SetShellDamage0(int dummy)
    {
        int value;
        if (int.TryParse(inputField_shellDamage[0].text, out value))
        {
            settingDataComponent.ShellDamage[0] = value;
        }
        else
        {
            settingDataComponent.ShellDamage[0] = settingDataComponent.ShellDamage_Default[0];
            inputField_shellDamage[0].text = settingDataComponent.ShellDamage[0].ToString();
        }
    }
    public void SetShellDamage1(int dummy)
    {
        int value;
        if (int.TryParse(inputField_shellDamage[1].text, out value))
        {
            settingDataComponent.ShellDamage[1] = value;
        }
        else
        {
            settingDataComponent.ShellDamage[1] = settingDataComponent.ShellDamage_Default[1];
            inputField_shellDamage[1].text = settingDataComponent.ShellDamage[1].ToString();
        }
    }
    public void SetShellDamage2(int dummy)
    {
        int value;
        if (int.TryParse(inputField_shellDamage[2].text, out value))
        {
            settingDataComponent.ShellDamage[2] = value;
        }
        else
        {
            settingDataComponent.ShellDamage[2] = settingDataComponent.ShellDamage_Default[2];
            inputField_shellDamage[2].text = settingDataComponent.ShellDamage[2].ToString();
        }
    }
    public void SetShellDamage3(int dummy)
    {
        int value;
        if (int.TryParse(inputField_shellDamage[3].text, out value))
        {
            settingDataComponent.ShellDamage[3] = value;
        }
        else
        {
            settingDataComponent.ShellDamage[3] = settingDataComponent.ShellDamage_Default[3];
            inputField_shellDamage[3].text = settingDataComponent.ShellDamage[3].ToString();
        }
    }
    #endregion


    // HPとシールド
    #region
    // Player
    // HP
    public void SetPlayerHpMax(int dummy)
    {
        int value;
        if (int.TryParse(inputField_playerHp.text, out value) && value > 0)
        {
            settingDataComponent.PlayerHp_max = value;
        }
        else
        {
            settingDataComponent.PlayerHp_max = 1;
            inputField_playerHp.text = settingDataComponent.PlayerHp_max.ToString();
        }
    }
    // シールド
    public void SetPlayerShieldMax(int dummy)
    {
        int value;
        if (int.TryParse(inputField_playerShield.text, out value) && value > 0)
        {
            settingDataComponent.PlayerShield_max = value;
        }
        else
        {
            settingDataComponent.PlayerShield_max = 0;
            inputField_playerShield.text = settingDataComponent.PlayerShield_max.ToString();
        }
    }

    // Enemy
    // HP
    public void SetEnemyHpMax(int dummy)
    {
        int value;
        if (int.TryParse(inputField_enemyHp.text, out value) && value > 0)
        {
            settingDataComponent.EnemyHp_max = value;
        }
        else
        {
            settingDataComponent.EnemyHp_max = 1;
            inputField_enemyHp.text = settingDataComponent.EnemyHp_max.ToString();
        }
    }
    // シールド
    public void SetEnemyShieldMax(int dummy)
    {
        int value;
        if (int.TryParse(inputField_enemyShield.text, out value) && value > 0)
        {
            settingDataComponent.EnemyShield_max = value;
        }
        else
        {
            settingDataComponent.EnemyShield_max = 0;
            inputField_enemyShield.text = settingDataComponent.EnemyShield_max.ToString();
        }
    }
    #endregion

    // 連続シールド
    public void SetCanShieldContinuously(int dummy)
    {
        int value;
        if (int.TryParse(inputField_canShieldContinuously.text, out value) && value >= 0)
        {
            settingDataComponent.CanShieldContinuously = value;
        }
        else
        {
            settingDataComponent.CanShieldContinuously = 1;
            inputField_canShieldContinuously.text = settingDataComponent.CanShieldContinuously.ToString();
        }
    }
#endregion


}
