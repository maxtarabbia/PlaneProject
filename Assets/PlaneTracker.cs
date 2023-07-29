using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneTracker : MonoBehaviour
{
    public GameObject planeGO;

    public Vector3 CamOffset;
    public Vector3 RotationOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

#if         UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
           
        }
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
