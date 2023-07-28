using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{

    PlaneTracker tracker;
    public float sens;
    Vector3 velocity;
    Vector3 RotInertia;
    public float DragCoefficient;

    public float yawCorrectionPower = 3f;
    public float PitchCorrectionPower = 10f;

    // Start is called before the first frame update
    void Start()
    {
        tracker = FindObjectOfType<PlaneTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        DoPlaneRotation();
        DoPlaneMovement();

        DoAOACheck();

        print("Vel = " + velocity.magnitude);

        tracker.UpdateCamPosition();
    }
    public void DoPlaneRotation()
    {
        Vector3 inputRots = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Roll")) * sens * Time.deltaTime;

        

        RotInertia += (inputRots * velocity.sqrMagnitude * 10f) * 0.05f;

        RotInertia *= 1 - (Time.deltaTime);

        gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(RotInertia);

    }
    public void DoPlaneMovement()
    {
        if(Input.GetAxisRaw("Throttle") > 0.1)
        {
            velocity += gameObject.transform.rotation * Vector3.forward * Time.deltaTime * 0.1f;
        }
        velocity = velocity * (1-(Time.deltaTime*velocity.sqrMagnitude * DragCoefficient));

        velocity += Vector3.down * 0.05f * Time.deltaTime;
        //print("Velocity: " + velocity);
        gameObject.transform.position += velocity;
    }
    public void DoAOACheck()
    {
        Vector3 AOAalongAxis = Quaternion.Inverse(gameObject.transform.rotation) * velocity.normalized;

        gameObject.transform.eulerAngles += new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0) * velocity.sqrMagnitude * Time.deltaTime * 10000f;

        float pressureStrength = (Mathf.Abs(AOAalongAxis.x) * yawCorrectionPower + Mathf.Abs(AOAalongAxis.y) * PitchCorrectionPower) *Time.deltaTime;

        velocity = Vector3.LerpUnclamped(velocity, (gameObject.transform.rotation * Vector3.forward) * velocity.magnitude,
            pressureStrength);

    }
}
