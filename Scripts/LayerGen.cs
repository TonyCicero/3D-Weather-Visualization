using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;


public class LayerGen : MonoBehaviour
{
    public int sizeX = 2000;
    public int sizeY = 1000;

    public float lat;
    public float lon;

    public wxEquations Equations;

    Texture2D tex;
    Texture2D Temp_tex;

   // Texture2D[] layerTex;

    public Renderer[] layers;
    public Renderer rend;

    public string path = "T:/Layers/"; //path for layers

    [System.Serializable]
    public class cloudLayer
    {
        public Renderer inner;
        public Renderer outer;
    };
    public cloudLayer[] cloudL;

    public class point{
        public int x;
        public int y;

    };

    public class Sector{
        public bool isNull;
        public bool interpolated;
        public string Location;
        public float temp;
        public float wind;
        public float windDeg;
        public float vis;
        public float pres;
        public float humid;
        public float clouds;
        public int weatherID;
        public int fetchTime;
        public int setID;
        public Sector(){
            isNull = true;
        }

        public Sector(string L, float T, float W, float WD, float V, float P, float H, float C, int I, int F, int S){ // For inputting real data
            isNull = false;
            interpolated = false;
            Location = L;
            temp = T;
            wind = W;
            windDeg = WD;
            vis = V;
            pres = P;
            humid = H;
            clouds = C;
            weatherID = I;
            fetchTime = F;
            setID = S;
        }

        public Sector(float T, float W, float WD, float V, float P, float H, float C,int I){ // For inputting interpolated data
            isNull = false;
            interpolated = true;
            temp = T;
            wind = W;
            windDeg = WD;
            vis = V;
            pres = P;
            humid = H;
            clouds = C;
            weatherID = I;
        }

    };

    public Sector[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        //layerTex = new Texture2D[11];
        grid = new Sector[sizeX,sizeY];

        //tex = new Texture2D(sizeX, sizeY); //create new texture
        //point XYcoords = geoCoords2XY(lat, lon);
        //drawCircle(20, XYcoords.x, XYcoords.y);
        //drawCircle(20,0,0);
        //tex.Apply(); //apply changes to texture
        //rend.material.mainTexture = tex;  //put texture on material
    }

    public void changePath(string cpath)
    {
        path = cpath;
    }

    public void drawCircle(Texture2D Tex, int r, int x, int y){
       
       for (float i =0; i<360; i+=0.1f){
           int x1 = (int)(r*Mathf.Cos(i*Mathf.PI / 180));
           int y1 = (int)(r*Mathf.Sin(i*Mathf.PI / 180));
           Tex.SetPixel(x+x1,y+y1, Color.red); //set Pixel Color
       }
    }
 

    public point geoCoords2XY(float lat, float lon){
        float pixelX = (float)sizeX/360; //pixels per deg of lon
        float pixelY = (float)sizeY/180; //pixels per deg of lat
        int x,y;
        if (lon < 0){
            
            x = (int) ((sizeX/2)+(lon * pixelX));
        }else{
            x = (int) ((sizeX/2)+(Mathf.Abs(lon) * pixelX));
        }
        if (lat < 0){
            y = (int)  ((sizeY/2) - (Mathf.Abs(lat) * pixelY));
        }else{
            y = (int) (((Mathf.Abs(lat) * pixelY))+sizeY/2);
        }
        point Point = new point();
        //Debug.Log(x+","+y);
        Point.x = x;
        Point.y = y;
        return Point;
    }

