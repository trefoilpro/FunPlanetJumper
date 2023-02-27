using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Player : NetworkBehaviour
{
    public static Player LocalPlayer;
    [SyncVar] public string MatchID;

    private NetworkMatch _networkMatch;

    private void Start()
    {
        _networkMatch = GetComponent<NetworkMatch>();

        Debug.Log("isLocalPlayer = " + isLocalPlayer);
        
        if (isLocalPlayer)
        {   
            LocalPlayer = this;
        }
        else
        {
            MainMenu.instance.SpawnPlayerUIPrefab(this);
        }
    }

    public void HostGame()
    {
        string ID = MainMenu.GetRandomID();
        CmdHostGame(ID);
    }

    [Command]
    public void CmdHostGame(string ID)
    {
        MatchID = ID;
        if (MainMenu.instance.HostGame(ID, gameObject))
        {
            Debug.Log("Lobby was spawned");
            _networkMatch.matchID = ID.ToGuid();
            TargetHostGame(true, ID);
        }
        else
        {
            Debug.Log("Error in creating a lobby");
            TargetHostGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetHostGame(bool success, string ID)
    {
        MatchID = ID;
        Debug.Log($"ID {MatchID} == {ID}");
        MainMenu.instance.HostSuccess(success, ID);
    }
    
     public void JoinGame(string inputID)
    {
        CmdJoinGame(inputID);
    }

    [Command]
    public void CmdJoinGame(string ID)
    {
        MatchID = ID;
        if (MainMenu.instance.JoinGame(ID, gameObject))
        {
            Debug.Log("Connection completed");
            _networkMatch.matchID = ID.ToGuid();
            TargetJoinGame(true, ID);
        }
        else
        {
            Debug.Log("Error in Connection to the lobby");
            TargetJoinGame(false, ID);
        }
    }

    [TargetRpc]
    void TargetJoinGame(bool success, string ID)
    {
        MatchID = ID;
        Debug.Log($"ID {MatchID} == {ID}");
        MainMenu.instance.JoinSuccess(success, ID);
    }
    
    public void BeginGame()
    {
        CmdBeginGame();
    }

    [Command]
    public void CmdBeginGame()
    {
        MainMenu.instance.BeginGame(MatchID);
        Debug.Log("Game is starting");
    }

    public void StartGame()
    {
        TargetBeginGame();
    }

    [TargetRpc]
    void TargetBeginGame()
    {
        Debug.Log($"ID {MatchID} | start");
        DontDestroyOnLoad(gameObject);
        MainMenu.instance.inGame = true;
        gameObject.SetActive(true);
        SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    }
}
