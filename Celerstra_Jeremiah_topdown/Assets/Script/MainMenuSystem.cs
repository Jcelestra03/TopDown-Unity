using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSystem : MonoBehaviour
{
    public AudioClip MenuSoundEffect;
    private AudioSource noise;


    // Start is called before the first frame update
    void Start()
    {
       noise = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadlevel1Delay()
    {
        Invoke("loadlevel1", 0.7f);
    }

    public void loadlevel1()
    {
        
        SceneManager.LoadScene("SampleScene");
        noise.clip = MenuSoundEffect;
        noise.Play();
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
