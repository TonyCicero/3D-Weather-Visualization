
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using SQLite4Unity3d;

public class SqliteTest : MonoBehaviour
{

	
    // Start is called before the first frame update
    void Start()
    {
        SQLiteConnection dbconnection = new SQLiteConnection(Application.dataPath + "/resources/dbfile", false);
            dbconnection.DropTable<Person> ();
            dbconnection.CreateTable<Person> ();

            dbconnection.InsertAll (new[]{
                new Person{
                    Id = 1,
                    Name = "Peter",
                    Surname = "Parker",
                    Age = 56
                },
                new Person{
                    Id = 2,
                    Name = "Roberto",
                    Surname = "Arthurson",
                    Age = 16
                },
                new Person{
                    Id = 3,
                    Name = "John",
                    Surname = "Doe",
                    Age = 25
                },
                new Person{
                    Id = 4,
                    Name = "Roberto",
                    Surname = "Huertas",
                    Age = 37
                }
            });
            Debug.Log("Hello");
            Debug.Log(dbconnection.Query<Person>("SELECT * FROM Person")[0]);
            
            
           
            
            
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
