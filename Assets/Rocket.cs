using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{

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

    private void Rotate()
    {
        rigid.freezeRotation = true; //take manual control of rotation

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward);
        }

        rigid.freezeRotation = false; //resume physics control of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigid.AddRelativeForce(Vector3.up);
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
