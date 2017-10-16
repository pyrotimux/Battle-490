using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProtoContrl : NetworkBehaviour {
    private Vector3 hitpos;
    private ProtoMove sltobj;
    public GameObject movarea, moveablearea;
    public GameObject[] toons;

    [SyncVar]
    public string pname = "Player 1";

    [SyncVar]
    public Color pcolor = Color.white;

    [SyncVar]
    public string playerstr = "1111111"; 

    void SpawnToons() {
        for (int i = 0; i < 3; i++) {
            GameObject curtoon = (GameObject)Instantiate(toons[i], transform.position + transform.forward * 5 + transform.up * 1, Quaternion.identity);
            NetworkServer.Spawn(curtoon);
            ProtoMove m = curtoon.GetComponent<ProtoMove>();
            if (isLocalPlayer) { m.owner = 1; m.startpos = (i * 2) + 1; }
        }
        
    }

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Camera.main.transform.position = this.transform.position;
            Camera.main.transform.rotation = this.transform.rotation;
            SpawnToons();
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (Input.GetMouseButtonDown(0) && playerstr[0].CompareTo('1') == 0) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.StartsWith("toon"))
                {
                    if (moveablearea) NetworkServer.Destroy(moveablearea);
                    if (sltobj) sltobj.selected = false;
                    sltobj = hit.transform.GetComponent<ProtoMove>();
                    if (sltobj.owner == 1 && playerstr[sltobj.startpos].CompareTo('1') == 0) {
                        sltobj.selected = true;
                        moveablearea = (GameObject)Instantiate(movarea, new Vector3(sltobj.transform.position.x, -0.57f, sltobj.transform.position.z), Quaternion.identity);
                        NetworkServer.Spawn(moveablearea);
                    }
                    
                }
                else if(hit.transform.name.StartsWith("moveablearea")) 
                {
                    sltobj.moveto = hit.point;
                    NetworkServer.Destroy(hit.transform.gameObject);
                    Debug.Log(playerstr);
                    playerstr = playerstr.Substring(0, sltobj.startpos) + "0" + playerstr.Substring(sltobj.startpos + 1);
                    Debug.Log(playerstr);
                }

            }

        }
    }
}
