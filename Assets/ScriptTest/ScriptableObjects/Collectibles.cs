using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TargetBuff {single,party}
public abstract class Collectibles : ScriptableObject
{
    public TargetBuff target;
    public Sprite sprite;

    public abstract void ApplyEffect(Transform parent);

}
