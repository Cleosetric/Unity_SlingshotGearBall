using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    #region Singleton
	private static PartyManager _instance;
	public static PartyManager Instance { get { return _instance; } }

	private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }
	#endregion

    public List<HeroBase> party = new List<HeroBase>();
    [SerializeField] public HeroBase currentHero;
    [SerializeField] private Player player;

    private int currentHeroIndex = 0;
    
    public HeroBase GetActiveHero(){
        return currentHero;
    }

    public void SwitchHero(){
        currentHeroIndex ++;
        if(currentHeroIndex >= party.Count){
            currentHeroIndex = 0;
        }
        currentHero = party[currentHeroIndex];
        player.RefreshHero();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        currentHero = party[currentHeroIndex];
    }
}
