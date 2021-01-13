using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float RCSThrust = 100f;
    [SerializeField] float ThrusterPower = 100f;
    Rigidbody rigid; //this is a variable
    AudioSource Boosters;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        Boosters = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Death":
                {
                    print("Dead");
                    break;
                }
            case "Energy":
                {
                    print("Energy");
                    break;
                }
            case "":
                {
                    print("OK");
                    break;
                }
        }
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
            if (!Boosters.isPlaying)
            {
                Boosters.Play();
            }
        }
        else
        {
            Boosters.Stop();
        }
    }
}
