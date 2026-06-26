using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SceneManager.LoadScene("TitleScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
