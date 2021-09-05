using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
[CustomEditor(typeof(LeaderboardManager))]
public class LeaderboardEditor : Editor
{
   public override void OnInspectorGUI(){
        LeaderboardManager m=(LeaderboardManager)target;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("JSON Parser");
        m.parser=(IParser)EditorGUILayout.ObjectField( m.parser,typeof(IParser),true);
        EditorGUILayout.EndHorizontal();

        m.serverIp=EditorGUILayout.TextField("Server Ip",m.serverIp);
        m.apiKey=EditorGUILayout.TextField("API Key",m.apiKey);
        m.gameId=EditorGUILayout.TextField("Game Id",m.gameId);
        m.gameKey=EditorGUILayout.TextField("Game Key",m.gameKey);


        if(m.originalGameKey==string.Empty||m.gameKey==string.Empty){
            if(GUILayout.Button("Generate Game Key")){
                m.GenerateThis();
            }
        }else{
            if(GUILayout.Button("Get Game Key")){
                m.GetThis();
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(m);
            EditorSceneManager.MarkSceneDirty(m.gameObject.scene);
        }
       
   }
}
