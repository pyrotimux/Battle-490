using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMgr : NetworkBehaviour {
    public Transform[] spwnpos, mutexpos;
    public GameObject[] collectprefab; // collectables prefab (keyboard, monitor, laptop and mouse)
    public GameObject mutexprefab; // multiplier (for getting new toon) prefab (VintagePC)
    public GameObject[] playerprefab;
    private int countcol = 0, countmutx = 0;


    private Transform toonpos;
    private string toonowner = "";
    private int toontype = 0;
    private Color tooncolor = Color.white;

	// Use this for initialization
	void Start () {
        CmdCollectSpawn();
	}

    private void LateUpdate()
    {
        if (countcol == 2000)
        {
            CmdCollectSpawn();
            countcol = 0;
        }
        else {
            countcol++;
        }

        if (countmutx == 5000)
        {
            CmdSpCollectSpawn();
            countmutx = 0;
        }
        else
        {
            countmutx++;
        }

    }

    // spawn collectables (should be keyboard, laptop, monitor or mouse)
    [Command]
    public void CmdCollectSpawn() {
        GameObject col = (GameObject)Instantiate(collectprefab[Random.Range(0, collectprefab.Length)], spwnpos[Random.Range(0, spwnpos.Length)].position, Quaternion.identity);
        NetworkServer.Spawn(col);
    }

    // spawn new toon (multiplier) (should be VintagePC)
    [Command]
    public void CmdSpCollectSpawn()
    {
        GameObject col = (GameObject)Instantiate(mutexprefab, mutexpos[Random.Range(0, mutexpos.Length)].position, Quaternion.identity);
        NetworkServer.Spawn(col);
    }

    public void SpawnPlayer(Transform toonpos, int toontype, string toonowner, Color tooncolor) {
        this.toonpos = toonpos;
        this.toontype = toontype;
        this.toonowner = toonowner;
        this.tooncolor = tooncolor;
        CmdPlayerSpawn();
    }


    [Command]
    public void CmdPlayerSpawn()
    {
        GameObject player = (GameObject)Instantiate(playerprefab[toontype - 1], toonpos.position, Quaternion.identity);
        ProtoMove pm = player.GetComponent<ProtoMove>();
        pm.owner = toonowner;
        pm.type = toontype;
        pm.pcolor = tooncolor;
        NetworkServer.Spawn(player);
    }

}
