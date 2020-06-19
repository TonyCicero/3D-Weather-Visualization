using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SQLite4Unity3d;

public class WeatherConnection : MonoBehaviour
{
    public string dbfile = "/Database/WorldData.db";
    public LayerGen layerGen;
    // Start is called before the first frame update
    void Start()
    {
        //loadFromDB();

    }

    public void changePath(string path)
    {
        dbfile = path;
    }

    public void loadFromDB()
    {
        //string dbfile = Application.dataPath + "/Database/WorldData.db";
        SQLiteConnection dbconnection = new SQLiteConnection(dbfile, false);
        List<Weather> database = dbconnection.Query<Weather>("SELECT * FROM Weather");
        layerGen.InsertData(database);
    }
}

