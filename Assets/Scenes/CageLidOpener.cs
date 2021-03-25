using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageLidOpener : MonoBehaviour
{
    // Start is called before the first frame update
    public Rocket Open;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Open.KeyObtained)
        {
            Destroy(gameObject);
        }
        else { return; }
    }
}
