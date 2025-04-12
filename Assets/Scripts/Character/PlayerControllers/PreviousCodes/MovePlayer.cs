using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed = 1.90f;
    //private float rotSpeed = 2.0f;
    private CharacterController Player;
    private  float movHorizontal;
    private  float movVertical;
    private Vector3 movePlayer;
    private Animator animator;
    private int jumper=0;
     // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        Debug.Log("Método Start\n");
    }

    // Update is called once per frame
    void Update()
    {
        movHorizontal = Input.GetAxis("Horizontal");
        movVertical = Input.GetAxis("Vertical");
        movePlayer = new Vector3(movHorizontal, 0, movVertical);
        Player.Move(movePlayer.normalized * Time.deltaTime * moveSpeed);


        if(movePlayer != Vector3.zero){
            animator.SetBool("isWalking", true);
            Quaternion TargetRotation = Quaternion.LookRotation(movePlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRotation, Time.deltaTime * 19);
            if(Input.GetKey(KeyCode.LeftShift)){
                animator.SetBool("isSprint", true);
                moveSpeed = 5.0f;
            }else{
                animator.SetBool("isSprint", false);
                moveSpeed = 1.9f;
            }
        }else{
            animator.SetBool("isWalking", false);
        }

        if(Input.GetKey(KeyCode.Space)){
            animator.SetTrigger("isJump");
        }
        if(animator.GetBool("isJump")){
            jumper+=1;
        }
        if(jumper > 30){
            jumper =0;
            animator.ResetTrigger("isJump");
        }
        
        //Debug.Log(animator.GetAnimatorTransitionInfo(3))
        // if(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
        //     Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        //     animator.ResetTrigger("isJump");
        // }
        
    }
}
