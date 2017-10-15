using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProtoContrl : NetworkBehaviour {
    private Vector3 hitpos;
    private ProtoMove sltobj;
    public GameObject movarea;

    [SyncVar]
    public string playerstr = "1110000110000110000";

	// Use this for initialization
	void Start () {
        if (isLocalPlayer)
        {
            Camera.main.transform.position = this.transform.position;
            Camera.main.transform.rotation = this.transform.rotation;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name.StartsWith("p1"))
                {
                    if (sltobj) sltobj.selected = false;
                    sltobj = hit.transform.GetComponent<ProtoMove>();
                    sltobj.selected = true;
                    GameObject.Instantiate(movarea, new Vector3(sltobj.transform.position.x,-0.57f, sltobj.transform.position.z), Quaternion.identity);
                }
                else if(hit.transform.name.StartsWith("moveablearea")) 
                {
                    sltobj.moveto = hit.point;
                    Destroy(hit.transform.gameObject);
                }

            }

        }
    }
}
