using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapLoader : MonoBehaviour {

    public GameObject mapObject;

    public string mapToLoad;
    public GameObject[] walkables;
    public GameObject[] obstacles;

    //-------------------------------------

    void Start ()
    {
        LoadMap();
	}

    //-------------------------------------

    public void LoadMap()
    {
        ClearMap();

        string path = Application.streamingAssetsPath + "/" + mapToLoad + ".json";
        string raw = File.ReadAllText(path);

        Map map = mapObject.GetComponent<Map>();
        if (map != null)
        {
            map.visualInfo = JsonUtility.FromJson<MapVisualInfo>(raw);
            map.OnMapVisualInfoLoaded();

            CreateMap(map);

            map.OnMapLoaded();
        }
    }

    public void ClearMap()
    {
        var childs = new List<Transform>();
        foreach (Transform transform in mapObject.transform)
        {
            childs.Add(transform);
        }
        childs.ForEach(child => DestroyImmediate(child.gameObject));
    }

    public void SaveMap()
    {
        string path = Application.streamingAssetsPath + "/" + mapToLoad + ".json";
        Map map = mapObject.GetComponent<Map>();
        string raw = JsonUtility.ToJson(map.visualInfo);

        File.WriteAllText(path, raw);
    }

    public void RecreateMap()
    {
        ClearMap();
        CreateMap(mapObject.GetComponent<Map>());
    }

    void CreateMap(Map map)
    {
        for(int x = 0; x < map.Width; ++x)
        {
            for(int y = 0; y < map.Height; ++y)
            {
                int value = map.visualInfo.data[y * map.Width + x];
                GameObject prefab = null;

                if (value == 0)
                    prefab = walkables[Random.Range(0, walkables.Length)];
                else if (value == 1)
                    prefab = obstacles[Random.Range(0, obstacles.Length)];

                if(prefab != null)
                {
                    Vector3 spawn = new Vector3(x - (map.Width / 2) + (transform.localScale.x / 2),
                        -y + (map.Height / 2) - (transform.localScale.y / 2),
                        0);

                    GameObject obj = Instantiate(prefab, spawn, Quaternion.identity, mapObject.transform);

                    if(obj != null)
                    {
                        Node node = obj.GetComponent<Node>();
                        if(node != null)
                        {
                            map.nodes[x, y] = node;
                            node.SetNode(x, y);
                        }
                    }
                }
            }
        }
    }
}
