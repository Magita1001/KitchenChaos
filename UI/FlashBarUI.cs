using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBarUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool("IsFlash", false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgessChangendEventArgs e)
    {
        float burnShowProgessAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgessAmount;
        animator.SetBool("IsFlash", show);
    }
}
