using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class KitchenObjectSo : ScriptableObject
{
    //脚本对象，规定了厨房对象需绑定的一些信息
    public Transform perfeb;
    public Sprite sprite;
    public string objectName;
}
