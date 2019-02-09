/*************************************************************
* Usage:
* AppData.Instance.addJsonNode(<key sting>, <JSONNode>);
* AppData.Instance.addJsonNode("test", Node);
* AppData.Instance.getJsonNodeByKey("test"); // returns JSONNode
* AppData.Instance.removeJsonNode("test"); // removes key and JSONNode
*
* AppData.Instance.addObject(<key sting>, <object>);
* AppData.Instance.addObject("test", Object); 
* AppData.Instance.getObjectByKey("test"); // returns object 
* AppData.Instance.removeObject("test"); // removes key and object
*
* AppData.Instance.addString(<key sting>, <string>);
* AppData.Instance.addString("test", "String"); 
* AppData.Instance.getStringByKey("test"); // returns String 
* AppData.Instance.removeString("test"); // removes key and String
*************************************************************/

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public class AppData : MonoBehaviour {
  public static AppData Instance;

//Dictionaries to contain various data searchable by keys
  private Dictionary<string, string> stringManager = new Dictionary<string, string>();
  private Dictionary<string, object> objectManager = new Dictionary<string, object>();
  private Dictionary<string, JSONNode> jsonManager = new Dictionary<string, JSONNode>();
  private Dictionary<string, Dictionary<string, string>> multiStringDictionaries = new Dictionary<string, Dictionary<string, string>>();

  private GameObject canvasObj;

  private Double lastTime = 0;
  private Double appTotalSeconds = 0;

  //world simulation
  private WorldEngine worldSim = new WorldEngine();
  
  void Awake ()   
  {
    if (Instance == null)
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }
    else if (Instance != this)
    {
        Destroy (gameObject);
    }
  }

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  void Update()
  {

    //Call Application component once every second
    if(lastTime < gameSecondTimer()){
      appTotalSeconds += 1;
      Debug.Log(appTotalSeconds +" : "+gameSecondTimer()+" : "+timeFormate());

    }

    //World Simulation
    worldSim.update();
  }

  private Double gameSecondTimer() {
    double currentTime = Math.Floor(Time.time);
    lastTime = currentTime;
    return currentTime;
  }

  private string timeFormate() {
    TimeSpan t = TimeSpan.FromSeconds( appTotalSeconds );
      string answer = string.Format("{0:D2}:{1:D2}:{2:D2}",
      t.Hours, 
      t.Minutes, 
      t.Seconds);

    return t.Days+" - "+answer;
  }

// Object dictionary methods
  public void addObject(string key, object obj) {
    objectManager[key] = obj;
  }

  public object getObjectByKey(string key) {
    return objectManager[key];
  }

  public void removeObject(string key) {
    objectManager.Remove(key);
  }

// JSON dictionary methods
  public void addJsonNode(string key, JSONNode node) {
    jsonManager[key] = node;
  }

  public JSONNode getJsonNodeByKey(string key) {
    return jsonManager[key];
  }

  public void removeJsonNode(string key) {
    jsonManager.Remove(key);
  }

