using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{ 
    public GameObject tomatoSlice;
    public GameObject lettuceSlice;
    private Vector3 extraSpace;
    public AudioSource sound;
    public AudioSource drop;
    // Start is called before the first frame update
    void Start()
    {
        extraSpace = new Vector3(0, 0.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Tomato"))
        {
            //Create 3 slices
            for (int i = 0; i < 3; i++)
            {
                PhotonNetwork.Instantiate(tomatoSlice.name, collision.gameObject.transform.position, Quaternion.Euler(0, 0, 45));
            }

            //Destroy tomato
            Object.Destroy(collision.gameObject);

            sound.Play();
        }
        if (collision.gameObject.CompareTag("Lettuce"))
        {
            //Create 6 slices
            for (int i = 0; i < 6; i++)
            {
                PhotonNetwork.Instantiate(lettuceSlice.name, collision.gameObject.transform.position + extraSpace, Quaternion.Euler(0, 0, 45));
            }
            //Destroy tomato
            Object.Destroy(collision.gameObject);

            sound.Play();
        }
        else
        {
            drop.Play();
        }
    }
}
