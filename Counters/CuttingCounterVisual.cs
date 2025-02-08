using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour
{
    private Animator anime;
    private const string Cutting = "Cut";
    [SerializeField] private CuttingCounter cuttingCounter;
    private void Awake()
    {
        anime = this.GetComponent<Animator>();
    }
    private void Start()
    {
        cuttingCounter.OnCut += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, System.EventArgs e)
    {
        anime.SetTrigger(Cutting);
    }

}
