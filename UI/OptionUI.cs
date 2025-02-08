using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionUI : MonoBehaviour
{
    public static OptionUI Instance { get; private set; }

    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;

    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;    
    [SerializeField] private TextMeshProUGUI PauseText;

    [SerializeField] private Button moveUPBtn;
    [SerializeField] private Button moveDownBtn;
    [SerializeField] private Button moveLeftBtn;
    [SerializeField] private Button moveRightBtn;
    [SerializeField] private Button interactBtn;
    [SerializeField] private Button interactAlternateBtn;
    [SerializeField] private Button PauseBtn;

    [SerializeField] private Button CloseButton;

    [SerializeField] private Transform pressToRebindKey;
    private void Awake()
    {
        Instance = this;

        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        });

        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        CloseButton.onClick.AddListener(() =>
        {
            Hide();
        });

        moveUPBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Up); });
        moveDownBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Down); });
        moveLeftBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Left); });
        moveRightBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Move_Right); });
        interactBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interect); });
        interactAlternateBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InterectAlternate); });
        PauseBtn.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause); });
    }
    private void Start()
    {
        GameManager.Instance.OnUnPause += Instance_OnUnPause;
        UpdateVisual();
        HidePressToRebindKey();
        Hide();
    }

    private void Instance_OnUnPause(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "音效：" + Mathf.Round(SoundManager.Instance.GetVolume() * 10);
        musicText.text = "音乐：" + Mathf.Round(MusicManager.Instance.GetVolume() * 10);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interect);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InterectAlternate);
        PauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }
    public  void Show()
    {
        this.gameObject.SetActive(true);
    }
    private void Hide()
    {
        this.gameObject.SetActive(false);
    }
    private void ShowPressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKey.gameObject.SetActive(false);
    }
    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }
}
