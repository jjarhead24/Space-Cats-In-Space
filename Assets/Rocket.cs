using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RCSThrust = 100f;
    [SerializeField] float ThrusterPower = 100f;
    [SerializeField] AudioClip Engine;
    [SerializeField] AudioClip LvlComplete;
    [SerializeField] AudioClip Shocked;
    [SerializeField] ParticleSystem EngineParticle;
    [SerializeField] ParticleSystem SuccessParticle;
    [SerializeField] ParticleSystem DeathParticle;
    public bool KeyObtained = false;

    

    Rigidbody rigid; //this is a variable
    AudioSource SoundPlayer;

    enum State { Alive, Dying, Trancending }
    State CurrentState = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        SoundPlayer = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentState == State.Alive)
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (CurrentState != State.Alive) {return;}



        switch (collision.gameObject.tag)
        {
            case "Death":
                {
                    CurrentState = State.Dying;
                    SoundPlayer.Stop();
                    EngineParticle.Stop();
                    SoundPlayer.PlayOneShot(Shocked);
                    DeathParticle.Play();
                    Invoke("LoadAfterDeath", 2.2f);
                    break;
                }
            case "Energy":
                {
                    print("Energy");
                    break;
                }
            case "Finish":
                {
                    CurrentState =State.Trancending;
                    SoundPlayer.Stop();
                    EngineParticle.Stop();
                    SoundPlayer.PlayOneShot(LvlComplete);
                    SuccessParticle.Play();
                    Invoke("LoadNextLvl", 1f); 
                    break;
                }
            case "Key":
                {
                    KeyObtained = true;
                    break;
                }
            case "":
                {
                    break;
                }
        }
    }

    private void LoadAfterDeath()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextLevel = currentScene.buildIndex;
        nextLevel -= 1;
        if (nextLevel == 0)
        {
            nextLevel = 1;
        }
        SceneManager.LoadScene(nextLevel);

    }

    private void LoadNextLvl()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextLevel = currentScene.buildIndex;
        nextLevel += 1;   
        SceneManager.LoadScene(nextLevel);
        
    }

    private void Rotate()
    {
        rigid.freezeRotation = true; //take manual control of rotation

        float rotationSpeed = RCSThrust * Time.deltaTime; //rotates based on frames per second
        
        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigid.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        float ThrustSpeed = ThrusterPower * Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            rigid.AddRelativeForce(Vector3.up * ThrustSpeed *Time.deltaTime);
            if (!SoundPlayer.isPlaying)
            {
                SoundPlayer.PlayOneShot(Engine);
            }
            EngineParticle.Play();
        }
        else
        {
            SoundPlayer.Stop();
            EngineParticle.Stop();
        }
    }
}
