using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour
{
    public BloonManager bloonManager;

    public Material[] bloonMaterials;

    int currentBloonTypeIndex;

    float speed;

    int currentWaypointIndex;
    Transform waypointParentObject;
    List<GameObject> waypoints;

    Dictionary<string,float> BloonTagToSpeedLookupTable = new Dictionary<string, float> {
        {"Red",10f},
        {"Blue",15f},
        {"Green",15f},
        {"Yellow",20f}
    };


    void Start() {

        bloonManager = new BloonManager();

        for (int i=0; i<bloonManager.bloonTypes.Length; i++) {
            if (gameObject.tag == bloonManager.bloonTypes[i])
                currentBloonTypeIndex = i;
        }

        speed = BloonTagToSpeedLookupTable[tag];
        waypointParentObject = GameObject.FindGameObjectWithTag("Waypoints").transform;
        waypoints = new List<GameObject>();
        for (int i = 0; i < waypointParentObject.childCount; i++) {
            waypoints.Add(waypointParentObject.GetChild(i).gameObject);
        }
        currentWaypointIndex = 0;
    }

    void Update() {
        //Collision with waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < .01f)
            currentWaypointIndex += 1;

        //Reached end of map
        if (currentWaypointIndex == waypoints.Count)
            Destroy(this.gameObject);
        else {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, step);
        }
    }

    public void BloonHitLogic() {
        currentBloonTypeIndex -= 1;
        if (currentBloonTypeIndex == -1)
            Destroy(gameObject);
        else {
            gameObject.tag = bloonManager.bloonTypes[currentBloonTypeIndex];
            gameObject.transform.GetComponentInChildren<MeshRenderer>().sharedMaterial = bloonMaterials[currentBloonTypeIndex];
            speed = BloonTagToSpeedLookupTable[bloonManager.bloonTypes[currentBloonTypeIndex]];
        }
    }
}
