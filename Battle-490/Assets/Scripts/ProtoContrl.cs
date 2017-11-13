using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// this class handles control protocol of players
/// </summary>
/// 
public class ProtoContrl : NetworkBehaviour {
    private Vector3 hitpos; // where raycast hit occur
    public GameObject movarea, moveablearea; // movablearea stuff
    public GameObject atkarea, attackingarea; // attacking stuff
    public GameObject[] toons; // toons that i control
    public ProtoMove sltobj; // obj that is currently selected
    private GameObject canvas; // this show gui if it's my turn
    private bool setal = false;  // limit painting gui over and over again.


    [SyncVar]
    public string pname = "Player 1"; // network player name 

    [SyncVar]
    public Color pcolor = Color.white; // network player color 

    [SyncVar]
    public bool myturn = false; // define if player turn or not

    /// <summary>spawn toons</summary>
    /// <param name="pname"> takes player name </param>
    [Command]
    void CmdSpawn(string pname)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject curtoon = (GameObject)Instantiate(toons[i], transform.position + transform.forward * 5 * (i+1) + transform.up * 1, Quaternion.identity);
            NetworkServer.Spawn(curtoon);
            ProtoMove m = curtoon.GetComponent<ProtoMove>();
            m.owner = pname; m.pcolor = pcolor;
        }
    }

    /// <summary>move toons</summary>
    /// <param name="hit"> where to move  </param>
    [Command]
    void CmdMove(Vector3 hit)
    {
        sltobj.moveto = hit;
        
        
    }

    /// <summary>select toons</summary>
    /// <param name="slt"> selected game object  </param>
    /// <param name="b"> bool that select or not  </param>
    [Command]
    void CmdSelected(GameObject slt, bool b)
    {
        sltobj = slt.GetComponent<ProtoMove>();
        sltobj.selected = b;
    }

    /// <summary>deselect toons</summary>
    /// <param name="b"> bool that select or not  </param>
    [Command]
    void CmdDeselected(bool b)
    {
        sltobj.selected = b;
    }

    /// <summary>spawn moveable area</summary>
    /// <param name="mat"> the material that wanted to be spawned (?) </param>
    [Command]
    void CmdSpawnMat(GameObject mat)
    {
        GameObject m  = (GameObject)Instantiate(mat, new Vector3(sltobj.transform.position.x, -0.57f, sltobj.transform.position.z), Quaternion.identity);
        NetworkServer.Spawn(m);
        moveablearea = m;
    }

    /// <summary>destroy moveable area</summary>
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
    void CmdDestroyGameObject(GameObject go) {
        NetworkServer.Destroy(go);
    }

    /// <summary>end player turn.</summary>
    [Command]
    void CmdEndTurn()
    {
        myturn = false;
    }

    /// <summary>init</summary>
    void Start () {
        if (isLocalPlayer) 
        {
            // if i am local then i want a camera 
            Camera.main.transform.position = this.transform.position;
            Camera.main.transform.rotation = this.transform.rotation;
            CmdSpawn(pname); // then ask the server to spawn toons for me.
            canvas = GameObject.Find("Canvas");
        }

        
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return; // if i am not local player then get out of here.
        // if it's not my turn the dont show gui component.
        if (!myturn) { canvas.SetActive(false); setal = false; return; } else if(!setal) { canvas.SetActive(true); setal = true; }
        

        // if player press the endturn button then end player turn
        if (CrossPlatformInputManager.GetButtonDown("endturn")) CmdEndTurn();

        if (CrossPlatformInputManager.GetButton("move")) //if we click on ui button move (?)
        {
            if(sltobj.canmove)
                CmdSpawnMat(movarea); // spawn moveable area
        }

        if (CrossPlatformInputManager.GetButton("attack")) //if we click on ui button attack (?)
        {
            if(sltobj.canattack)
                CmdSpawnMat(atkarea); //spawn attacking area (?)
        }

        // if player press fire1 / left mouse  then cast ray from screen
        if (CrossPlatformInputManager.GetButton("Fire1")) {
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // if ray cast hit a collider then 
            {
                if (hit.transform.name.StartsWith("toon")) // if i click on toons
                {
                    if (moveablearea || attackingarea) CmdDestroyMat(); // destroy previous spawn move areas
                    if (sltobj) CmdDeselected(false); // deselect all toons
                    ProtoMove hitpm = hit.transform.GetComponent<ProtoMove>(); // get the new selected toon so i can compare

                    if (hitpm.owner == pname && (hitpm.canmove || hitpm.canattack)) { // if it is a toon that i can control
                        CmdSelected(hit.transform.gameObject, true); // then select that toon. 
                        
                    }else if(hitpm.owner != pname){
                        Vector3 temp = hitpm.moveto;
                        CmdDestroyGameObject(hitpm.gameObject);
                        CmdMove(temp);
                    }
                    
                }
                else if(hit.transform.name.StartsWith("moveablearea") && sltobj.canmove) // if i am clicking on moveable area and my toon can move
                {
                    CmdMove(hit.point); // move the toon
                    CmdDestroyMat(); // destroy moveable areas.

                }

            }

        }
    }
}
