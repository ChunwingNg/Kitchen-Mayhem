using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public Transform HandAnchor;
    public Transform head;
    public Transform trackingSpace;
    public float speed;
    public OVRInput.Controller myHand;
    Grabbable grabbedObject;
    public Transform attachPoint;
    public Transform laser;
    public GameObject laserObj;
    private float triggerValue;
    public GameObject arcPointPrefab;
    public float arcSpeed;
    public float snapRotateDelta;
    List<GameObject> arcPoints = new List<GameObject>();
    bool canSnapRotate = true;
    bool tep = false;
    //View for netowrking
    public PhotonView photonView;


    // Start is called before the first frame update
    void Start()
    {
        attachPoint = Instantiate<GameObject>(new GameObject(), this.transform).GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headToController = HandAnchor.position - head.position;
        float d = headToController.magnitude;
        float s = 1;
        if(d > .5f)
        {
            s = 1 + 2 * (d - .5f);
        }
        transform.position = head.position + s * d * headToController.normalized;

        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, myHand);

        if(triggerValue < .5f && grabbedObject != null)
        {
            if (grabbedObject.endGrab(this))
            {
                grabbedObject = null;
            }
        }

        //arc
        //Vector3 footPos = head.position;
        
        /*bool startTP = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);      

        if (startTP || tep)
        {
            tep = true;
            Vector3 arcVelocity = laser.forward * arcSpeed;
            Vector3 arcPos = laser.position;
            
            footPos.y -= head.localPosition.y;

            float distance = 0;
            foreach (GameObject p in arcPoints)
            {
                GameObject.Destroy(p);
            }
            arcPoints.Clear();
            while (distance < 10)
            {
                Vector3 delta_p = arcVelocity * .01f;

                RaycastHit[] hits = Physics.RaycastAll(arcPos, arcVelocity.normalized, delta_p.magnitude);
                bool arcHit = false;
                Vector3 arcHitPoint = Vector3.zero;
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit hit = hits[i];
                    GameObject go2 = GameObject.Instantiate<GameObject>(arcPointPrefab, hit.point, Quaternion.identity);
                    go2.transform.forward = arcVelocity.normalized;
                    go2.transform.localScale = new Vector3(1, 1, .01f);
                    arcPoints.Add(go2);
                    arcHit = true;
                    arcHitPoint = hit.point;
                    break;
                }
                if (arcHit)
                {
                    //teleporter
                    Vector3 targetPoint = arcHitPoint;
                    Vector3 offset = targetPoint - footPos;

                    bool trigger = OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);

                    if (trigger)
                    {
                        foreach (GameObject p in arcPoints)
                        {
                            GameObject.Destroy(p);
                        }
                        arcPoints.Clear();
                        trackingSpace.Translate(offset, Space.World);
                        tep = false;

                    }

                    break;
                }

                arcPos += delta_p;
                arcVelocity += new Vector3(0, -9.8f, 0) * .01f;
                distance += delta_p.magnitude;

                GameObject go = GameObject.Instantiate<GameObject>(arcPointPrefab, arcPos, Quaternion.identity);
                go.transform.forward = arcVelocity.normalized;
                go.transform.localScale = new Vector3(1, 1, delta_p.magnitude);
                arcPoints.Add(go);
            }
            
        }*/
        
        /*
        Ray r = new Ray(laser.position, laser.forward);
        RaycastHit[] hits = Physics.RaycastAll(r, 100.0f);
        laser.localScale = new Vector3(0, 0, 0);
        for (int i = 0; i < hits.Length; i++)
        {
            laser.localScale = new Vector3(1, 1 , hits[i].distance);
            RaycastHit hit = hits[i];
            Rigidbody rb = hit.rigidbody;
            if(rb != null && !rb.CompareTag("Fridge"))
            {
                laserObj.GetComponent<MeshRenderer>().enabled = true;
                triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, myHand);
                if (triggerValue > .05f && attachedRigidBody == null)
                {
                    laserObj.GetComponent<MeshRenderer>().enabled = false;
                    //Setting photon view for the object that the hand is holding
                    photonView = rb.GetComponent<PhotonView>();
                    //Check for ownership of item, if not the owner, try to become the owner of the object
                    if (!photonView.IsMine)
                    {
                        photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    attachedRigidBody = rb;
                    attachPoint.position = this.transform.position;
                    attachPoint.rotation = this.transform.rotation;
                    //this.GetComponent<MeshRenderer>().enabled = false;
                }

        //Keep this commented out
                else if (triggerValue < .04f && attachedRigidBody != null)
                {
                    this.GetComponent<MeshRenderer>().enabled = true;
                    attachedRigidBody = null;
                }

                break;
            }

           else
            {
                //teleporter
                Vector3 targetPoint = hit.point;
                Vector3 footPos = head.position;
                footPos.y -= head.localPosition.y;
                Vector3 offset = targetPoint - footPos;
                
                bool startTP = OVRInput.GetDown(OVRInput.Button.PrimaryThumbstick, myHand);
                if (startTP)
                {
                    laserObj.GetComponent<MeshRenderer>().enabled = true;
                    buttonDown = true;
                }
                bool trigger = OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick, myHand);

                if (trigger)
                {
                    trackingSpace.Translate(offset, Space.World);
                    buttonDown = false;
                }

            }
            

        }*/
        /*
        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, myHand);
        float up = joystickInput.y;
        float right = joystickInput.x;

        Vector3 headForwardVector = head.forward;
        headForwardVector.y = 0;
        headForwardVector.Normalize();
      

        Vector3 direction = headForwardVector * up; 

        trackingSpace.transform.Translate(direction * speed * Time.deltaTime, Space.World);

        float rightMag = Mathf.Abs(right);
        if(rightMag > .9f && canSnapRotate)
        {
            trackingSpace.transform.RotateAround(footPos, Vector3.up, snapRotateDelta * Mathf.Sign(right));
            //snap rotate
            canSnapRotate = false;
        }
        if(rightMag < .2f)
        {
            canSnapRotate = true;
        }*/

    }



    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRb = other.attachedRigidbody;
        if(otherRb == null)
        {
            return;
        }
        Grabbable gr = otherRb.GetComponent<Grabbable>();
        if(gr == null)
        {
            return;
        }

        triggerValue = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, myHand);
        if(triggerValue > .55f && grabbedObject == null)
        {
            attachPoint.position = otherRb.position;
            attachPoint.rotation = otherRb.rotation;
            if(gr.startGrab(this, attachPoint))
            {
                grabbedObject = gr;
                //this.GetComponent<MeshRenderer>().enabled = false;
            }
            
            //Setting photon view for the object that the hand is holding
            photonView = otherRb.GetComponent<PhotonView>();
            //Check for ownership of item, if not the owner, try to become the owner of the object
            if (!photonView.IsMine)
            {
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
            }

            
            laserObj.GetComponent<MeshRenderer>().enabled = false;
            
        }
        
        
    }
}
