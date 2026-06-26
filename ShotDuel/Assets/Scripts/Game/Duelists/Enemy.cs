using UnityEngine;

public class Enemy : Duelist
{
    // Start 
    public override void Start()
    {
        base.Start();

        // GMから取得
        hp_max = gameManager.EnemyHp_max;
        shield_max = gameManager.EnemyShield_max;
        sgMag_max = gameManager.EnemySgMag_max;
        // ステータスの初期化
        hp_current = hp_max;
        shield_current = shield_max;
    }

    // Update
    void Update()
    {
    }
}
