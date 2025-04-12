using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CharacterNetworkManager : NetworkBehaviour
{

	CharacterManager character;

	[Header("Position")]
	public NetworkVariable<Vector3> networkPosition =	new NetworkVariable<Vector3>(Vector3.zero,
														NetworkVariableReadPermission.Everyone,
														NetworkVariableWritePermission.Owner);
	public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(Quaternion.identity,
															NetworkVariableReadPermission.Everyone,
															NetworkVariableWritePermission.Owner);
	public Vector3 networkPositionVelocity;
	public float networkPositionSmoothTime = 0.1f;
	public float networkRotationSmoothTime = 0.1f;

	[Header("Animator")]
	public NetworkVariable<float> animatorHorizontalMovement = 
				new NetworkVariable<float>(	0f, 
											NetworkVariableReadPermission.Everyone, 
											NetworkVariableWritePermission.Owner);

	public NetworkVariable<float> animatorVerticalMovement =
			new NetworkVariable<float>(0f,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);
	public NetworkVariable<float> animatorMoveAmount =
			new NetworkVariable<float>(0f,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);

	[Header("Flags")]
	public NetworkVariable<bool> isSprinting = new NetworkVariable<bool>(false, 
																NetworkVariableReadPermission.Everyone, 
																NetworkVariableWritePermission.Owner);

	[Header("Stats")]
	public NetworkVariable<int> endurance =
			new NetworkVariable<int>(1,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);
	public NetworkVariable<float> currentStamina =
			new NetworkVariable<float>(0,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);
	public NetworkVariable<int> maxStamina =
			new NetworkVariable<int>(0,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);

	public NetworkVariable<int> strength =
			new NetworkVariable<int>(0,
										NetworkVariableReadPermission.Everyone,
										NetworkVariableWritePermission.Owner);

	protected virtual void Awake()
	{
		character = GetComponent<CharacterManager>();
	}
	// LOGIC TO PLAY ACTION ANIMATION FOR ALL CLIENTS
	// request to the server rpc 
	[ServerRpc] 
	public void NotifyTheServerOfActionAnimationServerRpc(ulong clientID, string animationID, bool applyRootMotion)
	{
		if (IsServer)
		{
			PlayActionAnimationForAllClientsClientRpc( clientID,  animationID,  applyRootMotion);
		}
	}
	[ClientRpc]
	public void PlayActionAnimationForAllClientsClientRpc(ulong clientID, string animationID, bool applyRootMotion)
	{
		if(clientID != NetworkManager.Singleton.LocalClientId)
		{
			PerformActionAnimation(animationID, applyRootMotion);
		}
	}
	private void PerformActionAnimation(string animationID, bool applyRootMotion)
	{
		character.applyRootMotion = applyRootMotion;
		character.animator.CrossFade(animationID, 0.2f);
	}

}
