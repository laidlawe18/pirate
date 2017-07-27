using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Map : NetworkBehaviour {

    public float scale;
    public Color[] colors;
    public Color[] minimapColors;
    public int width;
    public int height;
    public int pixelSize;
    int[,] map;
    public int landCutoff = 3;
    public float buildingCutoff;
    public GameObject island;

    List<Vector2[]> paths;

    public Island[] islands;

    [SyncVar]
    Vector2 offset;

    [SyncVar]
    int numIslands;

    void Awake()
    {
        if (NetworkServer.active)
        {
            offset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));
        } else
        {
            GameManager.instance.map = this;
        }
    }

    // Use this for initialization
    public void Init () {
        print(offset);
        print(numIslands);
        Texture2D tex = new Texture2D(width, height);
        Texture2D colTex = new Texture2D(width, height);
        map = new int[width / pixelSize , height / pixelSize];
        
        

        tex.filterMode = FilterMode.Point;
        
        for (float i = 0; i < width; i++)
        {
            for (float j = 0; j < height; j++)
            {
                
                tex.SetPixel((int) i, (int) j, colors[(int) (Mathf.PerlinNoise((int) (i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length]);
                colTex.SetPixel((int)i, (int)j, (int)(Mathf.PerlinNoise((int)(i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length < landCutoff ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0));
                map[(int) i / pixelSize, (int) j / pixelSize] = (int) (Mathf.PerlinNoise((int)(i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length;
            }
        }
        
        tex.Apply();
        colTex.Apply();
        Sprite sprite = Sprite.Create(colTex, new Rect(0, 0, width, height), new Vector2(.5f, .5f));
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D pc = gameObject.AddComponent<PolygonCollider2D>();
        if (NetworkServer.active)
        {
            for (int i = 0; i < pc.pathCount; i++)
            {
                GameObject newIsland = Instantiate(island, pc.GetPath(i)[0], Quaternion.identity);
                
                Island newIslandScript = newIsland.GetComponent<Island>();
                newIslandScript.SetResourceMult(Mathf.PerlinNoise(pc.GetPath(i)[0].x * 100 / scale + offset.x, pc.GetPath(i)[0].y * 100 / scale + offset.y));
                newIslandScript.islandID = i;
                NetworkServer.Spawn(newIsland);
            }
            Destroy(pc);
        }
        numIslands = pc.pathCount;
        paths = new List<Vector2[]>();
        for (int i = 0; i < pc.pathCount; i++)
        {
            Vector2[] vecs = pc.GetPath(i);
            Vector2 pos = vecs[0];
            for (int j = 0; j < vecs.Length; j++)
            {
                vecs[j] -= pos;
            }
            paths.Add(vecs);
        }
        Destroy(pc);
        islands = new Island[numIslands];
        sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(.5f, .5f));
        sr.sprite = sprite;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public float GetElevation(float x, float y)
    {
        return Mathf.PerlinNoise((x * 100 + width / 2) / scale + offset.x, (y * 100 + height / 2) / scale + offset.y);
    }

    public bool SafeForBuilding(float x, float y)
    {
        return GetElevation(x, y) < buildingCutoff / (float) colors.Length;
    }

    public Sprite GetMinimapSprite(int miniWidth)
    {
        float miniScale = (float)miniWidth / width;
        int miniHeight = (int)(miniScale * height);

        Texture2D miniTex = new Texture2D(miniWidth, miniHeight);
        int miniPixelSize = (int)(pixelSize / miniScale / 1.5);
        for (float i = 0; i < width; i++)
        {
            for (float j = 0; j < height; j++)
            {
                miniTex.SetPixel((int)(i * miniScale), (int)(j * miniScale), minimapColors[(int)(Mathf.PerlinNoise((int)(i / miniPixelSize) * miniPixelSize / scale + offset.x, (int)(j / miniPixelSize) * miniPixelSize / scale + offset.y) * minimapColors.Length) % minimapColors.Length]);
            }
        }
        miniTex.Apply();
        return Sprite.Create(miniTex, new Rect(0, 0, miniWidth, miniHeight), new Vector2(.5f, .5f));
    }

    public Vector2 SetupMinimapCamera(Camera minimapCamera, int miniWidth)
    {
        minimapCamera.aspect = (float)width / height;
        minimapCamera.orthographicSize = (float)height / 200f;
        float miniScale = (float)miniWidth / width;
        int miniHeight = (int)(miniScale * height);
        return new Vector2(miniWidth, miniHeight);
    }

    public Island GetIslandByID(int id)
    {
        return islands[id];
    }

    public Vector2[] SetIslandByID(int id, Island i)
    {
        islands[id] = i;
        return paths[id];
    }
}
