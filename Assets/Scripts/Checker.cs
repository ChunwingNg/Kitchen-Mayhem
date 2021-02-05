using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Order
{
    private float numPatty;
    private float numLettuce;
    private float numTomat;
    private float numCheese;
    private float numBurntPatty;
    private float time;
    public bool kill = false;
    public bool completed = false;

    public Order(float a, float b, float c, float d, float e)
    {
        numPatty = a;
        numLettuce = b;
        numTomat = c;
        numCheese = d;
        numBurntPatty = e;
        time = 0;
    }

    public Order(float a, float b, float c, float d, float e, float t, bool k, bool com)
    {
        numPatty = a;
        numLettuce = b;
        numTomat = c;
        numCheese = d;
        numBurntPatty = e;
        time = t;
        kill = k;
        completed = com;
    }

    public void addTime(float t)
    {
        time += t;
    }

    public float currTime()
    {
        return time;
    }

    public float Points()
    {
        float num = time - 60;
        if(num <= 1)
        {
            num = 1;
        }
        return 180 - num;
    }
    public float Cheese()
    {
        return numCheese;
    }

    public float Lettuce()
    {
        return numLettuce;
    }

    public float Tomat()
    {
        return numTomat;
    }

    public float Patty()
    {
        return numPatty;
    }

    public float BurntPatty()
    {
        return numBurntPatty;
    }
}

public class Checker : MonoBehaviourPunCallbacks, IPunObservable
{

    public AudioSource correct;
    public AudioSource wrong;
    public AudioSource bell;

    public float newOrderTimer = 0;
    private Buns burger;
    private bool isWrong;

    private List<Order> orders;
    float totalOrders;
    private System.Random rnd = new System.Random();
    private float newOrder = 60;

    public GameObject tp;

    void Start()
    {
        orders = new List<Order>();
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        int num;
        if (stream.IsWriting)
        {
            stream.SendNext(orders.Count);

            for(int i = 0; i < orders.Count; i++)
            {
                stream.SendNext(orders[i].Patty());
                stream.SendNext(orders[i].Lettuce());
                stream.SendNext(orders[i].Tomat());
                stream.SendNext(orders[i].Cheese());
                stream.SendNext(orders[i].BurntPatty());
                stream.SendNext(orders[i].currTime());
                stream.SendNext(orders[i].kill);
                stream.SendNext(orders[i].completed);
            }
        }
        else
        {
            orders = new List<Order>();
            num = (int)stream.ReceiveNext();
            for(int k = 0; k < num; k++)
            {
                orders.Add(new Order((float)stream.ReceiveNext(),(float)stream.ReceiveNext(),
                    (float)stream.ReceiveNext(),(float)stream.ReceiveNext(),
                    (float)stream.ReceiveNext(),(float)stream.ReceiveNext(),
                    (bool)stream.ReceiveNext(),(bool)stream.ReceiveNext()));
            }
        }
    }

    public List<Order> getOrders()
    {
        return orders;
    }

    // Start is called before the first frame update
    public override void OnJoinedRoom()
    {
        if (photonView.IsMine)
        {
            orders = new List<Order>
            {
                //new Order(rnd.Next(0,4), rnd.Next(0, 4), rnd.Next(0, 4), rnd.Next(0, 4), 0)
            
                //Test Order
                new Order(1, 1, 1, 1, 0)
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine && PhotonNetwork.InRoom)
        {
            totalOrders = orders.Count;
            //Counts up on the timer
            newOrderTimer += Time.deltaTime;
            //Creates a new random order every 30 - 120 seconds
            if ((newOrderTimer >= newOrder) && (orders.Count < 11))
            {
                orders.Add(new Order(rnd.Next(0, 4), rnd.Next(0, 4), rnd.Next(0, 4), rnd.Next(0, 4), 0));
                photonView.RPC("Bell", RpcTarget.All, "wrong");
                Debug.Log("Orders: " + totalOrders);
                newOrderTimer = 0;
                newOrder = rnd.Next(30, 120);
            }
            for (int i = 0; i < totalOrders; i++)
            {
                orders[i].addTime(Time.deltaTime);
                if (orders[i].currTime() >= 180)
                {
                    orders[i].kill = true;
                    photonView.RPC("Wrong", RpcTarget.All, "wrong");
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isWrong = false;
        if (other.gameObject.CompareTag("Bottom Bun"))
        {
            burger = other.gameObject.GetComponent<Buns>();
            //Loops through each order
            for (int i = 0; i < totalOrders; i++)
            {
                if (burger.CompareTag("Bottom Bun"))
                {
                    if ((burger.numCheese == orders[i].Cheese())
                        && (burger.numLettuce == orders[i].Lettuce())
                        && (burger.numPatty == orders[i].Patty())
                        && (burger.numBurntPatty == orders[i].BurntPatty())
                        && (burger.numTomat == orders[i].Tomat())
                        && burger.hasTopBun)
                    {
                        photonView.RPC("Correct", RpcTarget.All, "wrong");
                        isWrong = false;
                        if (photonView.IsMine)
                        {
                            orders[i].completed = true;
                        }
                        break;
                    }
                    else
                    {
                        isWrong = true;
                    }
                }
            }

            if (isWrong)
            {
                photonView.RPC("Wrong",RpcTarget.All, "wrong");
            }
            if (!other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                other.gameObject.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
            other.gameObject.GetComponent<Transform>().position = tp.transform.position;
        }

    }

    [PunRPC]
    public void Bell(string s)
    {
        Debug.Log(s);
        bell.Play();
    }

    [PunRPC]
    public void Correct(string s)
    {
        Debug.Log(s);
        correct.Play();
    }

    [PunRPC]
    public void Wrong(string s)
    {
        Debug.Log(s);
        wrong.Play();
    }
}
