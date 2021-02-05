using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightHandControls : MonoBehaviour
{
    public Transform head;
    public Transform trackingSpace;
    public float speed;
    bool canSnapRotate = true;
    public float snapRotateDelta;
    public Text conLeave;
    public GameObject leavePrompt;
    public float timer;
    bool timeCheck = false;
    // Start is called before the first frame update
    void Start()
    {
        timer = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        bool trig = OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.RTouch);
        if (trig)
        {
            timer = 10.0f;
            leavePrompt.SetActive(true);
            timeCheck = true;
        }
        else if(timeCheck)
        {
            timer -= Time.deltaTime;
            conLeave.text = "Press the same button on the left controller to exit. This prompt will close in: " + timer;
            bool leave = OVRInput.GetUp(OVRInput.Button.Two, OVRInput.Controller.LTouch);
            if (leave)
            {
                Debug.Log("Exiting application...");
                leavePrompt.SetActive(false);
                timeCheck = false;
                Application.Quit();
            }
            else if (timer < 0)
            {
                Debug.Log("Timer ran out");
                timer = 10.0f;
                leavePrompt.SetActive(false);
                timeCheck = false;
            }
        }
        Vector3 footPos = head.position;

        Vector2 joystickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);
        float up = joystickInput.y;
        float right = joystickInput.x;

        Vector3 headForwardVector = head.forward;
        headForwardVector.y = 0;
        headForwardVector.Normalize();
        

        Vector3 direction = headForwardVector * up; // + headRightVector * right;

        trackingSpace.transform.Translate(direction * speed * Time.deltaTime, Space.World);

        float rightMag = Mathf.Abs(right);
        if (rightMag > .9f && canSnapRotate)
        {
            trackingSpace.transform.RotateAround(footPos, Vector3.up, snapRotateDelta * Mathf.Sign(right));
            //snap rotate
            canSnapRotate = false;
        }
        if (rightMag < .2f)
        {
            canSnapRotate = true;
        }
    }
}