    public void InsertData(List<Weather> wx){
        Debug.Log(wx.Count);
      
        List<List<Weather>> wxBySetID = new List<List<Weather>>();

        double tempSetID = wx[0].setID;
        int j = 0;
        
        for (int i = 0; i < wx.Count; i++){
            
            if(i == 0)
            {
                wxBySetID.Add(new List<Weather> { wx[i] });
            }
            if (tempSetID != wx[i].setID)
            {
                grid = new Sector[sizeX, sizeY];
                for (int k = 0; k < wxBySetID[j].Count; k++)
                {
                    point pt = new point();
                    float lat = wxBySetID[j][k].Lat;
                    float lon = wxBySetID[j][k].Lon;
                    pt = geoCoords2XY(lat, lon);
                    grid[pt.x, pt.y] = new Sector(wxBySetID[j][k].location, wxBySetID[j][k].temp, wxBySetID[j][k].wind,
                        wxBySetID[j][k].windDeg, wxBySetID[j][k].visibility, wxBySetID[j][k].pressure, wxBySetID[j][k].humidity,
                        wxBySetID[j][k].clouds, wxBySetID[j][k].weatherID, wxBySetID[j][k].fetchTime, wxBySetID[j][k].setID);
                }
                interpolate(10, 2, (int)tempSetID);
                tempSetID = wx[i].setID;
                j++;
                wxBySetID.Add(new List<Weather> { wx[i] });
            }
            else
            {
                wxBySetID[j].Add(wx[i]);
            }
        }
        /*
        for (int i=0; i<wx.Count;i++){
            point pt = new point();
            float lat = wx[i].Lat;
            float lon = wx[i].Lon;
            pt = geoCoords2XY(lat,lon);
            grid[pt.x, pt.y] = new Sector(wx[i].location, wx[i].temp, wx[i].wind, wx[i].windDeg, wx[i].visibility, wx[i].pressure, wx[i].humidity, wx[i].clouds, wx[i].weatherID, wx[i].fetchTime, wx[i].setID);
        }
        interpolate(10,2);*/
    }


    public void interpolate(int radius, int power, int setID){
        for (int x=0; x<sizeX; x++){
            for(int y=0; y<sizeY; y++){
                if (grid[x, y] == null){ //interpolate for this point
                    float denom = 0;
                    float tempNum = 0;
                    float windNum = 0;
                    float windDegNum = 0;
                    float visNum = 0;
                    float presNum = 0;
                    float humidNum = 0;
                    float cloudsNum = 0;
                    float weatherID = 0;
                    for (int i=x-radius; i<= x+radius; i++){
                        for (int j=y-radius; j<=y+radius; j++){
                            int x1=i;
                            int y1=j;
                            if (x1 < 0){
                                x1 = sizeX - i;
                            }
                            if (x1 > sizeX-1){
                                x1 = x1 - sizeX;
                            }
                            if (y1 < 0){
                                y1 = sizeY - y1;
                            }
                            if (y1 > sizeY-1){
                                y1 = y1 - sizeY;
                            }
                            if(grid[x1, y1] != null && !grid[x1, y1].interpolated){
                                float dp = Mathf.Pow(Mathf.Abs(x-i)+Mathf.Abs(y-j), power);
                                denom += 1 / dp;
                                tempNum += (grid[x1, y1].temp)/dp;
                                windNum += (grid[x1, y1].wind)/dp;
                                windDegNum += (grid[x1, y1].windDeg)/dp;
                                visNum += (grid[x1, y1].vis)/dp;
                                presNum += (grid[x1, y1].pres)/dp;
                                humidNum += (grid[x1, y1].humid)/dp;
                                cloudsNum += (grid[x1, y1].clouds)/dp;
                                weatherID += (grid[x1, y1].weatherID); 
                            }

                        }
                    }
                    grid[x, y] = null;
                    grid[x, y] = new Sector(tempNum/denom, windNum/denom, windDegNum/denom, visNum/denom, presNum/denom, humidNum/denom, cloudsNum/denom, (int)(weatherID/denom));
                    
                }
            }
        }
        genTempMap(0,setID);
        genWindSpeed(1,setID);
        genPrecip(2, setID);
        genClouds(3,setID);
        
    }

    public void genTempMap(int l, int setID)
    {
        float keyMax = 40;
        float keyMin = -30;
        float scaledValue;
        Texture2D tmp = new Texture2D(sizeX, sizeY);
        //layerTex[l] = new Texture2D(sizeX, sizeY);
        for(int i=0; i<sizeX; i++){
            for(int j=0; j< sizeY; j++){
                scaledValue = (grid[i,j].temp - keyMin)/(keyMax-keyMin); // scales temp to number between 0-1
                tmp.SetPixel(i,j, Color.Lerp(Color.blue, Color.red, scaledValue));
            }
        }   
        tmp.Apply(); //apply changes to texture
        //layers[l].material.mainTexture = layerTex[l];
        SaveTextureAsPNG(tmp, "/Temperature/"+setID+".png");
    }

