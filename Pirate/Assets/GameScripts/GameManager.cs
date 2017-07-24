using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public List<Player> players;
    public List<Selectable> selectables;
    public Player localPlayer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Use this for initialization
    void Start () {
		
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
