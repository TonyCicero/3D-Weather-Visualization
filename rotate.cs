using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    float speed = 10;
    public bool doRotate = true;

    void Start()
    {
        
    }

    public void toggle(bool x)
    {
        doRotate = x;
    }

    // Update is called once per frame
    void Update()
    {
        if (doRotate)
        {
            transform.Rotate(0, 0, speed);
        }
    }
}
