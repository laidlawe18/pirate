using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public float scale;
    public Color[] colors;
    public int width;
    public int height;
    public int pixelSize;
    int[,] map;
    public int landCutoff = 3;

	// Use this for initialization
	void Start () {
        Texture2D tex = new Texture2D(width, height);
        Texture2D colTex = new Texture2D(width, height);
        map = new int[width / pixelSize , height / pixelSize];
        Vector2 offset = new Vector2(Random.Range(0, 100000), Random.Range(0, 100000));

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
        gameObject.AddComponent<PolygonCollider2D>();

        sprite = Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(.5f, .5f));
        sr.sprite = sprite;

    }

    bool CanVisit(int i, int j, bool[,] visited)
    {
        if (i >= 0 && j >= 0 && i < visited.GetLength(0) && j < visited.GetLength(1) && !visited[i, j])
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
