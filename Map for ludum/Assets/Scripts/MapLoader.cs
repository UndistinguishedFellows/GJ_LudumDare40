using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MapLoader : MonoBehaviour {

	#region Variables

    public GameObject mapObject;

    public string mapToLoad;
    public GameObject[] walkables;
    public GameObject[] obstacles;

	#endregion

	#region UnityMethods

	void Start () 
	{
		LoadMap();
	}
	

	void Update () 
	{
		
	}

	#endregion

    public void LoadMap()
    {
        ClearMap();

        string path = Application.streamingAssetsPath + "/" + mapToLoad + ".json";
        string raw = File.ReadAllText(path);

        Map map = mapObject.GetComponent<Map>();
        map.visualInfo = JsonUtility.FromJson<MapVisualInfo>(raw);

        CreateMap(map);

        map.OnMapLoaded();
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

    public void CreateMap(Map map)
    {
        for (int x = 0; x < map.visualInfo.width; ++x)
        {
            for (int y = 0; y < map.visualInfo.height; ++y)
            {
                int value = map.visualInfo.data[y * map.visualInfo.width + x];
                GameObject pref = null;
                if (value == 0)
                {
                    pref = walkables[Random.Range(0, walkables.Length)];
                }
                else if (value == 1)
                {
                    pref = obstacles[Random.Range(0, obstacles.Length)];
                }

                Vector3 spawnPos = new Vector3(x - (map.Width / 2) + (transform.localScale.x / 2), -y + (map.Height / 2) - (transform.localScale.y / 2), 0);
                GameObject obj = Instantiate(pref, spawnPos, Quaternion.identity, mapObject.transform);
                //TODO: Get obj tile script and set neighbours
            }
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
}
