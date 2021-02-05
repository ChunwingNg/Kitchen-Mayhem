using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    //Stores the objects
    public GameObject topBun;
    public GameObject bottomBun;
    public GameObject patty;
    public GameObject tomato;
    public GameObject lettuce;
    public GameObject cheese;


    //Positions to spawn at
    public GameObject top1;
    public GameObject top2;
    public GameObject top3;
    public GameObject bottom1;
    public GameObject bottom2;
    public GameObject bottom3;
    public GameObject patty1;
    public GameObject patty2;
    public GameObject patty3;
    public GameObject tomato1;
    public GameObject tomato2;
    public GameObject lettuce1;
    public GameObject lettuce2;
    public GameObject cheese1;
    public GameObject cheese2;
    public GameObject cheese3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (PhotonNetwork.InRoom)
        {
            if (other.gameObject.CompareTag("Fridge Door"))
            {
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.anchor = new Vector3(0.5f, 0, 0);
                joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                joint.enableCollision = false;
                joint.breakForce = 25;
                joint.breakTorque = 25;
                Debug.Log("Fridge is closed, spawning items");

                //Spawning the top buns
                PhotonNetwork.Instantiate(topBun.name, top1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(topBun.name, top2.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(topBun.name, top3.transform.position, Quaternion.identity);

                //Spawning the bottom buns
                PhotonNetwork.Instantiate(bottomBun.name, bottom1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(bottomBun.name, bottom2.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(bottomBun.name, bottom3.transform.position, Quaternion.identity);

                //Spawning the patties
                PhotonNetwork.Instantiate(patty.name, patty1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(patty.name, patty2.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(patty.name, patty3.transform.position, Quaternion.identity);

                //Spawning the tomatos
                PhotonNetwork.Instantiate(tomato.name, tomato1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(tomato.name, tomato2.transform.position, Quaternion.identity);

                //Spawning the lettuce
                PhotonNetwork.Instantiate(lettuce.name, lettuce1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(lettuce.name, lettuce2.transform.position, Quaternion.identity);

                //Spawning the cheese
                PhotonNetwork.Instantiate(cheese.name, cheese1.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(cheese.name, cheese2.transform.position, Quaternion.identity);
                PhotonNetwork.Instantiate(cheese.name, cheese3.transform.position, Quaternion.identity);
            }
        }
    }
}
