using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class NextGameTimer : MonoBehaviour
{
    [SerializeField] private TMP_InputField gametimerText;
    private void Start()
    {
        gametimerText.text = GameManager.Instance.GetGamePlayingTimerMax().ToString();

        gametimerText.onEndEdit.AddListener((inputTime) =>
        {
            if (float.TryParse(inputTime, NumberStyles.Float, CultureInfo.InvariantCulture, out float parsedTime)) 
            {
                // 转换成功时设置时间
                GameManager.Instance.SetGamePlayingTimerMax(parsedTime);
            }
        });
    }
    
}
