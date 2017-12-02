using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow
{
    private Map map;
    private Texture2D walkableTexture;
    private Texture2D obstacleTexture;

    GUIStyle walkableStyle;
    GUIStyle obstaclesStyle;

    private bool pendingToRecreateMap = false;
    private MapLoader mapLoader;

    private void OnEnable()
    {
        mapLoader = FindObjectOfType<MapLoader>();

        minSize = new Vector2(350, 300);

        //titleContent.text = "Map editor";
        walkableTexture = new Texture2D(10, 10);
        Color[] cols = new Color[10 * 10];
        for (int i = 0; i < 100; ++i)
        {
            int x = (int)i % 10;
            int y = (int)i / 10;
            if (x == 0 || x == 9)
                cols[i] = new Color(0, 0, 0, 1);
            else if (y == 0 || y == 9)
                cols[i] = new Color(0, 0, 0, 1);
            else
                cols[i] = new Color(0, 1, 0, 1);
        }
        walkableTexture.SetPixels(cols);

        obstacleTexture = new Texture2D(10, 10);
        cols = new Color[10 * 10];
        for (int i = 0; i < 100; ++i)
        {
            int x = (int)i % 10;
            int y = (int)i / 10;
            if (x == 0 || x == 9)
                cols[i] = new Color(0, 0, 0, 1);
            else if (y == 0 || y == 9)
                cols[i] = new Color(0, 0, 0, 1);
            else
                cols[i] = new Color(1, 0, 0, 1);
        }
        obstacleTexture.SetPixels(cols);

        walkableStyle = new GUIStyle();
        walkableStyle.normal.background = walkableTexture;
        walkableStyle.alignment = TextAnchor.MiddleCenter;
        walkableStyle.border = new RectOffset(1, 1, 1, 1);

        obstaclesStyle = new GUIStyle();
        obstaclesStyle.normal.background = obstacleTexture;
        obstaclesStyle.alignment = TextAnchor.MiddleCenter;
        obstaclesStyle.border = new RectOffset(1, 1, 1, 1);


    }

    void OnGUI()
    {
        if (map != null)
        {
            Event ev = Event.current;
            if (pendingToRecreateMap && ev.type != EventType.Repaint)
                HandleUtility.Repaint();

            Display(ev);
        }
    }

    //---------------------------------------

    public void SetMap(Map map)
    {
        if (map != null)
            this.map = map;
    }

    void Display(Event ev)
    {
        GUILayout.BeginArea(new Rect(0, 0, 300, 40));
        GUILayout.BeginHorizontal();

        GUILayout.Label(string.Format("Width: {0}", map.Width));
        GUILayout.Label(string.Format("Height: {0}", map.Height));

        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        Rect editRect = new Rect(40, 40, 25 * map.Width, 25 * map.Height);

        GUILayout.BeginArea(editRect);
        for (int y = 0; y < map.Height; ++y)
        {
            GUILayout.BeginHorizontal();

            for (int x = 0; x < map.Width; ++x)
            {
                int value = map.visualInfo.data[y * map.Width + x];
                GUILayout.Box(value.ToString(), value == 0 ? walkableStyle : obstaclesStyle);
                if (ev.type == EventType.MouseDown && ev.button == 0 && ev.modifiers == EventModifiers.None)
                {
                    if (GUILayoutUtility.GetLastRect().Contains(ev.mousePosition))
                    {
                        map.visualInfo.data[y * map.Width + x] = value == 0 ? 1 : 0;
                        pendingToRecreateMap = true;
                    }
                }
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(5, position.height - 40, position.width - 10, 40));

        if (GUILayout.Button("Save changes and recreate map"))
        {
            if (pendingToRecreateMap)
            {
                Undo.RecordObject(map, "Save map changes and recreate map");
            }
            mapLoader.SaveMap();
            mapLoader.RecreateMap();
            pendingToRecreateMap = false;
        }

        GUILayout.EndArea();
    }

}
