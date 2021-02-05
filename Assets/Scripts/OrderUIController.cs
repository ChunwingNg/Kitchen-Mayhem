using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderUIController : MonoBehaviour
{
    public Order order;

    public Text patty;
    public Text cheese;
    public Text lettuce;
    public Text tomato;

    public Text pattyS;
    public Text cheeseS;
    public Text lettuceS;
    public Text tomatoS;

    public float pat;
    public float che;
    public float let;
    public float tom;

    public Slider timer;
    public Image fill;

    public float currTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer.value = currTime / 180;
        fill.color = Color.Lerp(Color.red, Color.green, currTime / 180);
        patty.text = "" + pat;
        cheese.text = "" + che;
        lettuce.text = "" + let;
        tomato.text = "" + tom;
        pattyS.text = "" + pat;
        cheeseS.text = "" + che;
        lettuceS.text = "" + let;
        tomatoS.text = "" + tom;
    }
}
