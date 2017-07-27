using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour {

    public static GameManager instance;


    public List<Player> players;
    public List<Selectable> selectables;
    public Player localPlayer;
    public GameObject mapPrefab;
    public Map map;
    public UIManager ui;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
    }

    void Start()
    {
        Debug.Log("Game manager start");
        Debug.Log("Map: " + map);
        players = new List<Player>();
        selectables = new List<Selectable>();
        if (NetworkServer.active)
        {
            GameObject newMap = Instantiate(mapPrefab);
            NetworkServer.Spawn(newMap);
            map = newMap.GetComponent<Map>();
        }
        ui.gameObject.SetActive(true);
        map.Init();
        Camera.main.GetComponent<CameraMovement>().setBounds(map.width / 200f, map.height / 200f);
        if (isClient)
        {
            ui.InitUI();
        }
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}

    public int AddPlayer(Player p)
    {
        players.Add(p);
        return players.Count;
    }

    public void AddSelectable(Selectable s)
    {
        selectables.Add(s);
    }
    
}