// String dictionary methods
  public void addString(string key, string text) {
    stringManager[key] = text;
  }

  public string getStringByKey(string key) {
    return stringManager[key];
  }

  public void removeString(string key) {
    stringManager.Remove(key);
  }

  //multiple string dictionaries
  public void addStringDictionary(string key) {
    multiStringDictionaries[key] = stringManager;
  }

  public Dictionary<string, string> getStringDictionaryByKey(string key) {
    return multiStringDictionaries[key];
  }

  public void removeStringDictionary(string key) {
    multiStringDictionaries.Remove(key);
  }


  //read-write string dictionary to file.
  public void writeStringDictionary(string file){
    WriteDictionaryFile(stringManager, file);
  }

  public void readStringDictionary(string file){
    ReadDictionaryFile(file, stringManager);
  }

  public int getStringDictionaryItemCount() {
    return stringManager.Count;
  }

  public int getObjectDictionaryItemCount() {
    return objectManager.Count;
  }

  public int getJsonDictionaryItemCount() {
    return jsonManager.Count;
  }

  public void clearStringDictionary() {
    stringManager = new Dictionary<string, string>();
  }

  public void clearObjectDictionary() {
    objectManager = new Dictionary<string, object>();
  }

  public void clearJsonDictionary() {
    jsonManager = new Dictionary<string, JSONNode>();
  }

  // Canvas methods
  public void setCanvas(GameObject canvas) {
    canvasObj = canvas;
  }

  public GameObject getCanvas() {
    return canvasObj;
  }

  //Read-Write Binary files
  private void WriteDictionaryFile(Dictionary<string, string> dictionary, string file)
    {
        string path = Application.dataPath + "/resources/files/" + file;
        using(BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            // Put count.
            writer.Write(dictionary.Count);
            // Write pairs.
            foreach (var pair in dictionary)
            {
                writer.Write(pair.Key);
                writer.Write(pair.Value);
            }
        }
    }

    private void ReadDictionaryFile(string file, Dictionary<string, string> dictionary)
    {
        string path = Application.dataPath + "/resources/files/" + file;
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            // Get count.
            int count = reader.ReadInt32();
            // Read in all pairs.
            for (int i = 0; i < count; i++)
            {
                string key = reader.ReadString();
                string value = reader.ReadString();
                dictionary[key] = value;
            }
        }
    }

  //Sqlite methods
  public int GetRecordCount(string tableString, string dbName) {

    string conn = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;

    IDbConnection dbconn;
	  dbconn = new SqliteConnection(conn);
	  dbconn.Open();

    IDbCommand dbcmd = dbconn.CreateCommand();
    string sqlQuery = "SELECT count(*) FROM " + tableString;
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    int recordCount = 0;

    while(reader.Read()){
      recordCount = reader.GetInt32(0);
    }

    return recordCount;
  }

  //  Gets table data and returns it as a json string
  public string SqlRequestToJson(string sqlString, string tableString, string dbName) {
    string conn = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;

    IDbConnection dbconn;
	  dbconn = new SqliteConnection(conn);
	  dbconn.Open();

    IDbCommand dbcmd = dbconn.CreateCommand();
    string sqlQuery = sqlString;
    dbcmd.CommandText = sqlQuery;
    IDataReader reader = dbcmd.ExecuteReader();

    var jsonString = "{\"data\":{";
    bool startParse = true;
    //int recordCount = GetRecordCount(tableString, dbName);

    while (reader.Read())
    {
      
      for (int i = 0; i < reader.FieldCount; i++)
      { 
        if(reader.GetName(i) == "id") {

          if(startParse) {
            jsonString += "\"" + reader.GetValue(i) + "\"" + ":{";
            startParse = false;
          }else{
            jsonString += ",\"" + reader.GetValue(i) + "\"" + ":{";
          } 
          
        }else if (i == reader.FieldCount - 1){
          jsonString += "\"" + reader.GetName(i) + "\":\"" + reader.GetValue(i) + "\"";
        }else{
          jsonString += "\"" + reader.GetName(i) + "\":\"" + reader.GetValue(i) + "\",";
        }
        
      }

      jsonString += "}";

      //int id = reader.GetInt32(0);
      //string name = reader.GetString(1);
      //string hd = reader.GetString(2);
      
      //Debug.Log("id: " + id + "  name: " + name + "  hd: " +  hd + " records: " + recordCount);

    }

    jsonString += "}}";

    Debug.Log(jsonString);

    reader.Close();
    reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

    return jsonString;
  }

  public bool RunSQL(string sqlString, string dbName) {

    string conn = "URI=file:" + Application.streamingAssetsPath + "/" + dbName;

    IDbConnection dbconn;
	  dbconn = new SqliteConnection(conn);
	  dbconn.Open();

    IDbCommand dbcmd = dbconn.CreateCommand();
    string sqlQuery = sqlString;
    dbcmd.CommandText = sqlQuery;
    dbcmd.ExecuteNonQuery();
    //IDataReader reader = dbcmd.ExecuteReader();

    //reader.Close();
    //reader = null;
    dbcmd.Dispose();
    dbcmd = null;
    dbconn.Close();
    dbconn = null;

    return true;
  }
}