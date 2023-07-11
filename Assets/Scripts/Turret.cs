using System.Collections;
using System.Collections.Generic;
using System.Linq; //For Array.Contains()
using UnityEngine;

public class Turret : MonoBehaviour
{

    float rangeRadius;

    public GameObject bullet;
    float delayBetweenShots;
    float currentDelay;
    bool shotReady;

    
    [HideInInspector] public int shootingIndex; //0: First, 1: Close
    [HideInInspector] public Vector3[] shootingTypes;
    Transform bloonParent;


    void Start() {
        rangeRadius = 20;
        delayBetweenShots = 1f;
        currentDelay = 0f;
        shotReady = true;

        shootingIndex = 0;
        shootingTypes = new Vector3[2] {Vector3.zero, Vector3.zero};
        bloonParent = GameObject.FindGameObjectWithTag("BloonParent").transform;
    }

    void Update() {
        ShootLogic();
    }

    void ShootLogic() {
        if (currentDelay <= 0f) {
            shotReady = true;
            currentDelay = delayBetweenShots;
        }
        currentDelay -= Time.deltaTime;

        if (shotReady)
            if (DetectBloonInRange()) {
                shotReady = false;
                Transform bulletObj = Instantiate(bullet, transform.position, transform.rotation).transform;
                bulletObj.parent = transform;
            }
    }

    //Returns true if there is a bloon within range then updates the firstBloon and closestBloon variable accordingly.
    bool DetectBloonInRange() {
        Collider[] bloonsInRange = Physics.OverlapSphere(transform.position, rangeRadius, (1 << LayerMask.NameToLayer("Bloon")));
        if (bloonsInRange.Length != 0) {

            switch (shootingIndex) {
                case 0:
                    //Searching for first bloon that's in range
                    Collider currentBloon = bloonParent.GetChild(0).gameObject.GetComponent<Collider>();
                    int counter = 1;
                    while (!bloonsInRange.Contains(currentBloon) && counter < transform.childCount) {
                        counter += 1;
                        currentBloon = bloonParent.GetChild(counter).gameObject.GetComponent<Collider>();
                        print(counter);
                    }
                    shootingTypes[0] = currentBloon.transform.position;
                    break;

                case 1:
                    //Searching for closest bloon
                    shootingTypes[1] = bloonsInRange[0].transform.position;
                    float distanceToClosestBloon = Vector3.Distance(transform.position, shootingTypes[1]);
                    foreach (Collider collider in bloonsInRange) {
                        float distanceToBloon = Vector3.Distance(transform.position, collider.transform.position);
                        if (distanceToBloon < distanceToClosestBloon) {
                            distanceToClosestBloon = distanceToBloon;
                            shootingTypes[1] = collider.transform.position;
                        }
                    }
                    break;
            }
            return true;
        }
        return false;
    }
}
