using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataAccess : MonoBehaviour
{
    [Header("Store Cell Data")]
    public string filename = "log.txt";

    //private string root = "CellData/";

    // Start is called before the first frame update
    void Start()
    {
        CreateFile();
        // ReadFile();
        WriteFile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateFile()
    {
  
        if (!File.Exists(filename))
        {
            File.Create(filename).Close();
        }

        Debug.Log("Creating file... I created a file!");
    }

    private void ReadFile()
    {
        using (StreamReader reader = new StreamReader(filename))
        {
            while (!reader.EndOfStream)//while we haven't reached the end of file...
            {
                //keep reading
                Debug.Log(reader.ReadLine());
            }
        }
    }
      
    private void WriteFile()
    {
        using(StreamWriter writer = new StreamWriter(filename))
        {
            for(int i=0; i<10; i++)
            {
                writer.WriteLine(i.ToString());
            }
        }
    }
}

