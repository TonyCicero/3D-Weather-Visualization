using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    [SerializeField]
    Transform Earth;
    [SerializeField]
    float zoomSpeed = 10;
    [SerializeField]
    float panSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        { 
            transform.RotateAround(Earth.position, new Vector3(0,1,0), 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.RotateAround(Earth.position, new Vector3(0, -1, 0), 30 * Time.deltaTime);

        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.RotateAround(Earth.position, new Vector3(0, 0, 1), 30 * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.RotateAround(Earth.position, new Vector3(0, 0, -1), 30 * Time.deltaTime);
        }

        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            transform.position += transform.forward * Time.deltaTime*zoomSpeed;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            transform.position -= transform.forward * Time.deltaTime*zoomSpeed;
        }
       
        if (Input.GetAxis("Mouse X") > 0)
        {
            //transform.Rotate(0, panSpeed, 0);
        }
        else if (Input.GetAxis("Mouse X") < 0)
        {
            //transform.Rotate(0, -panSpeed, 0);
        }
        if (Input.GetAxis("Mouse Y") > 0)
        {
            //transform.Rotate(-panSpeed,0, 0);
        }
        else if (Input.GetAxis("Mouse Y") < 0)
        {
            //transform.Rotate(panSpeed, 0, 0);
        }




    }
}
