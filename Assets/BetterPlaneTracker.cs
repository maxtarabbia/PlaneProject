using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPlaneTracker : MonoBehaviour
{
    public GameObject planeGO;



    public float TargetDist = 10f;
    public float DistVariance = 5f;
    public Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        Time.fixedDeltaTime = 0.01f;
    }
    // Update is called once per frame
    void Update()
    {
        UpdateCam();

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

    public void UpdateCam()
    {   
        //Check Plane
        if(planeGO == null)
        {
            print("Plane GameObject is null!");
            return;
        }


        //Cam Position
        Vector3 offset = -planeGO.transform.position + gameObject.transform.position;

        float magnitude;

        gameObject.transform.position += velocity;

        if (offset.magnitude > TargetDist + DistVariance)
            magnitude = TargetDist + DistVariance;
        else if (offset.magnitude < TargetDist - DistVariance)
            magnitude = TargetDist - DistVariance;
        else
            magnitude = offset.magnitude * 0.9f + TargetDist * 0.1f;

        //print(magnitude);

        Vector3 newposition = planeGO.transform.position + (offset.normalized * magnitude);

        velocity = newposition - gameObject.transform.position;

        gameObject.transform.position = newposition;

        //Cam Rotation
        gameObject.transform.rotation = Quaternion.LookRotation(planeGO.transform.position - gameObject.transform.position, Vector3.up);
    }

}
