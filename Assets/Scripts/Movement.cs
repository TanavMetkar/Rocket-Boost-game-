using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] float mainThrust=100;
    [SerializeField] float rotationThrust=1;
    [SerializeField] AudioClip mainEngine;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;
    
    Rigidbody rb;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))           //start thrusting
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if(!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if(!mainEngineParticles.isPlaying)
            {
            mainEngineParticles.Play();
            } 
        }
        else                                    //stop thrusting
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))            //rotate left
        {
            ApplyRotation(rotationThrust);
            if(!rightThrusterParticles.isPlaying)
            {
                rightThrusterParticles.Play();
            } 
        }
        else if (Input.GetKey(KeyCode.D))       //rotate right
        {
            ApplyRotation(-rotationThrust);
            if(!leftThrusterParticles.isPlaying)
            {
                leftThrusterParticles.Play();
            } 
        }
        else                                    //stop rotating
        {
            rightThrusterParticles.Stop();
            leftThrusterParticles.Stop();
        }
    }

    private void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation=true; // freezing rotation so that you can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation=false; // unfreezing rotation so that physics system can take over
    }
}
