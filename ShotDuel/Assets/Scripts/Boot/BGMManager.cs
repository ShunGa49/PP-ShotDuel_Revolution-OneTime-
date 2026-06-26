using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        audioSource.Play(); // 一応最初に鳴らす
    }

    /// <summary>
    /// シーン読み込み時
    /// </summary>
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    /// <summary>
    /// シーン切り替わり時
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// シーン変化時に呼ばれる関数（引数LoadSceneMode modeは使わない）
    /// </summary>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;

        // ゲームシーンは再生を中止
        if (sceneName == "GameScene") 
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        else
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}