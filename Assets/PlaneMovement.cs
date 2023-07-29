using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlaneMovement : MonoBehaviour
{

    PlaneTracker tracker;
    public float sens;
    Vector3 velocity = new(0,0,0.5f);
    Vector3 RotInertia;
    public float DragCoefficient;

    public TextMeshProUGUI textMeshProUGUI;

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



        textMeshProUGUI.text = ("Speed: " + velocity.magnitude.ToString("0.00"));

        DoPlaneRotation();
        DoPlaneMovement();
        DoAOACheck();

        tracker.UpdateCamPosition();
        tracker.CamFOVAdditive = velocity.magnitude * 20f;
    }
    public void DoPlaneRotation()
    {
        Vector3 inputRots = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Roll")) * sens * Time.deltaTime;

        

        RotInertia += 0.5f * velocity.magnitude * inputRots;

        RotInertia *= 1 - (Time.deltaTime);

        gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(RotInertia);

    }
    public void DoPlaneMovement()
    {
        if(Input.GetAxisRaw("Throttle") > 0.1)
        {
            velocity += gameObject.transform.rotation * Vector3.forward * Time.deltaTime * 0.1f;
            tracker.isBoosting = true;
        }
        else
        {
            tracker.isBoosting = false;
        }
        velocity = velocity * (1-(Time.deltaTime*velocity.sqrMagnitude * DragCoefficient));

        velocity += Vector3.down * 0.03f * Time.deltaTime;
        //print("Velocity: " + velocity);
        gameObject.transform.position += velocity;
    }
    public void DoAOACheck()
    {
        Vector3 AOAalongAxis = Quaternion.Inverse(gameObject.transform.rotation) * velocity.normalized;

        gameObject.transform.eulerAngles += new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0) * velocity.magnitude * Time.deltaTime * 50f;

        textMeshProUGUI.text += "\nPressureStrength on Plane Rotation: " + (new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0).magnitude * velocity.magnitude *50f).ToString("0.0");

        float pressureStrength = (Mathf.Abs(AOAalongAxis.x) * yawCorrectionPower + Mathf.Abs(AOAalongAxis.y) * PitchCorrectionPower) *Time.deltaTime;

        velocity = Vector3.LerpUnclamped(velocity, (gameObject.transform.rotation * Vector3.forward) * velocity.magnitude,
            pressureStrength);

        textMeshProUGUI.text += "\nPressureStrength on Velocity Vector: " + (pressureStrength / Time.deltaTime).ToString("0.0");
    }
}
