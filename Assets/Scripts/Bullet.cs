using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    float speed;
    Turret turret;

    void Start() {
        speed = 100f;
        turret = GetComponentInParent<Turret>();
        transform.LookAt(turret.shootingTypes[turret.shootingIndex] + Vector3.up*4);
    }

    void Update() {
        transform.position += transform.forward * Time.deltaTime * speed;
        OutOfBoundsCheck();
    }


    void OutOfBoundsCheck() {
        if (Mathf.Abs(transform.position.x) >= 150 || Mathf.Abs(transform.position.y) >= 150 || Mathf.Abs(transform.position.z) >= 150)
            Destroy(gameObject);
    }


    void OnCollisionEnter(Collision other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bloon")) {
            other.gameObject.GetComponent<Bloon>().BloonHitLogic();
            Destroy(gameObject);
        }
    }
}
