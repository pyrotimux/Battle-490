using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFunction : MonoBehaviour
{

    GameObject gomain, godirect, gomatch, goback, gobut;

    public void InitButton()
    {
        gomain = GameObject.Find("MainPanel");
        godirect = gomain.transform.GetChild(1).gameObject;
        gomatch = gomain.transform.GetChild(2).gameObject;
        goback = gomain.transform.GetChild(0).gameObject;
        gobut = gomain.transform.GetChild(3).gameObject;
    }

    public void Button_Quit()
    {
        Application.Quit();
    }

    public void Button_Show_DirectPlay()
    {
        InitButton();
        godirect.SetActive(true);
        goback.SetActive(true);
        gobut.SetActive(false);
    }

    public void Button_Show_MatchMake()
    {
        InitButton();
        gomatch.SetActive(true);
        goback.SetActive(true);
        gobut.SetActive(false);
    }

    public void Button_Show_Back()
    {
        InitButton();
        godirect.SetActive(false);
        gomatch.SetActive(false);
        goback.SetActive(false);
        gobut.SetActive(true);
    }
}