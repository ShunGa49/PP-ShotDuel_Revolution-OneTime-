using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private Image image = null;

    // エディタでコンポーネント初期化のときに呼ばれるメソッド
    private void Reset()
    {
        // 画像を取得
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Imageのalpha値を時間経過で[0→1]へ変更するコルーチン
    /// </summary>
    /// <param name="time">フェードの所要時間(s)</param>
    /// <param name="on_completed">完成した</param>
    /// <param name="is_reversing">逆転する</param>
    /// <returns></returns>
    #region Imageのalpha値を時間経過で[0→1]へ変更するコルーチン: ChangeAlphaValueFrom0To1OverTime()
    private IEnumerator ChangeAlphaValueFrom0To1OverTime( float duration, Action on_completed, bool isReversing = false )
    {
        if (!isReversing) // fadeout
        {
            image.enabled = true;
        }

        float elapsed_time = 0.0f; // 経過時間
        Color color = image.color;

        while (elapsed_time < duration)
        {
            // 進捗率(0f～1f)
            float elapsed_rate = Mathf.Min(elapsed_time / duration, 1.0f);
            // alpha値
            if (isReversing)
            {
                // alpha値 = 1f-進捗率
                color.a = 1.0f - elapsed_rate;
            }
            else
            {
                // alpha値 = 進捗率
                color.a = elapsed_rate;
            }
            image.color = color;

            yield return null;
            elapsed_time += Time.deltaTime;
        }

        if (isReversing) // fadein
        {
            image.enabled = false;
        }
        if (on_completed != null)
        {
            on_completed();
        }
    }
    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration"></param>
    /// <param name="on_completed"></param>
    #region
    // 濃→薄
    public void FadeIn(float duration, Action on_completed = null)
    {
        StartCoroutine(ChangeAlphaValueFrom0To1OverTime(duration, on_completed, true));
    }
    // 薄→濃
    public void FadeOut(float duration, Action on_completed = null)
    {
        StartCoroutine(ChangeAlphaValueFrom0To1OverTime(duration, on_completed));
    }
    #endregion
}
//参考：https://game-programming-lab.com/unity/user-interface-in-unity/create-fade-in-unity/