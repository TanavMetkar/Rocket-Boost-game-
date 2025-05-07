using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] float levelLoadDelay=2f;

    [SerializeField] ParticleSystem successParticles;
    [SerializeField] ParticleSystem crashParticles;

    AudioSource audioSource;

    bool isTransitioning=false;
    bool collisionDisable=false;

    void Start()
    {
        audioSource=GetComponent<AudioSource>();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionDisable = !collisionDisable;  // toggle collision
        }
    }
     void OnCollisionEnter(Collision other) 
     {
        if(isTransitioning || collisionDisable)
        {
            return;
        }
        switch(other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("This thing is friendly");
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartCrashSequence();
                break;            
        }
     }

     void StartCrashSequence()
     {
         isTransitioning=true;
         audioSource.Stop();
         audioSource.PlayOneShot(crash);
         crashParticles.Play();
         GetComponent<Movement>().enabled=false;
         Invoke("ReloadLevel",levelLoadDelay);
     }
     void ReloadLevel()
     {
         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
         SceneManager.LoadScene(currentSceneIndex);
     }



    void StartSuccessSequence()
    {
        isTransitioning=true;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        GetComponent<Movement>().enabled=false;
        Invoke("LoadNextLevel",levelLoadDelay);
    }
    
     void LoadNextLevel()
     {
         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
         int nextSceneIndex = currentSceneIndex+1;
         if(nextSceneIndex==SceneManager.sceneCountInBuildSettings)
         {
             nextSceneIndex=0;
         }
         SceneManager.LoadScene(nextSceneIndex);
     }
}
