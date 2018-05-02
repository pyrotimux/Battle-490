using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
/// this class handles control protocol of players
/// </summary>
/// 
public class ProtoContrl : NetworkBehaviour {
    private Vector3 hitpos; // where raycast hit occur
    public GameObject moveablearea; // movablearea stuff
    public GameObject attackingarea; // attacking stuff
    public bool areaset = false;
    public GameObject[] toons; // toons that i control
    public GameObject sltobj;
    public ProtoMove sltpm; // obj that is currently selected
    private GameObject canvas; // this show gui if it's my turn
    private bool setal = false;  // limit painting gui over and over again.
    public ProtoHandlers pbut;
    public int playerScore = 0; // save the player score here
    public GameObject projectile;
    public bool gameover = false;

    [SyncVar]
    public bool winning = false;

    [SyncVar]
    public string pname = "Player 1"; // network player name 

    [SyncVar]
    public Color pcolor = Color.red; // network player color 

    [SyncVar]
    public bool myturn = false; // define if player turn or not


    /// <summary>spawn toons</summary>
    /// <param name="pname"> takes player name </param>
    [Command]
    void CmdSpawn(string pname)
    {
        for (int i = 0; i < toons.Length; i++)
        {
            GameObject curtoon = (GameObject)Instantiate(toons[i], transform.position + transform.forward * 5 * (i+1) + transform.up * 5, Quaternion.identity);
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
        sltpm.moveto = hit;
    }
    

    [Command]
    public void CmdAttack(Vector3 hit)
    {
        sltpm.canattack = false;
        // sltpm.moveto = hit;
        GameObject col = (GameObject)Instantiate(projectile, sltpm.GetTransform().position + transform.up * 1.0f, Quaternion.identity);
        ProjectileAcclerate projacc = col.GetComponent<ProjectileAcclerate>();
        projacc.owner = pname;
        projacc.type = sltpm.type;
        col.transform.LookAt(hit);
        NetworkServer.Spawn(col);

    }

    /// <summary>select toons</summary>
    /// <param name="slt"> selected game object  </param>
    /// <param name="b"> bool that select or not  </param>
    [Command]
    void CmdPlayerSelected(GameObject go)
    {
        sltpm = go.GetComponent<ProtoMove>();
        sltpm.selected = true;
        RpcPlayerSelected(go);
    }

    [ClientRpc]
    void RpcPlayerSelected(GameObject go)
    {
        sltpm = go.GetComponent<ProtoMove>();
        sltpm.selected = true;
    }

    [Command]
    void CmdPlayerDeselected()
    {
        sltpm.selected = false;
        RpcPlayerDeselected();
    }

    [ClientRpc]
    void RpcPlayerDeselected()
    {
        sltpm.selected = false;
    }


    /// <summary>spawn moveable area</summary>
    /// <param name="mat"> the material that wanted to be spawned (?) </param>
    [Command]
    void CmdSpawnMoveArea()
    {
        GameObject m  = (GameObject)Instantiate(moveablearea, new Vector3(sltpm.transform.position.x, -0.57f, sltpm.transform.position.z), Quaternion.identity);
        NetworkServer.Spawn(m);
        areaset = true;
    }

    /// <summary>spawn moveable area</summary>
    /// <param name="mat"> the material that wanted to be spawned (?) </param>
    [Command]
    void CmdSpawnAttackArea()
    {
        GameObject m = (GameObject)Instantiate(attackingarea, new Vector3(sltpm.transform.position.x, -0.57f, sltpm.transform.position.z), Quaternion.identity);
        NetworkServer.Spawn(m);
        areaset = true;
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
            //Camera.main.transform.position = this.transform.position;
            //Camera.main.transform.rotation = this.transform.rotation;

            //StartCoroutine(DelayCamAttach(2));

            CmdSpawn(pname); // then ask the server to spawn toons for me.
            canvas = GameObject.Find("Canvas");
            pbut = GameObject.Find("pButtonHandler").GetComponent<ProtoHandlers>();
        }

        
    }

