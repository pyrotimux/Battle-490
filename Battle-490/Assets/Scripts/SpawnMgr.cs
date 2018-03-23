using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnMgr : NetworkBehaviour {
    public Transform[] spwnpos;
    public GameObject collectprefab;
    private int counter = 0;

	// Use this for initialization
	void Start () {
        CmdCollectSpawn();
	}

    private void LateUpdate()
    {
        if (counter == 1000)
        {
            CmdCollectSpawn();
            counter = 0;
        }
        else {
            counter++;
        }
        
    }

    [Command]
    public void CmdCollectSpawn() {
        GameObject col = (GameObject)Instantiate(collectprefab, spwnpos[Random.Range(0,2)].position, Quaternion.identity);
        NetworkServer.Spawn(col);
    }
	

    

	
}
