using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class TitleManager : MonoBehaviour
{

    [Header("フェード")]
    [SerializeField] private Fade fadePanel = null;     // FadePanel
    [SerializeField] private float fadeInTime = 1.0f;

    // Start
    void Start()
    {
        fadePanel.FadeIn(fadeInTime);
    }
}
