using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayController : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject canvas;
    public GameObject tray;
    public float score;

    private List<Order> orders;
    private List<GameObject> orderUIs = new List<GameObject>();

    public GameObject position1;
    public GameObject position2;
    public GameObject position3;
    public GameObject position4;
    public GameObject position5;
    public GameObject position6;
    public GameObject position7;
    public GameObject position8;
    public GameObject position9;
    public GameObject position10;
    public GameObject position11;

    public Text top;
    public Text bot;

    void Start()
    {
        orderUIs.Add(position1);
        orderUIs.Add(position2);
        orderUIs.Add(position3);
        orderUIs.Add(position4);
        orderUIs.Add(position5);
        orderUIs.Add(position6);
        orderUIs.Add(position7);
        orderUIs.Add(position8);
        orderUIs.Add(position9);
        orderUIs.Add(position10);
        orderUIs.Add(position11);
        score = 0;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            score = (float)stream.ReceiveNext();
        }
    }

    // Update is called once per frame
    void Update()
    {
        top.text = "Score\n" + score;
        bot.text = "Score\n" + score;
        //Display order/scoreboard
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) && !canvas.activeSelf)
        {
            canvas.SetActive(true);
        }
        else if (!OVRInput.Get(OVRInput.Button.PrimaryHandTrigger, OVRInput.Controller.LTouch) && canvas.activeSelf)
        {
            canvas.SetActive(false);
        }
        if (PhotonNetwork.InRoom)
        {
            orders = tray.GetComponent<Checker>().getOrders();
            int i = 0;
            for (; i < orders.Count; i++)
            {
                if (orders[i].completed)
                {
                    if (tray.GetComponent<PhotonView>().IsMine)
                    {
                        score += orders[i].Points();
                        orders.Remove(orders[i]);
                    }
                }
                else if (orders[i].kill)
                {
                    if (tray.GetComponent<PhotonView>().IsMine)
                    {
                        orders.Remove(orders[i]);
                        score -= 50;
                    }
                }
                else
                {
                    if (!orderUIs[i].activeSelf)
                    {
                        orderUIs[i].SetActive(true);
                    }
                    orderUIs[i].GetComponent<OrderUIController>().currTime = 180 - orders[i].currTime();
                    orderUIs[i].GetComponent<OrderUIController>().pat = orders[i].Patty();
                    orderUIs[i].GetComponent<OrderUIController>().che = orders[i].Cheese();
                    orderUIs[i].GetComponent<OrderUIController>().let = orders[i].Lettuce();
                    orderUIs[i].GetComponent<OrderUIController>().tom = orders[i].Tomat();
                }
            }
            for (; i < 11; i++)
            {
                if (orderUIs[i].activeSelf)
                    orderUIs[i].SetActive(false);
            }
        }
    }
}
