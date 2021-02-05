using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftHandControls : MonoBehaviour
{
    public Transform head;
    public Transform laser;
    public GameObject arcPointPrefab;
    public float arcSpeed;
    private float triggerValue;
    List<GameObject> arcPoints = new List<GameObject>();
    public OVRInput.Controller myHand;
    public Transform trackingSpace;
    bool tep = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //arc
        Vector3 footPos = head.position;

        bool startTP = OVRInput.Get(OVRInput.Button.PrimaryThumbstick, OVRInput.Controller.LTouch);

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

        }
    }
}
