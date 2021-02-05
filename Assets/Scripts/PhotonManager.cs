using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{

    public GameObject player;
    public GameObject orderUI;
    public GameObject tray;
    public Rig rig;

    // Start is called before the first frame update
    void Start()
    {
        //Use setting
        PhotonNetwork.ConnectUsingSettings();
        
        //Change the send rate
        PhotonNetwork.SendRate = 30;

    }

    //Set up call back
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        PhotonNetwork.JoinOrCreateRoom("room", new Photon.Realtime.RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Room Joined");
        Debug.Log("Players in room: " + PhotonNetwork.CurrentRoom.PlayerCount);
        Avatar avatar = PhotonNetwork.Instantiate(player.name, Vector3.zero, Quaternion.identity).GetComponent<Avatar>();
        avatar.rig = rig;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
