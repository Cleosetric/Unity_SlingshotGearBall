using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    #region Singleton
	private static Party _instance;
	public static Party Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }

       Init();
    }
	#endregion

    public delegate void PartyOnActorChanged();
    public PartyOnActorChanged partyOnActorChanged;

    Actor[] actors;
    Actor activeActor;
    private int currentIndex = 0;

    void Init()
    {
        actors = gameObject.GetComponentsInChildren<Actor>();
        activeActor = actors[currentIndex];
    }

    public Actor getActiveActor(){
        return activeActor;
    }

    public void setActor(int index){
        activeActor = actors[index];
        currentIndex = index;
        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }

    public void switchActor(){
        currentIndex ++;
        if(currentIndex >= actors.Length){
            currentIndex = 0;
        }
        setActor(currentIndex);
    }

    public void nextActor(){
        if(currentIndex < (actors.Length - 1))
        {
            currentIndex ++;
            setActor(currentIndex);
        }   
    }

    public void prevActor(){
        if(currentIndex > 0)
        {
            currentIndex --;
            setActor(currentIndex);
        }   
    }

    public void ActorGainExp(int value){
        getActiveActor().GainExp(value);
        if(partyOnActorChanged != null) partyOnActorChanged.Invoke();
    }
}
