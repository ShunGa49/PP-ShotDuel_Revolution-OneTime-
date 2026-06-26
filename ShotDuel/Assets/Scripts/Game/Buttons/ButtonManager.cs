using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CommandButton[] commandButtonGroup;    // 各ボタンのC#スクリプトを取得

    // Start
    void Start()
    {
    }

    // Update
    void Update()
    {
    }

    /// <summary>
    /// 条件に合うものは有効化（初期化で呼び出し）💗
    /// </summary>
    public void AllToggteReset(bool enable = false)
    {
        foreach (var button in commandButtonGroup)
        {
            // クリックしていない
            button.ToggleSetOff();
            // 無効化
            button.SetInteractable(false);

            if (enable)
            {
                // 条件が合う場合、有効化
                button.Reset();
            }
        }
    }

    /// <summary>
    /// 指定されたID以外のトグルボタンを 無効化 / 未選択化
    /// </summary>
    public void AllToggteDisable_withoutID(int id = 0)
    {
        foreach (var button in commandButtonGroup)
        {
            if (id == 0 || button.ID != id)
            {
                if (gameManager.CanReselect)
                {
                    button.Reset();
                }
                else
                {
                    button.SetInteractable(false);
                }
            }
        }
    }
}
