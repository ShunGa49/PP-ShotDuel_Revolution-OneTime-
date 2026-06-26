using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class Enemy_Remington : MonoBehaviour
{
    [Header("CV")]
    [SerializeField] private AudioClip cv_Start;

    private AudioSource audioSource;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPlayAnim()
    {
        animator.SetBool("IsStart",true);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(cv_Start);
        }
    }
}
