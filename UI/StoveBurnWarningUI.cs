using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        Hide();
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgessChangendEventArgs e)
    {
        float burnShowProgessAmount = .5f;
        bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgessAmount;
        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
