using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "가방",menuName = "ScriptableObject/BackPack", order = int.MaxValue)]
public class SOBackPack : ScriptableObject
{
    public string BackPackName ;
    public Sprite BackImage;
    public string BackPackDescription;
    public int MaxStackCount;
}
