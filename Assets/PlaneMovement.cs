using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaneMovement : MonoBehaviour
{

    public float sens;
    public float Thrust;
    public float DragCoefficient;

    Rigidbody RB;

    public TextMeshProUGUI textMeshProUGUI;

    public float yawCorrectionPower = 3f;
    public float PitchCorrectionPower = 10f;

    ParticleSystem PS;
    public bool Throttle = false;

    float BonkResetTimer = 1; 
    // Start is called before the first frame update
    void Start()
    {
        PS = GetComponentInChildren<ParticleSystem>();
        RB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        BonkResetTimer += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        textMeshProUGUI.text = ("Speed: " + RB.velocity.magnitude.ToString("0.00"));

        DoPlaneRotation();
        DoPlaneMovement();
        DoAOACheck();
    }
    public void DoPlaneRotation()
    {
        Vector3 inputRots = new Vector3(Input.GetAxisRaw("Vertical"), Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Roll")) * sens;

        RB.AddRelativeTorque(inputRots * (RB.velocity.magnitude + 1f), ForceMode.Force);

        /*
        RotInertia += 0.5f * velocity.magnitude * inputRots;

        RotInertia *= 1 - (Time.deltaTime);

        gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(RotInertia);
        */
    }
    public void UpdateDrag()
    {
        DragCoefficient = GameObject.Find("DragSlider").GetComponent<Slider>().value;
    }
    public void DoPlaneMovement()
    {
        if (Input.GetAxisRaw("Throttle") > 0.1 && BonkResetTimer > 1)
        {
            RB.AddRelativeForce(Vector3.forward * Thrust);
            Throttle = true;
        }
        else
        {
            Throttle = false;
        }

        if (Throttle)
            PS.Play();
        else if(!Throttle && PS.isPlaying)
            PS.Stop();

        //Ground Check
        if (gameObject.transform.position.y <= -50 && RB.velocity.y < 0)
        {
            RB.velocity = new Vector3(RB.velocity.x, RB.velocity.y * -1, RB.velocity.z);
            RB.velocity *= 0.9f;
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        BonkResetTimer = 0;
        print("Hit Obj at " + RB.velocity.magnitude);
    }
    public void DoAOACheck()
    {
        Vector3 AOAalongAxis = Quaternion.Inverse(gameObject.transform.rotation) * RB.velocity.normalized * RB.velocity.magnitude/50f;

        RB.AddRelativeTorque(new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0) * Mathf.Clamp01(BonkResetTimer)); 

        //gameObject.transform.eulerAngles += new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0) * velocity.magnitude * Time.deltaTime * 100f;

        textMeshProUGUI.text += "\nPressureStrength on Plane Rotation: " + (new Vector3(-AOAalongAxis.y, AOAalongAxis.x, 0).magnitude *100f).ToString("0.0");

        float pressureStrength = (Mathf.Abs(AOAalongAxis.x) * yawCorrectionPower + Mathf.Abs(AOAalongAxis.y) * PitchCorrectionPower) * Mathf.Clamp01(BonkResetTimer);

        RB.velocity = Vector3.LerpUnclamped(RB.velocity, (gameObject.transform.rotation * Vector3.forward) * RB.velocity.magnitude,
            pressureStrength);

        textMeshProUGUI.text += "\nPressureStrength on Velocity Vector: " + (pressureStrength / Time.deltaTime).ToString("0.0");
    }
}
