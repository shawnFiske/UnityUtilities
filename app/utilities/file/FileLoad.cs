/*************************************************************
* Usage:
* string tempString = FileLoad.readFile(Application.dataPath + "/resources/test.json");
*
*************************************************************/

    using System.IO;  
    using System.Collections.Generic;
    using System.Collections;
    using UnityEngine;

public static class FileLoad {

    public static string readFile (string path) {
        try 
        {
            string line;
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader sr = new StreamReader(path)) 
            {
                // Read and display lines from the file until the end of 
                // the file is reached.
                //while ((line = sr.ReadToEnd()) != null) 
                //{
                line = sr.ReadToEnd();
                    //Debug.Log(line);
                //}
            }

            //Debug.Log("Ending: "+line);
            return line;
        }
        catch (System.Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
            return e.Message;
        }
    }

    public static string simpleReadFile (string path) {
        return File.ReadAllText(path);
    }
}