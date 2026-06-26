using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SeceLoadButton : MonoBehaviour
{
    enum LOAD_SCENE
    {
        Title=0,
        Rule,
        Config,
        Game,
        End,

        Reload
    }

    [Header("SE")]
    [SerializeField] private LOAD_SCENE loadScene;
    [Header("SE")]
    [SerializeField] private AudioClip se_ButtonClick;
    [Header("フェード")]
    [SerializeField] private Fade fadePanel = null;     // FadePanel
    [SerializeField] private float fadeOutTime = 2.0f;

    private AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// シーン遷移ボタンの関数。
    /// </summary>
    public void PushButton()
    {
        audioSource.PlayOneShot(se_ButtonClick);

        Action on_completed = () =>
        {
            switch (loadScene)
            {
                case LOAD_SCENE.Title:
                    SceneManager.LoadScene("TitleScene");
                    break;
                case LOAD_SCENE.Rule:
                    SceneManager.LoadScene("RuleScene");
                    break;
                case LOAD_SCENE.Config:
                    SceneManager.LoadScene("ConfigScene");
                    break;
                case LOAD_SCENE.Game:
                    SceneManager.LoadScene("GameScene");
                    break;
                
                case LOAD_SCENE.Reload:
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    break;

                case LOAD_SCENE.End:
                #if UNITY_EDITOR
                    // Unityエディターでの動作
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    // 実際のゲーム終了処理
                    Application.Quit();
                #endif
                    SceneManager.LoadScene("TitleScene");
                    break;
                default:
                    Debug.LogWarning("例外が発生しました。");
                    break;
            }
        };
        fadePanel.FadeOut(fadeOutTime, on_completed);

        
    }
}
