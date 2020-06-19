using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords : MonoBehaviour
{
    [SerializeField]
    float Radius = 5;
    // Start is called before the first frame update
    void Start()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = Coord2Dto3D(90,0); 
    }

    public Vector3 Coord2Dto3D(float lat,float lon){
        lat = Mathf.Deg2Rad *(lat);
        lon = Mathf.Deg2Rad *(-1*lon);
        float z = Radius * Mathf.Cos(lat) * Mathf.Cos(lon);
        float x = Radius * Mathf.Cos(lat) * Mathf.Sin(lon);
        float y = Radius * Mathf.Sin(lat);
        return new Vector3(x,y,z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
