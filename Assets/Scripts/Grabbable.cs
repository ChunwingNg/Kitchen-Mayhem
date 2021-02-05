using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Grabbable : MonoBehaviour
{
    Hand grabbedBy = null;
    Rigidbody rb;
    Transform offsetLocation = null; 
    Vector3 localGrabbedLocation;
    public bool useForce = false;
    public bool useExact = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
       if(grabbedBy != null)
        {
            
            if(!useForce && !useExact)
            {
                Vector3 difference = rb.position - offsetLocation.position;
                rb.velocity = -difference / Time.fixedDeltaTime;

                Quaternion rotationDifference = rb.rotation * Quaternion.Inverse(offsetLocation.rotation);
                float angle;
                Vector3 axis;
                rotationDifference.ToAngleAxis(out angle, out axis);
                Vector3 angularVelocity = -Mathf.Deg2Rad * angle / Time.fixedDeltaTime * axis;
                rb.angularVelocity = angularVelocity;
            }
            else if(useForce && !useExact)
            {
                Vector3 worldGrabPosition = transform.TransformPoint(localGrabbedLocation);
                Vector3 force = grabbedBy.transform.position - worldGrabPosition;
                rb.AddForceAtPosition(100*force, worldGrabPosition);
            }
            else
            {
                HingeJoint hj = GetComponent<HingeJoint>();
                if(hj != null)
                {
                    Vector3 worldGrabPosition = transform.TransformPoint(localGrabbedLocation);
                    Vector3 a = worldGrabPosition - hj.connectedAnchor;
                    Vector3 b = grabbedBy.transform.position - hj.connectedAnchor;

                    Vector3 axis = hj.axis;
                    float dotAaxis = Vector3.Dot(a, axis);
                    float dotBaxis = Vector3.Dot(b, axis);
                    a = a - dotAaxis * axis;
                    b = b - dotBaxis * axis;
                    float angle = Vector3.SignedAngle(a.normalized, b.normalized, axis);

                    transform.Rotate(hj.axis, angle, Space.Self);
                }
            }
            
        }


    }

    public bool startGrab(Hand g, Transform grabLocation)
    {
        if(grabbedBy != null || g == null)
        {
            return false;
        }
        else
        {
            grabbedBy = g;
            offsetLocation = grabLocation;
            localGrabbedLocation = transform.InverseTransformPoint(g.transform.position);
            return true;
        }
    }

    public bool endGrab(Hand g)
    {
        if(grabbedBy == null || g == null)
        {
            return false;
        }
        grabbedBy = null;
        offsetLocation = null;
        return true;
    }
}
