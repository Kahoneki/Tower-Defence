using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    RaycastHit hitInfo;

    public Camera gameCamera;

    public Transform nodeParent;
    List<MeshRenderer> nodes;

    public Material defaultNodeMaterial;
    public Material highlightedNodeMaterial;

    MeshRenderer nodeHighlighted;
    MeshRenderer nodeHighlightedLastFrame;

    public GameObject turret;
    public GameObject turretParent;

    void Start() {
        nodes = new List<MeshRenderer>();
        foreach (Transform node in nodeParent)
            nodes.Add(node.GetComponent<MeshRenderer>());
    }

    void Update() {
        Ray ray = gameCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, (1 << LayerMask.NameToLayer("Node")))) {
            HighlightNodeLogic();
            if (Input.GetMouseButtonDown(0))
                LeftClick();
        }

        //Player not hovering over a node so stop highlighting the highlighted node
        else
            if (nodeHighlighted != null) {
                nodeHighlighted.sharedMaterial = defaultNodeMaterial;
                nodeHighlighted = null;
            }
    }


    void HighlightNodeLogic() {
        nodeHighlighted = hitInfo.collider.gameObject.GetComponent<MeshRenderer>();
        if (nodeHighlighted != nodeHighlightedLastFrame && nodeHighlightedLastFrame != null) {
            nodeHighlightedLastFrame.sharedMaterial = defaultNodeMaterial;
            nodeHighlighted.sharedMaterial = highlightedNodeMaterial;
        }
        nodeHighlightedLastFrame = nodeHighlighted;
    }


    void LeftClick() {
        float halfTurretHeight = turret.transform.localScale.y/2;
        Vector3 position = hitInfo.collider.transform.position + Vector3.up*halfTurretHeight;

        //Stop player from being able to place multiple turrets on same node.
        bool existingTurretAtPos = false;
        foreach (Transform turretChild in turretParent.transform) {
            if (turretChild.position == position)
                existingTurretAtPos = true;
        }
        if (!existingTurretAtPos) {
            Transform turretObj = Instantiate(turret, position, transform.rotation).transform;
            turretObj.parent = turretParent.transform;
        }
        
    }
}