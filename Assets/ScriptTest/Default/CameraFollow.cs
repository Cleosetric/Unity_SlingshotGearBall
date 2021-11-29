using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform bound;
    public Vector3 offset = new Vector3(0, 90, -45);
    public int duration = 30;
    public float rect = 2f;
    public float zoomIn = 10f;
    public float zoomOut = 15f;

    void LateUpdate()
    {
        UpdateZoom();
        if (Boundary())
        {
            UpdateTrack();
        } else
        {
            UpdateTrack();
        }
    }

    void UpdateZoom()
    {
        float zooming = (!bound) ? zoomOut : zoomIn;
        Camera.main.orthographicSize = (Camera.main.orthographicSize * (duration - 1) + zooming) / duration;
    }

    void UpdateTrack()
    {
        int d = duration;
        Vector3 c = transform.position;
        Vector3 t = Party.Instance.GetLeader().transform.position + offset;
        Vector3 track = new Vector3((c.x * (d - 1) + t.x) / d, (c.y * (d - 1) + t.y) / d, -10);
        transform.position = track;
        transform.LookAt(track);
    }

    bool Boundary()
    {
        Vector3 pl = Party.Instance.GetLeader().transform.position;
        if (bound)
        {
            float sx = bound.position.x - rect;
            float sz = bound.position.z - rect;
            float ex = bound.position.x + rect;
            float ez = bound.position.z + rect;
            return pl.x == Mathf.Clamp(pl.x, sx, ex) && pl.z == Mathf.Clamp(pl.z, sz, ez);
        }
        else return false;
    }
}
