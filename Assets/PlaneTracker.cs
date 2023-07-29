using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTracker : MonoBehaviour
{
    public GameObject planeGO;

    public Vector3 CamOffset;
    public Vector3 RotationOffset;

    float BaseCamFOV = 80;
    float transitionTime = 0.0f;

    public float CamFOVIncrease;
    public float CamFOVAdditive;

    public bool isBoosting;

    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        BaseCamFOV = cam.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBoosting)
        {
            transitionTime += Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(BaseCamFOV, BaseCamFOV + CamFOVIncrease, LinToSph(transitionTime * 5)) + CamFOVAdditive;
        }
        else
        {
            transitionTime -= Time.deltaTime;
            cam.fieldOfView = Mathf.Lerp(BaseCamFOV + CamFOVIncrease, BaseCamFOV, LinToSph(1 - transitionTime * 5)) + CamFOVAdditive;
        }

        transitionTime = Mathf.Clamp(transitionTime, 0, 0.2f);




        if (Input.GetKeyDown(KeyCode.Escape))
        {

#if         UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
           
        }
    }


    public float LinToSph(float i)
    {
        i = Mathf.Clamp01(i);
        return Mathf.Clamp01(Mathf.Sqrt(1 - Mathf.Pow(i - 1,2)));
    }

    public void UpdateCamPosition()
    {
        if(planeGO == null)
        {
            print("Plane GameObject is null!");
            return;
        }

        gameObject.transform.rotation = planeGO.transform.rotation;
        gameObject.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(RotationOffset);
        gameObject.transform.position = planeGO.transform.position + planeGO.transform.rotation * CamOffset;
    }

}
