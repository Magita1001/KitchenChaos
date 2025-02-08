using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Player Player;
    private const string Is_Walking = "Iswalk";
    Animator anime;
    private void Awake()
    {
        anime = GetComponent<Animator>();        
    }

    private void Update()
    {
        anime.SetBool(Is_Walking, Player.IsWalk());
    }

}
