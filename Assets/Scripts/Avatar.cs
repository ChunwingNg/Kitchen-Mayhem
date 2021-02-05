using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Avatar : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rig rig;

    public Transform head;
    public Transform lHand;
    public Transform rHand;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rig.head.position);
            stream.SendNext(rig.head.rotation);
            stream.SendNext(rig.lHand.position);
            stream.SendNext(rig.rHand.position);
        }
        else
        {
            head.position = (Vector3)stream.ReceiveNext();
            head.rotation = (Quaternion)stream.ReceiveNext();
            lHand.position = (Vector3)stream.ReceiveNext();
            rHand.position = (Vector3)stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            head.position = rig.head.position;
            head.rotation = rig.head.rotation;
            lHand.position = rig.lHand.position;
            lHand.rotation = rig.lHand.rotation;
            rHand.position = rig.rHand.position;
            rHand.rotation = rig.rHand.rotation;
        }

    }
}
