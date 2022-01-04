using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Controller : MonoBehaviour
{
    public ARSessionOrigin sessionOrigin;
    public GameObject arObjectToSpawn;
    public GameObject Indicator;
    public GameObject joystickCanvas;
    private GameObject spawnedObject;
    private Pose PlacementPose;
    private ARRaycastManager aRRaycastManager;
    private bool placementPoseIsValid = false;
   /* private GameObject go;*/
    void Start()
    {
        aRRaycastManager = FindObjectOfType<ARRaycastManager>();
        joystickCanvas.SetActive(false);
    }

    // need to update placement indicator
    void Update()
    {
        if (spawnedObject == null && placementPoseIsValid && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            ARPlaceObject();
            joystickCanvas.SetActive(true);
          /*   go = GameObject.Instantiate(arObjectToSpawn);
            go.transform.position = transform.localPosition;*/
        }


        UpdatePlacementPose();
        UpdatePlacementIndicator();


    }
    void UpdatePlacementIndicator()
    {
        if (spawnedObject == null && placementPoseIsValid)
        {
            Indicator.SetActive(true);
            Indicator.transform.SetPositionAndRotation(PlacementPose.position, PlacementPose.rotation);
        }
        else
        {
            Indicator.SetActive(false);
        }
    }

    void UpdatePlacementPose()
    {
        var screenCenter = sessionOrigin.camera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        aRRaycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            PlacementPose = hits[0].pose;
            var cameraForwad = sessionOrigin.camera.transform.forward;
            var cameraOrientation = new Vector3(cameraForwad.x,0, cameraForwad.z).normalized;
            PlacementPose.rotation = Quaternion.LookRotation(cameraOrientation);
        }
    }

    void ARPlaceObject()
    {
        spawnedObject = Instantiate(arObjectToSpawn, PlacementPose.position, PlacementPose.rotation);
    }

   /* private void OnTriggerEnter(Collision other)
    {
        if (other.name == "Player")
        {
            Destroy(go);
        }

      
    }*/

}
