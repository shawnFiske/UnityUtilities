using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

public static class SceneUtils {


	public static JSONNode getSceneList(){
		int numScenes = SceneManager.sceneCount;
		string scenelist = "{\"scenes\":[";

		for(var i = 0; i < numScenes; i++) {
			if(i < numScenes - 1) {
				scenelist += "\"" + SceneManager.GetSceneAt(i).name  + "\",";
			}else{
				scenelist += "\"" + SceneManager.GetSceneAt(i).name  + "\"]";
			}
		}

		scenelist += "}";


		return JSON.Parse(scenelist);
	}

	public static int getTotalSceneCount(){
		return SceneManager.sceneCountInBuildSettings;
	}

	public static Scene createNewScene(string name){
		return SceneManager.CreateScene(name);
	}

	public static Scene getCurrentScene() {
		return SceneManager.GetActiveScene();
	}
}
