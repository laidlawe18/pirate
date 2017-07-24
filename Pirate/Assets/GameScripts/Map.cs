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

    [SyncVar]
    Vector2 offset;
    

	// Use this for initialization
	void Start () {
        Texture2D tex = new Texture2D(width, height);
        Texture2D colTex = new Texture2D(width, height);
        map = new int[width / pixelSize , height / pixelSize];
        if (isServer)
        {
            offset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));
        }

        
        /*GameObject minimap = GameObject.Find("Minimap");
        int miniWidth = (int)minimap.GetComponent<RectTransform>().sizeDelta.x;
        float miniScale = (float) miniWidth / width;
        int miniHeight = (int)(miniScale * height);
        minimap.GetComponent<RectTransform>().sizeDelta = new Vector2 (miniWidth, miniHeight);
        minimap.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(miniWidth + 30, miniHeight + 30);
        minimap.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(miniWidth / 2 + 25, miniHeight / 2 + 25);

        Camera minimapCamera = GameObject.Find("Minimap Camera").GetComponent<Camera>();

        minimapCamera.aspect = (float)width / height;
        minimapCamera.orthographicSize = (float) height / 200f;
        minimapCamera.targetTexture.width = miniWidth;
        minimapCamera.targetTexture.height = miniHeight;
        GameObject minimapFromCamera = GameObject.Find("Minimap From Camera");
        minimapFromCamera.GetComponent<RectTransform>().sizeDelta = new Vector2(miniWidth, miniHeight);

        Texture2D miniTex = new Texture2D(miniWidth, miniHeight);
        int miniPixelSize = (int) (pixelSize / miniScale / 1.5);*/

        tex.filterMode = FilterMode.Point;
        
        for (float i = 0; i < width; i++)
        {
            for (float j = 0; j < height; j++)
            {
                //miniTex.SetPixel((int)(i*miniScale), (int)(j*miniScale), minimapColors[(int)(Mathf.PerlinNoise((int)(i / miniPixelSize) * miniPixelSize / scale + offset.x, (int)(j / miniPixelSize) * miniPixelSize / scale + offset.y) * minimapColors.Length) % minimapColors.Length]);
                tex.SetPixel((int) i, (int) j, colors[(int) (Mathf.PerlinNoise((int) (i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length]);
                colTex.SetPixel((int)i, (int)j, (int)(Mathf.PerlinNoise((int)(i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length < landCutoff ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0));
                map[(int) i / pixelSize, (int) j / pixelSize] = (int) (Mathf.PerlinNoise((int)(i / pixelSize) * pixelSize / scale + offset.x, (int)(j / pixelSize) * pixelSize / scale + offset.y) * colors.Length) % colors.Length;
            }
        }


        //miniTex.Apply();
        tex.Apply();
        colTex.Apply();
        Sprite sprite = Sprite.Create(colTex, new Rect(0, 0, width, height), new Vector2(.5f, .5f));
        //Sprite miniSprite = Sprite.Create(miniTex, new Rect(0, 0, miniWidth, miniHeight), new Vector2(.5f, .5f));
        //minimap.GetComponent<Image>().sprite = miniSprite;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = sprite;

        Destroy(GetComponent<PolygonCollider2D>());
        PolygonCollider2D pc = gameObject.AddComponent<PolygonCollider2D>();
        print(pc.GetPath(0).Length);
        for (int i = 0; i < pc.pathCount; i++)
        {
            GameObject newIsland = Instantiate(island, pc.GetPath(i)[0], Quaternion.identity, transform);
            newIsland.transform.parent = gameObject.transform;
            Vector2[] vecs = pc.GetPath(i);
            Vector2 pos = vecs[0];
            for (int j = 0; j < vecs.Length; j++)
            {
                vecs[j] -= pos;
            }
            newIsland.GetComponent<PolygonCollider2D>().SetPath(0, vecs);
            newIsland.GetComponent<Island>().SetResourceMult(Mathf.PerlinNoise(pos.x * 100 / scale + offset.x, pos.y * 100 / scale + offset.y));
        }
        Destroy(pc);
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
}
