using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections.Specialized;
using System;


public class LoadLayers : MonoBehaviour
{
    [System.Serializable]
    public class cloudLayer
    {
        public Renderer inner;
        public Renderer outer;
    };
    public cloudLayer[] CloudL;
    public Renderer TempL;
    public Renderer WindL;
    public Renderer PrecipL;
    public string mainPath = "/Layers/";
    public string tempPath = "Temperature/";
    public string windPath = "Wind/";
    public string precipPath = "Precip/";
    public string cloudPath = "Clouds/";
    Dictionary<string, Texture2D> TempLayers; // Stores Temperature Layers
    Dictionary<string, Texture2D> WindLayers; // Stores Wind Layers
    Dictionary<string, Texture2D> PrecipLayers; // Stores Precip Layers
    Dictionary<string, List<Texture2D>> CloudLayers; // Stores Cloud Layers
    int imgWidth = 2000;
    int imgHeight = 1000;

    public float frameDelay = 0.25f;

    List<string> setID;

    // Start is called before the first frame update
    void Start()
    {
        setID = new List<string>();
        TempLayers = new Dictionary<string, Texture2D>();
        WindLayers = new Dictionary<string, Texture2D>();
        PrecipLayers = new Dictionary<string, Texture2D>();
        CloudLayers = new Dictionary<string, List<Texture2D>>();
    }

    public void startAnim()
    {
        StartCoroutine(animateWX());
    }
    public void changeSpeed(float speed)
    {
        frameDelay = speed;
    }

    IEnumerator animateWX()
    {
        for (int i = 0; i < setID.Count; i++)
        {
            string id = setID[i];
            DisplayLayer(id);
            yield return new WaitForSeconds(frameDelay);
        }
    }

    public void changePath(string path)
    {
        mainPath = path;
    }

    string extractID(string a)
    {
        string b = null;
        for (int i = 0; i < a.Length; i++)
        {
            if (Char.IsDigit(a[i]))
                b += a[i];
        }
        return b;
    }

    public void loadLayers()
    {
        foreach (string file in System.IO.Directory.GetFiles("T:/Layers/Temperature/"))
        {
            byte[] fileData = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(imgWidth, imgHeight);
            texture.LoadImage(fileData);
            string id = extractID(file);
            TempLayers.Add(id, texture);
            setID.Add(id);

        }
        foreach (string file in System.IO.Directory.GetFiles(mainPath + windPath))
        {

            byte[] fileData = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(imgWidth, imgHeight);
            texture.LoadImage(fileData);
            string id = extractID(file);
            WindLayers.Add(id, texture);

        }
        foreach (string file in System.IO.Directory.GetFiles(mainPath + precipPath))
        {

            byte[] fileData = File.ReadAllBytes(file);
            Texture2D texture = new Texture2D(imgWidth, imgHeight);
            texture.LoadImage(fileData);
            string id = extractID(file);
            PrecipLayers.Add(id, texture);

        }
        foreach (string file in System.IO.Directory.GetFiles(mainPath + cloudPath))
        {
            string pathName = (mainPath + cloudPath);
            string fileName = file.Remove(0, pathName.Length);
            if (fileName[0] == 'A')
            {
                List<Texture2D> tmp = new List<Texture2D>();
               
                for (int i = 0; i < 5; i++)
                {
                    byte[] fileData = File.ReadAllBytes(pathName+ fileName.Replace('A', (char)(65+i)));
                    tmp.Add(new Texture2D(imgWidth, imgHeight));
                    tmp[i].LoadImage(fileData);
                }
                string id = extractID(file);
                CloudLayers.Add(id, tmp);
            }
           

        }
    }

    public void DisplayLayer(string id){ // sets renderer texture to layer based on setID
        TempL.material.mainTexture = TempLayers[id];
        WindL.material.mainTexture = WindLayers[id];
        PrecipL.material.mainTexture = PrecipLayers[id];
        for (int i = 0; i < 5; i++)
        {
            CloudL[i].inner.material.mainTexture = CloudLayers[id][i];
            CloudL[i].outer.material.mainTexture = CloudLayers[id][i];
        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
