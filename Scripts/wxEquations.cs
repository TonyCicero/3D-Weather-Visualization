using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wxEquations : MonoBehaviour
{

    const float c = 243.04f;
    const float b = 17.625f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public float dewPoint(float T, float RH){ // supply temperature (c) & relative humidity (%)
        return (Mathf.Log(RH/100) + ((b*T)/(c+T)));
    }

    public float cloudBase(float T, float dP){ // returns cloud base alt in ft
        return (((T-dP)/2.5f)*1000);
    }

    public float Celcius2Fahrenheit(float C){ //(0°C × 9/5) + 32 = 32°F
        return (C*(9/5)) + 32;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
