using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastViewer : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform aimOrigin, aimTarget;
    [SerializeField] float maxRange = 30f;

    void Update()
    {
        lineRenderer.SetPosition(0, aimOrigin.position);
        Vector3 pos = aimOrigin.position + ((aimTarget.position - aimOrigin.position).normalized * maxRange);
        //pos = aim.transform.position;
        lineRenderer.SetPosition(1, pos);
    }
}
