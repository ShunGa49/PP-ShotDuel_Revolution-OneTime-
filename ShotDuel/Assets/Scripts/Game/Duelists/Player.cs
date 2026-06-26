using UnityEngine;
using static GameManager;

public class Player : Duelist
{
    private bool isSuicide = false;
    public bool IsSuicide { get => isSuicide; }

    // Start
    public override void Start()
    {
        base.Start();

        // GMから取得
        hp_max = gameManager.PlayerHp_max;
        shield_max = gameManager.PlayerShield_max;
        sgMag_max = gameManager.PlayerSgMag_max;
        // ステータスの初期化
        hp_current = hp_max;
        shield_current = shield_max;
    }

    /// <summary>
    /// 自殺（アニメーションから呼び出し）
    /// </summary>
    public void PlayerSuicide()
    {
        hp_current = 0;
        isSuicide = true;
    }
}