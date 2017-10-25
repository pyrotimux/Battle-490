using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProtoMove : NetworkBehaviour {
    public bool selected;
    public int startpos = 0; 
    public int speed = 10;
    private bool delayed = true;

    [SyncVar]
    public Vector3 moveto;

    [SyncVar]
    public string owner = "";

    [SyncVar]
    public bool canattack = false;

    [SyncVar]
    public bool canmove = false;

    [SyncVar]
    public Color pcolor = Color.white;

    // Use this for initialization
    void Start () {
        StartCoroutine(DelayStart(2));
    }

    public IEnumerator DelayStart(float time)
    {
        yield return new WaitForSeconds(time);
        delayed = false;
        moveto = transform.position;

    }

    // Update is called once per frame
    void LateUpdate () {
        if (delayed) return;
        if (selected)
        {
            //GetComponent<Renderer>().material.color = Color.red;
            Renderer[] rends = gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.material.color = Color.red;
        }
        else {
            //GetComponent<Renderer>().material.color = Color.white;
            Renderer[] rends = gameObject.transform.GetChild(0).GetChild(0).GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rends)
                r.material.color = pcolor;
        }


        if (Vector3.Distance(transform.position, moveto) > 0.5f) {
            //transform.position = Vector3.Lerp(transform.position, movingto, Time.deltaTime);
            transform.LookAt(moveto);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        }
        else {
            transform.position = moveto;
        }
        
    }
}
