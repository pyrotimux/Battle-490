using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

public class ProtoContrl : NetworkBehaviour {
    private Vector3 hitpos;
    public GameObject movarea, moveablearea;
    public GameObject[] toons;
    public ProtoMove sltobj;
    private GameObject canvas;
    private bool setal = false; 


    [SyncVar]
    public string pname = "Player 1";

    [SyncVar]
    public Color pcolor = Color.white;

    [SyncVar]
    public bool myturn = false; 

    [Command]
    void CmdSpawn(string pname)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject curtoon = (GameObject)Instantiate(toons[i], transform.position + transform.forward * 5 * (i+1) + transform.up * 1, Quaternion.identity);
            NetworkServer.Spawn(curtoon);
            ProtoMove m = curtoon.GetComponent<ProtoMove>();
            m.owner = pname; m.startpos = (i * 2) + 1;
        }
    }

    [Command]
    void CmdMove(Vector3 hit)
    {
        sltobj.moveto = hit;
        sltobj.canmove = false;
    }

    [Command]
    void CmdSelected(GameObject slt ,bool b)
    {
        sltobj = slt.GetComponent<ProtoMove>();
        sltobj.selected = b;
    }

    [Command]
    void CmdDeselected(bool b)
    {
        sltobj.selected = b;
    }

    [Command]
    void CmdSpawnMat()
    {
        GameObject m  = (GameObject)Instantiate(movarea, new Vector3(sltobj.transform.position.x, -0.57f, sltobj.transform.position.z), Quaternion.identity);
        NetworkServer.Spawn(m);
        moveablearea = m;
    }

    [Command]
    void CmdDestroyMat()
    {
        GameObject[] grounds = GameObject.FindGameObjectsWithTag("moveablegrounds");
        foreach (GameObject g in grounds)
        {
            NetworkServer.Destroy(g);
        }
    }

    [Command]
    void CmdEndTurn()
    {
        myturn = false;
    }

    void Start () {
        if (isLocalPlayer)
        {
            Camera.main.transform.position = this.transform.position;
            Camera.main.transform.rotation = this.transform.rotation;
            CmdSpawn(pname);
            canvas = GameObject.Find("Canvas");
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;
        if (!myturn) { canvas.SetActive(false); setal = false; return; } else if(!setal) { canvas.SetActive(true); setal = true; }
        
        if (CrossPlatformInputManager.GetButtonDown("endturn")) CmdEndTurn();
        

        if (CrossPlatformInputManager.GetButton("Fire1")) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.StartsWith("toon"))
                {
                    if (moveablearea) CmdDestroyMat();
                    if (sltobj) CmdDeselected(false);
                    sltobj = hit.transform.GetComponent<ProtoMove>();
                    if (sltobj.owner == pname && sltobj.canmove) {
                        if (sltobj) CmdSelected(hit.transform.gameObject, true);

                        CmdSpawnMat();
                    }
                    
                }
                else if(hit.transform.name.StartsWith("moveablearea") && sltobj.canmove) 
                {
                    CmdMove(hit.point);
                    CmdDestroyMat();

                }

            }

        }
    }
}
