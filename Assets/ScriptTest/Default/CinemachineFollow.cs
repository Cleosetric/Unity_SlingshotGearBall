using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CinemachineFollow : MonoBehaviour
{
    Party party;
    Actor actor;
    CinemachineVirtualCamera camera2D;

    public float maxCam;
    public float minCam;
    public float timeDuration;

    // Start is called before the first frame update
    void Start()
    {
        camera2D = GetComponent<CinemachineVirtualCamera>();
        party = Party.Instance;
        party.partyOnActorChanged += OnActorChanged;
        actor = party.GetLeader();
        camera2D.Follow = actor.parent.transform;
    }

    private void OnActorChanged()
    {
        actor = party.GetLeader();
        if(actor != null)
        camera2D.Follow = actor.parent.transform;
    }

    public void StartZoomIn(){
        StartCoroutine(ZoomIn());
    }

    public void StartZoomOut(){
        StartCoroutine(ZoomOut());
    }

    IEnumerator ZoomIn(){
        float timeElapsed = 0;
        while (timeElapsed < timeDuration)
        {
            camera2D.m_Lens.OrthographicSize = Mathf.Lerp(maxCam, minCam, timeElapsed / timeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        camera2D.m_Lens.OrthographicSize = minCam;
        OnActorChanged();
    }

    IEnumerator ZoomOut(){
        float timeElapsed = 0;
        while (timeElapsed < timeDuration)
        {
            camera2D.m_Lens.OrthographicSize = Mathf.Lerp(minCam, maxCam, timeElapsed / timeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        camera2D.m_Lens.OrthographicSize = maxCam;
        OnActorChanged();
    }

}