    public IEnumerator DelayDestroy(float time, GameObject go)
    {
        yield return new WaitForSeconds(time);
        CmdDestroyGameObject(go);

    }

    public IEnumerator DelayCamAttach(float time)
    {
        yield return new WaitForSeconds(time);

        transform.rotation = Quaternion.EulerRotation(0, Camera.main.transform.rotation.y, 0);
        Camera.main.transform.parent = this.transform;

    }

    // Update is called once per frame
    void Update () {
        if (!isLocalPlayer) return; // if i am not local player then get out of here.

        // activating the "You Win/You Lose" screen at the end of the game
        if (gameover) {
            if (winning)
            {
                Debug.Log("You Won!");
                GameObject.Find("WinGameOver").SetActive(true);
            }
            else
            {
                Debug.Log("You Lose!");
                GameObject.Find("LoseGameOver").SetActive(true);
            }
        }

        if (Input.GetButton("up")) //if we click on ui button move (?)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        else if (Input.GetButton("down")) // adding 1 more button (down = s) for camera to move backwards
        {
            transform.Translate(Vector3.back * Time.deltaTime * 10);
        }
        else if (Input.GetButton("left"))
        {
            //transform.rotation *= Quaternion.EulerRotation(0, -0.1f, 0);
            transform.rotation *= Quaternion.Euler(0, -5f, 0);
        }
        else if (Input.GetButton("right"))
        {
            //transform.rotation *= Quaternion.EulerRotation(0, 0.1f, 0);
            transform.rotation *= Quaternion.Euler(0, 5f, 0);
        }

        // if it's not my turn the dont show gui component.
        if (!myturn) {
            canvas.SetActive(false);
            setal = false;
            return;
        }
        else if (!setal) {
            canvas.SetActive(true);
            setal = true;
        }


        // if player press the endturn button then end player turn
        if (Input.GetButtonDown("endturn") || pbut.enddown) {
            CmdPlayerDeselected();
            CmdDestroyMat();
            CmdEndTurn();
            pbut.enddown = false; //unpress button
        }

        if (Input.GetButton("move") || pbut.movedown) //if we click on ui button move (?)
        {
            if (sltpm.canmove)
            {
                CmdSpawnMoveArea(); // spawn moveable area
                pbut.movedown = false;
            }
                
        }

        if (Input.GetButton("attack") || pbut.attackdown) //if we click on ui button attack (?)
        {
            if (sltpm.canattack)
            {
                CmdSpawnAttackArea(); //spawn attacking area (?)
                pbut.attackdown = false;
            }
                
        }


        // if player press fire1 / left mouse  then cast ray from screen
        if (Input.GetButton("Fire1")) {
            Debug.Log("Hello ");
            RaycastHit hit; 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) // if ray cast hit a collider then 
            {
                if (hit.transform.name.StartsWith("toon")) // if i click on toons
                {
                    if (areaset) CmdDestroyMat(); // destroy previous spawn move areas
                    if (sltpm) CmdPlayerDeselected(); // deselect all toons

                    ProtoMove hitpm = hit.transform.GetComponent<ProtoMove>(); // get the new selected toon so i can compare

                    if (hitpm.owner == pname && (hitpm.canmove || hitpm.canattack)) { // if it is a toon that i can control
                        CmdPlayerSelected(hit.transform.gameObject); // then select that toon.  
                    }
                    else if((hitpm.owner != pname) && hitpm.canbeattack){
                        bool isBase = hit.transform.name.StartsWith("toonbase");

                        if (isBase)
                            if (playerScore >= 490) {
                                winning = true;
                                CmdAttack(hitpm.moveto);
                            } else
                                Debug.Log("You do not have enough power to destroy a base.");
                        else if (!isBase)
                            CmdAttack(hitpm.moveto);

                        // CmdDestroyGameObject(hitpm.gameObject);
                    }
                    
                }
                else if(hit.transform.name.StartsWith("moveablearea") && sltpm.canmove) // if i am clicking on moveable area and my toon can move
                {
                    CmdMove(hit.point); // move the toon
                }
                CmdDestroyMat(); // destroy areas.

            }

        }

        
    }
}
