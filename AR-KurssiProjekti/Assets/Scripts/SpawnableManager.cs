using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnableManager : MonoBehaviour
{
    GameObject spawnableObject;
    public GameObject spawnablePrefab;
    Camera ARcamera;

    public ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Start is called before the first frame update
    void Start()
    {
        spawnableObject = null;
        ARcamera = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = ARcamera.ScreenPointToRay(Input.GetTouch(0).position);

        if(Input.touchCount == 0)
        {
            return;
        }

        if (raycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && spawnableObject == null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnableObject = hit.collider.gameObject;
                        spawnableObject.GetComponent<MeshRenderer>().material.color = new Color(2, 4, 5);
                    }
                    else
                    {
                        SpawnPrefab(hits[0].pose.position);
                    }
                }
            }

            else if(Input.GetTouch(0).phase == TouchPhase.Moved && spawnableObject != null)
            {
                spawnableObject.transform.position = hits[0].pose.position;
            }

            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                spawnableObject = null;
            }
        }

    }

    public void SpawnPrefab(Vector3 spawnPosition)
    {
        spawnableObject = Instantiate(spawnablePrefab, spawnPosition, Quaternion.identity);
    }
}
