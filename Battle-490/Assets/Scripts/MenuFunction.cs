using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// this class handles menu events dispatch by the buttons 
/// </summary>
/// 
public class MenuFunction : MonoBehaviour
{

    /// <summary>
    /// all the buttons that could be referenced.
    /// </summary>
    GameObject gomain, godirect, gomatch, goback, gobut;

    /// <summary>
    /// this function init button to vars by traversing from parent.
    /// </summary>
    public void InitButton()
    {
        gomain = GameObject.Find("MainPanel");
        godirect = gomain.transform.GetChild(1).gameObject;
        gomatch = gomain.transform.GetChild(2).gameObject;
        goback = gomain.transform.GetChild(0).gameObject;
        gobut = gomain.transform.GetChild(3).gameObject;
    }

    /// <summary>
    /// quit the application on button pressed.
    /// </summary>
    public void Button_Quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// show direct play panel on button pressed.
    /// </summary>
    public void Button_Show_DirectPlay()
    {
        InitButton();
        godirect.SetActive(true);
        goback.SetActive(true);
        gobut.SetActive(false);
    }

    /// <summary>
    /// show match make panel on button pressed.
    /// </summary>
    public void Button_Show_MatchMake()
    {
        InitButton();
        gomatch.SetActive(true);
        goback.SetActive(true);
        gobut.SetActive(false);
    }

    /// <summary>
    /// handle back action on button pressed.
    /// </summary>
    public void Button_Show_Back()
    {
        InitButton();
        godirect.SetActive(false);
        gomatch.SetActive(false);
        goback.SetActive(false);
        gobut.SetActive(true);
    }
    
}