    public void genWindSpeed(int l, int setID)
    {
        float keyMin = 0;
        float keyMax = 20;
        float scaledValue;
        //layerTex[l] = new Texture2D(sizeX, sizeY);
        Texture2D tmp = new Texture2D(sizeX, sizeY);
        for (int i=0; i<sizeX; i++){
            for(int j=0; j< sizeY; j++){
                scaledValue = (grid[i,j].wind - keyMin)/(keyMax-keyMin); // scales temp to number between 0-1
                tmp.SetPixel(i,j, Color.Lerp(Color.blue, Color.red, scaledValue));
            }
        }   
        tmp.Apply();
        //layers[l].material.mainTexture = layerTex[1];
        SaveTextureAsPNG(tmp, "/Wind/" + setID+".png");
    }

    public void genPrecip(int l, int setID)
    {
        Texture2D tmp = new Texture2D(sizeX, sizeY);
        //layerTex[l] = new Texture2D(sizeX, sizeY);
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                int wid = grid[i, j].weatherID;
                if ((int)(wid/100) == 5) // rain
                {
                    tmp.SetPixel(i, j, new Color32(0, 255, 0, 255));
                }
                else if ((int)(wid / 100) == 3) // drizzle
                {
                    tmp.SetPixel(i, j, new Color32(0,155,0,255));
                }
                else if ((int)(wid / 100) == 2) // thunder
                {
                    tmp.SetPixel(i, j, new Color32(255, 150, 0, 255));
                }
                else if ((int)(wid / 100) == 6) // snow
                {
                    tmp.SetPixel(i, j, new Color32(0, 150, 255, 255));
                }

            }
        }
        tmp.Apply();
        //layers[l].material.mainTexture = layerTex[1];
        SaveTextureAsPNG(tmp, "/Precip/" + setID + ".png");
    }

    public void genClouds(int l, int setID)
    {
        float[] altLayerCutoffs = {2500, 5000, 10000, 20000, 50000};
        float baseAlt;
        Texture2D[] tmp = new Texture2D[5];
        for(int i =0; i < altLayerCutoffs.Length; i++)
        {
            tmp[i] = new Texture2D(sizeX, sizeY);
        }
       
        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
               
                baseAlt = Equations.cloudBase(grid[i, j].temp, Equations.dewPoint(grid[i, j].temp, grid[i, j].humid));
                if (baseAlt < altLayerCutoffs[0])
                {
                    tmp[0].SetPixel(i, j, Color.white);  
                }else if (baseAlt < altLayerCutoffs[1])
                {
                    tmp[1].SetPixel(i, j, Color.white);
                }
                else if (baseAlt < altLayerCutoffs[2])
                {
                    tmp[2].SetPixel(i, j, Color.white);
                }
                else if (baseAlt < altLayerCutoffs[3])
                {
                    tmp[3].SetPixel(i, j, Color.white);
                }
                else if (baseAlt < altLayerCutoffs[4])
                {
                    tmp[4].SetPixel(i, j, Color.white);
                }
            }
        }
        for (int i = 0; i < altLayerCutoffs.Length; i++)
        {
            tmp[i].Apply();
            //cloudL[i].inner.material.mainTexture = layerTex[l + i];
           // cloudL[i].outer.material.mainTexture = layerTex[l + i];

            SaveTextureAsPNG(tmp[i], "/Clouds/"+(char)(65+i)+setID+".png");
        }
    }

    public void SaveTextureAsPNG(Texture2D _texture, string _fullPath)
         {
             byte[] _bytes =_texture.EncodeToPNG();
             System.IO.File.WriteAllBytes(path+_fullPath, _bytes);
             Debug.Log(_bytes.Length/1024  + "Kb was saved as: " + _fullPath);
         }

    // Update is called once per frame
    void Update()
    {
        
    }
}
