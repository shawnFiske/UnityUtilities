/*************************************************************
* Usage:
* SimpleJSON.JSONNode JsonNode = ProcessJson.toObject(jsonString);
*
*************************************************************/

using UnityEngine;
using System.Collections.Generic;

public static class ProcessJson {
    public static List<string> listKeys(object jsonObj)
    {
        List<string> temp = new List<string>();

       /* foreach (KeyValuePair<string, object> kvp in jsonObj)
        {
            Debug.Log(kvp.Key);
            temp.Add(kvp.Key);
        }*/

        return temp;
    }
}