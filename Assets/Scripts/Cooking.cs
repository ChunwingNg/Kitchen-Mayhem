using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cooking : MonoBehaviour
{
    public float timeToCook;
    private bool cooking;
    public AudioSource sound;
    public Material cooked;
    public Material burnt;
    private Renderer rend;

    public bool isCooked = false;
    public bool isBurnt = false;

    // Start is called before the first frame update
    void Start()
    {
        cooking = false;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (cooking)
        {
            timeToCook -= Time.deltaTime;
            if(timeToCook < 0.0f && timeToCook > -15.0f && !isCooked)
            {
                rend.material = cooked;
                Debug.Log("Juicy");
                isCooked = true;
            }
            if(timeToCook <= -15.0f && !isBurnt)
            {
                rend.material = burnt;
                Debug.Log("You fucker");
                isBurnt = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Grill")
        {
            Debug.Log("grill me harder");
            cooking = true;
            sound.Play();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Grill")
        {
            Debug.Log("you're so cold to me");
            cooking = false;
            sound.Stop();
        }
    }
}
