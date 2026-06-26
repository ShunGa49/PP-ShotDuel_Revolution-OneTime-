using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class RuleManager : MonoBehaviour
{
    [Header("フェード")]
    [SerializeField] private Fade fadePanel = null;     // FadePanel
    [SerializeField] private float fadeInTime = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadePanel.FadeIn(fadeInTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
