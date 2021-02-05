using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buns : MonoBehaviour
{
    private float tomatThicc = 0.007f;
    private float pattyThicc = 0.0125f;
    private float lettuceThicc = 0.005f;
    private float cheeseThicc = 0.005f;
    private bool isValid;
    public bool hasTopBun = false;

    public float numTomat = 0;
    public float numPatty = 0;
    public float numBurntPatty = 0;
    public float numLettuce = 0;
    public float numCheese = 0;
    public bool thePattyFactor;

    private float totalThicc = 0.008f;
    public FixedJoint connected;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.up);
        thePattyFactor = false;
        if(Physics.Raycast(ray, out hit, totalThicc + 0.01f))
        {
            GameObject other = hit.collider.gameObject;
            isValid = false;
            if (!hasTopBun)
            {
                if (other.CompareTag("Patty"))
                {
                    isValid = true;
                    totalThicc += pattyThicc;
                    thePattyFactor = true;
                    if (other.GetComponent<Cooking>().isBurnt)
                    {
                        numBurntPatty += 1;
                    }
                    else
                    {
                        numPatty += 1;
                    }
                }

                else if (other.CompareTag("Tomato Slice"))
                {
                    isValid = true;
                    totalThicc += tomatThicc;
                    numTomat += 1;
                }

                else if (other.CompareTag("Lettuce Slice"))
                {
                    isValid = true;
                    totalThicc += lettuceThicc;
                    numLettuce += 1;
                }
                else if (other.CompareTag("Cheese"))
                {
                    isValid = true;
                    totalThicc += cheeseThicc;
                    numCheese += 1;
                }
                else if (other.CompareTag("Top Bun"))
                {
                    hasTopBun = true;
                    isValid = true;
                }

                if (isValid)
                {
                    other.layer = 2;
                    other.transform.rotation = Quaternion.identity;
                    other.transform.position = transform.position + new Vector3(0, totalThicc, 0);

                    FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = other.gameObject.GetComponent<Rigidbody>();
                    joint.enableCollision = false;
                    totalThicc += .003f;
                    if (thePattyFactor)
                    {
                        totalThicc += .004f;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
    }
}
