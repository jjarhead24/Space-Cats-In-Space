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
                    SoundPlayer.PlayOneShot(Shocked);
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
                    SoundPlayer.PlayOneShot(LvlComplete);
                    Invoke("LoadNextLvl", 1f); 
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
        SceneManager.LoadScene(0);
    }

    private void LoadNextLvl()
    {

        
        SceneManager.LoadScene(1); //allow more levels to load
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
            rigid.AddRelativeForce(Vector3.up * ThrustSpeed);
            if (!SoundPlayer.isPlaying)
            {
                SoundPlayer.PlayOneShot(Engine);
            }
            
        }
        else
        {
            SoundPlayer.Stop();
        }
    }
}
