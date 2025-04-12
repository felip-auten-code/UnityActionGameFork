using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class CharacterAnimatorManager : MonoBehaviour
{

	public CharacterManager character;
	float vertical;
	float horizontal;
	protected virtual void Awake(){
		character = GetComponent<CharacterManager>();
	}
	public void UpdateaAnimatorMovementParameters(float horizontalValue, float verticalValue)
	{

		if (character.characterNetworkManager.isSprinting.Value)
		{
			verticalValue = 2;
		}

		character.animator.SetFloat("Horizontal", horizontalValue, 0.01f, Time.deltaTime);
		character.animator.SetFloat("Vertical", verticalValue, 0.01f, Time.deltaTime);
	}

	public virtual void PlayTargetActionAnimation(	string targetAnimation, 
													bool isPerformingAction, 
													bool applyRootMotion=true, 
													bool canRotate=false, 
													bool canMove=false){
		character.animator.applyRootMotion = applyRootMotion;
		character.applyRootMotion = applyRootMotion;
		// Can be used to stop a character from performing new actions
		// When player is damaged this flag will be true if is Stunned
		// Check for this before attempting some actions
		character.isPerformingAction = isPerformingAction;
		character.canRotate = canRotate;
		character.canMove = canMove;
		character.animator.CrossFade(targetAnimation, 0.2f);

		// TELL THE host we need to play an animation , and to play for everyone else present
		character.characterNetworkManager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
	}
}
