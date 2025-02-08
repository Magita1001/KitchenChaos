using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour,IKenchenParent
{
    private static Player instance;
    public static Player Instance => instance;
    
    
    //用于计算移动向量
    private Vector3 moveDir;
    //移动速度/旋转速度 SerializeField可以序列化private的属性，使其在inspector窗口中可见
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float rotateSpeed = 10f;   
    //控制旋转
    Quaternion RotationFwd;

    //走路状态机
    private bool isWalk;
    //是否可以移动
    bool canMove;
    //移动位置管理类
    [SerializeField] private GameInput GameInput;
    //玩家检测范围宽高
    float playerRadius = 0.7f;
    float playerHeight = 2f;
    private float moveDistance;

    //最后移动的位置
    Vector3 lastInteractDir;
    //射线检测击中的目标
    RaycastHit raycast = new RaycastHit();    
    //层级选择
    [SerializeField] private LayerMask counterLayerMask;

    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    [SerializeField] private Transform kitchenObjectHoldPoint;    


    //public UnityAction<Player, OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter selectedCounter;        
    }
    public event EventHandler OnPickedSomething;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("有多个玩家实例");
        }
        instance = this;
    }
    private void Start()
    {
        GameInput.OnInteractAction += GameInput_OnInteractAction;
        GameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(GameInput arg0, System.EventArgs arg1)
    {
        if (!GameManager.Instance.IsPlayGame()) return;

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
        moveDir = GameInput.GetMoveVector();     
    }

    private void GameInput_OnInteractAction(GameInput arg0, System.EventArgs arg1)
    {
        if (!GameManager.Instance.IsPlayGame()) return;
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
        moveDir = GameInput.GetMoveVector();     
    }

    // Update is called once per frame
    void Update()
    {
        #region 无用
        //this.transform.Translate(Vector3.forward * moveSpeed * Input.GetAxisRaw("Vertical") * Time.deltaTime, Space.World);
        //this.transform.Translate(Vector3.right * moveSpeed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, Space.World);

        ////角色移动的位置
        //moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        ////归一化处理
        //moveDir.Normalize();
        #endregion
        HandleMovement();
        HandleInteractions();
    }

    /// <summary>
    /// 是否处于走路状态 
    /// </summary>
    /// <returns></returns>
    public bool IsWalk()
    {
        return isWalk;
    }
   
    /// <summary>
    /// 检测交互
    /// </summary>
    private void HandleInteractions()
    {
        moveDir = GameInput.GetMoveVector();
        if (moveDir != Vector3.zero) 
        {
            lastInteractDir = moveDir;
        }

        if (Physics.Raycast(this.transform.position, lastInteractDir, out raycast, 2f, counterLayerMask)) 
        {
            if (raycast.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                //如果射线检测到目标，就从目标组件里试图得到BaseCounter
                //BaseCounter.Interact();
                if (baseCounter != selectedCounter)
                {
                    SetSelectedCounter(baseCounter);                   
                }
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }
  
    /// <summary>
    /// 处理移动
    /// </summary>
    private void HandleMovement()
    {
        //移动速度
        moveDistance = moveSpeed * Time.deltaTime;
        //移动位置
        moveDir = GameInput.GetMoveVector();
        //状态机走路动画控制
        isWalk = moveDir != Vector3.zero;

        canMove = !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);
        if (!canMove)
        {
            //无法移动时，尝试只在x或z轴上移动
            Vector3 moveDirXorZ = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDirXorZ, moveDistance);
            if (canMove)
            {
                //this.transform.Translate(moveDirX * moveSpeed * Time.deltaTime, Space.World);
                moveDir = moveDirXorZ;
            }
            else
            {
                //尝试只在z轴上移动
                moveDirXorZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 &&  !Physics.CapsuleCast(this.transform.position, this.transform.position + Vector3.up * playerHeight, playerRadius, moveDirXorZ, moveDistance);
            }
            if (canMove)
            {
                moveDir = moveDirXorZ;
            }
        }
        if (canMove)
        {
            this.transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.World);
        }
               
        //移动位置不为0时，启用插值运算
        if (moveDir != Vector3.zero) 
        {
            //this.transform.forward = Vector3.Lerp(this.transform.forward, moveDir, Time.deltaTime * rotateSpeed);
           
            //通过四元数插值运算让物体看向目标位置
            RotationFwd = Quaternion.LookRotation(moveDir);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, RotationFwd, Time.deltaTime * rotateSpeed);
        }
    }
   
    /// <summary>
    /// 处理选中物体
    /// </summary>
    private void SetSelectedCounter(BaseCounter clearCounter)
    {
        this.selectedCounter = clearCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }

    public Transform GetKitchenObjectFollowTransForm()
    {
        return kitchenObjectHoldPoint;
    }
    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
        if (kitchenObject != null)
        {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }
    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }
    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }
    public bool HasKitchenobject()
    {
        return kitchenObject != null;
    }
}