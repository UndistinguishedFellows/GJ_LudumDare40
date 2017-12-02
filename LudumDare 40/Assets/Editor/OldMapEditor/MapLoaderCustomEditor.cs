using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapLoader))]
public class MapLoaderCustomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapLoader mapLoader = target as MapLoader;
        base.OnInspectorGUI();

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Load map"))
            mapLoader.LoadMap();


        if (GUILayout.Button("Edit"))
        {
            MapEditorWindow win = EditorWindow.GetWindow<MapEditorWindow>("MapEditor");
            win.SetMap(mapLoader.mapObject.GetComponent<Map>());
        }

        if (GUILayout.Button("Clear map"))
        {
            mapLoader.ClearMap();
        }

        GUILayout.EndHorizontal();
    }

}