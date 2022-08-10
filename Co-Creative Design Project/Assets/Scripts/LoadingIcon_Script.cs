using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://www.youtube.com/watch?v=MFyrt3t5nXU
public class LoadingIcon_Script : MonoBehaviour
{
    private float rotZ;
    public float rotationSpeed;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotZ += Time.deltaTime * rotationSpeed; // Rotation of loading icon on Z Axis by set speed

        transform.rotation = Quaternion.Euler(0, 0, rotZ);
    }
}
