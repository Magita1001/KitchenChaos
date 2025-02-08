using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDING = "InputBindings";
    public static GameInput Instance { get; private set; }

    private PlayerInputAction inputActions;
    
    public event UnityAction<GameInput,EventArgs> OnInteractAction;
    public event UnityAction<GameInput,EventArgs> OnInteractAlternateAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnbindingRebind;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interect,
        InterectAlternate,
        Pause
    }


    private void Awake()
    {
        Instance = this;
        inputActions = new PlayerInputAction();
        inputActions.Player.Enable();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING))
        {
            inputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING));
        }

        inputActions.Player.Interact.performed += Interact_performed;
        inputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        inputActions.Player.Pause.performed += Pause_performed;
    }
    private void OnDestroy()
    {
        inputActions.Player.Interact.performed -= Interact_performed;
        inputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        inputActions.Player.Pause.performed -= Pause_performed;

        inputActions.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseAction?.Invoke(this,EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    //计算移动位置的函数
    public Vector3 GetMoveVector()
    {
        //移动位置
        Vector3 moveDir = inputActions.Player.Move.ReadValue<Vector3>();
        
        //角色移动的位置
        //moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //归一化处理

        moveDir.Normalize();

        return moveDir;       
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return inputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return inputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return inputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return inputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interect:
                return inputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InterectAlternate:
                return inputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return inputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }
    /// <summary>
    /// 重新绑定按键
    /// </summary>
    /// <param name="binding"></param>
    public void RebindBinding(Binding binding, Action onActionRebind)
    {
        inputActions.Player.Disable();

        InputAction inputAction;
        int bindingdex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = inputActions.Player.Move;
                bindingdex = 1;
                break;
            case Binding.Move_Down:
                inputAction = inputActions.Player.Move;
                bindingdex = 2;
                break;
            case Binding.Move_Left:
                inputAction = inputActions.Player.Move;
                bindingdex = 3;
                break;
            case Binding.Move_Right:
                inputAction = inputActions.Player.Move;
                bindingdex = 4;
                break;
            case Binding.Interect:
                inputAction = inputActions.Player.Interact;
                bindingdex = 0;
                break;
            case Binding.InterectAlternate:
                inputAction = inputActions.Player.InteractAlternate;
                bindingdex = 0;
                break;
            case Binding.Pause:
                inputAction = inputActions.Player.Pause;
                bindingdex = 0;
                break;
        }

         inputAction.PerformInteractiveRebinding(bindingdex).OnComplete((callback) =>
         {
             callback.Dispose();
             inputActions.Player.Enable();
             onActionRebind();
             PlayerPrefs.SetString(PLAYER_PREFS_BINDING, inputActions.SaveBindingOverridesAsJson());
             PlayerPrefs.Save();

             OnbindingRebind?.Invoke(this, EventArgs.Empty);
         }).Start();
    }
}
