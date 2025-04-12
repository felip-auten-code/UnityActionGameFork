using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    private CharacterController _controller;

    [SerializeField]
    private float       _playerSpeed = 5f;

    [SerializeField]
    private float       _rotationSpeed = 10f;
    [SerializeField]
    private GameObject _swordOnShoulder;
    [SerializeField]
    private GameObject _sword;
    [SerializeField]
    private GameObject _swordOnHand;

    [SerializeField]
    private Camera      _followCamera;

    private Vector3     _playerVelocity;
    private bool        _groundedPlayer;

    [SerializeField]
    private float       _jumpHeight = 1.0f;
    [SerializeField]
    private float       _gravityValue = -9.81f;
    private Animator    animator;
    private int         jumper=0;
    private int         _comboCounter;
    private Vector3     movementInput;

    GameObject currentWeaponInSheath;
    GameObject currentWeaponInHand;

    private void Awake(){
        DontDestroyOnLoad(this);
    }

    private void Start() 
    {
        _controller = GetComponent<CharacterController>();    
        animator    = GetComponent<Animator>();
        currentWeaponInSheath = Instantiate(_sword, _swordOnShoulder.transform);
    }

    private void Update() 
    {
        Movement();    
        
    }

    public void WeaponSwitch(){
        Debug.Log("WeaponSwitch event triggered");
        if(animator.GetBool("isUnsheathed")){
            Debug.Log("WeaponSwitch event triggered");
            currentWeaponInHand = Instantiate(_sword, _swordOnHand.transform);
            Destroy(currentWeaponInSheath);
        }else{
            Debug.Log("WeaponSwitch event triggered");
            currentWeaponInSheath = Instantiate(_sword, _swordOnShoulder.transform);
            Destroy(currentWeaponInHand);
        }
    }
    private void UnsheathSheath(){
        
        if(Input.GetKeyDown(KeyCode.Q)){
            if(animator.GetBool("isUnsheathed")){
                animator.SetBool("isUnsheathed", false);    
                _playerSpeed = 5f;
            }
            else{
                animator.SetBool("isUnsheathed", true);     
                _playerSpeed = 2.5f;
            }
        }
    }

    public void Hit(){
        Debug.Log("Player Has Attacked");
    }

    private void SprintAttack(){
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            if(animator.GetBool("isSprint")){
                animator.SetTrigger("isSprintAttack");
            }else{
                animator.SetTrigger("Attack2");
            }

            //else animator.SetTrigger("Attack2");
        }
        
    }
    private void Roll(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            animator.SetTrigger("isRoll");
        }
        if(Input.GetKeyUp(KeyCode.LeftControl)){
            animator.ResetTrigger("isRoll");
        }
    }
    public void FootL(){
        Debug.Log("FootL event triggered");

    }
    public void FootR(){
        Debug.Log("FootR event triggered");

    }
    void Movement() 
    {

        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0) 
        {
            _playerVelocity.y = 0f;
        }

        float horizontalInput   = Input.GetAxis("Horizontal");
        float verticalInput     = Input.GetAxis("Vertical");

        movementInput = Quaternion.Euler(0, _followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 movementDirection = movementInput.normalized;

        _controller.Move(movementDirection * _playerSpeed * Time.deltaTime);

        if (movementDirection != Vector3.zero) 
        {
            Quaternion desiredRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        if ((Input.GetKey(KeyCode.Space)) && _groundedPlayer)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3.0f * _gravityValue);
        }

        // Trigger Animations
        WalkNRun();
        Jump();
        Roll();
        UnsheathSheath();
        SprintAttack();

        _playerVelocity.y += _gravityValue * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);

    }

    private void WalkNRun(){
        if(movementInput != Vector3.zero){
            animator.SetBool("isWalking", true);
            if(Input.GetKey(KeyCode.LeftShift)){
                animator.SetBool("isSprint", true);
                _playerSpeed = 9.0f;
            }else{
                animator.SetBool("isSprint", false);
                if(animator.GetBool("isUnsheathed")){
                    _playerSpeed = 2.5f;
                }else
                    _playerSpeed = 5f;
            }
        }else{
            animator.SetBool("isWalking", false);
        }
    }

    private void Jump(){
        if(Input.GetKey(KeyCode.Space)){
            animator.SetTrigger("isJump");
        }
        if(animator.GetBool("isJump")){
            jumper+=1;
        }
        if(jumper > 30){
            jumper = 0;
            animator.ResetTrigger("isJump");
        }
    }
}
