using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;


/// <summary>
/// this help player customization from lobby to be applied on our player class.
/// </summary>
/// 
public class LobHooks : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();
        ProtoContrl locPlayer = gamePlayer.GetComponent<ProtoContrl>();

        locPlayer.pname = lobPlayer.playerName;
        locPlayer.pcolor = lobPlayer.playerColor;
    }
}
