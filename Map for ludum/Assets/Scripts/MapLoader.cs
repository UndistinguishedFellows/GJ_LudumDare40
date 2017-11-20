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
        map.tiles = new Tile[map.visualInfo.width, map.visualInfo.height]; //Dont really like to do this here but cant do it on OnMapLoad

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

                Vector3 spawnPos = new Vector3(x - (map.visualInfo.width / 2) + (transform.localScale.x / 2), -y + (map.visualInfo.height / 2) - (transform.localScale.y / 2), 0);
                GameObject obj = Instantiate(pref, spawnPos, Quaternion.identity, mapObject.transform);
                
                //Add tiles into map tiles array
                Tile tile = obj.GetComponent<Tile>();
                if (tile != null)
                {
                    map.tiles[x, y] = tile;
                    tile.SetTile(x, y);
                }
                else Debug.LogError(string.Format("Tile {0}, {1} has no Tile script added.", x, y));
            }
        }
        
        map.OnMapLoaded();
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
