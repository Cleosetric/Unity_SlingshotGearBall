using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Charachter
{
    public delegate void OnActorStatChanged();
    public OnActorStatChanged onActorStatChanged;

    [Space]
    [Header("Actor Info")]
    public ActorStats charachter;
    public ActorClass actorClass;
    public string actorName;
    public string actorDesc;
    public Sprite actorSprite;
    public Sprite actorFace;
    public float curveStat;

    [Space]
    [Header("Actor Status")]
    public int currentHP;
    public int currentSP;
    public int currentLevel;
    public int maxLevel = 10;
    public int currentExp;
    public int[] nextLevelExp;

    [Space]
    [Header("Actor Equipment")]
    public Equipment[] equipment;

    [Space]
    [Header("Control Parameter")]
    public int actionSight;
    public int actionCost;

    private Coroutine regen;

    private void Awake()
    {
        LoadData();
    }

    private void LoadData() {
        //Setup Info
        actorName = charachter.name;
        actorDesc = charachter.description;
        actorClass = charachter.actorClass;
        actorSprite = charachter.actorSprite;
        actorFace = charachter.actorFace;
        curveStat = charachter.growthCurve;

        //Setup Stats
        statMHP.SetValue(charachter.MHP.getValue());
        statMSP.SetValue(charachter.MSP.getValue());
        statATK.SetValue(charachter.ATK.getValue());
        statDEF.SetValue(charachter.DEF.getValue());
        statAGI.SetValue(charachter.AGI.getValue());
        statHRG.SetValue(charachter.HRG.getValue());
        statSRG.SetValue(charachter.SRG.getValue());

        statHIT.SetValue(charachter.HIT.getValue());
        statCRI.SetValue(charachter.CRI.getValue());
        statEVA.SetValue(charachter.EVA.getValue());
        statHRR.SetValue(charachter.HRR.getValue());
        statSRR.SetValue(charachter.SRR.getValue());

        currentHP = statMHP.getValue();
        currentSP = statMSP.getValue();

        int numSlot = System.Enum.GetNames(typeof(EquipmentSlot)).Length;
        equipment= new Equipment[numSlot];

        //Setup Control Param
        actionSight = charachter.actionSight;
        actionCost = charachter.actionCost;

        nextLevelExp = new int[maxLevel + 1];
        nextLevelExp[1] = 100;
        for (int i = 2; i < maxLevel; i++)
        {
            nextLevelExp[i] = Mathf.RoundToInt(nextLevelExp[i - 1] * 1.2f);
        }
    }

    private void Start() {
        // EquipManager.Instance.onEquipmentChanged += OnEquipsChanged;
    }

    public override void ApplyDamage(int damage)
    {
        damage -= statDEF.getValue();
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        currentHP -= damage;
        if(currentHP <= 0){
            currentHP = 0;
            Die();
        }   
    }

    public override void ApplyAction(int energy)
    {
        if(currentSP >= energy){
            currentSP -= energy;
            if(currentSP <= 0) currentSP = 0;
            StartRegen();
        }
    }

    public void OnItemEquiped(Equipment newItem)
    {
        if(newItem != null ){
            statMHP.AddModifier(newItem.modMHP);
            statMSP.AddModifier(newItem.modMSP);
            statATK.AddModifier(newItem.modATK);
            statDEF.AddModifier(newItem.modDEF);
            statAGI.AddModifier(newItem.modAGI);
            statHRG.AddModifier(newItem.modHRG);
            statSRG.AddModifier(newItem.modSRG);

            statHIT.AddModifier(newItem.modHIT);
            statCRI.AddModifier(newItem.modCRI);
            statEVA.AddModifier(newItem.modEVA);
            statHRR.AddModifier(newItem.modHRR);
            statSRR.AddModifier(newItem.modSRR);
        }
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
    }

    public void OnItemUnequiped(Equipment oldItem){
        
        if(oldItem != null){
            statMHP.RemoveModifier(oldItem.modMHP);
            statMSP.RemoveModifier(oldItem.modMSP);
            statATK.RemoveModifier(oldItem.modATK);
            statDEF.RemoveModifier(oldItem.modDEF);
            statAGI.RemoveModifier(oldItem.modAGI);
            statHRG.RemoveModifier(oldItem.modHRG);
            statSRG.RemoveModifier(oldItem.modSRG);

            statHIT.RemoveModifier(oldItem.modHIT);
            statCRI.RemoveModifier(oldItem.modCRI);
            statEVA.RemoveModifier(oldItem.modEVA);
            statHRR.RemoveModifier(oldItem.modHRR);
            statSRR.RemoveModifier(oldItem.modSRR);
        }
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Q)){
            ApplyDamage(statATK.getValue());
        }
        
        if(Input.GetKeyDown(KeyCode.W)){
            ApplyAction(actionCost);
        }    

        if(Input.GetKeyDown(KeyCode.E)){
            GainExp(50);
        }
    }

    public void GainExp(int experience){
        currentExp += experience;
        if(currentExp >= nextLevelExp[currentLevel] && currentLevel < maxLevel){
            LevelUp();
        }

        if(currentLevel >= maxLevel) currentExp = 0;
        if(onActorStatChanged != null) onActorStatChanged.Invoke();
        Debug.Log(actorName+" gainExp!");
    }

    private void LevelUp()
    {
        currentExp -= nextLevelExp[currentLevel];
        currentLevel++;
        IncreaseActorStat(curveStat);
    }

    private void IncreaseActorStat(float curve)
    {
        statMHP.SetValue(Mathf.RoundToInt(statMHP.getValue() * curve));
        statMSP.SetValue(Mathf.RoundToInt(statMSP.getValue() * curve));
        statATK.SetValue(Mathf.RoundToInt(statATK.getValue() * curve));
        statDEF.SetValue(Mathf.RoundToInt(statDEF.getValue() * curve));
        statAGI.SetValue(Mathf.RoundToInt(statAGI.getValue() * curve));

        currentHP = statMHP.getValue();
        currentSP = statMSP.getValue();
    }

    private void StartRegen(){
        if(regen != null) StopCoroutine(regen);
        regen = StartCoroutine(RegenerateStamina());
    }

    private IEnumerator RegenerateStamina(){
        yield return new WaitForSeconds(statSRR.getValue());

        while (currentSP < statMSP.getValue())
        {
            currentSP += statSRG.getValue();
            yield return new WaitForSeconds(0.25f);
        }
        regen = null;
    }

    public void Die()
    {
        Debug.Log(charachter.name + " Died");
    }
}
