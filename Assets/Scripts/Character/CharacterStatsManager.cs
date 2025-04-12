using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterStatsManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Stamina Regenaretaion")]
	private float staminaRegenerationTimer = 0;
	[SerializeField] float staminaRegenarationDelay = 2;
	private float staminaTickTimer = 0;

	[SerializeField] float staminaRegenAmount = 2;

    protected virtual void Awake(){
        character = GetComponent<CharacterManager>();
    }

	//public bool
    // Start is called before the first frame update
    public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
    {
        float stamina = 0;

        stamina = endurance * 10;
        return Mathf.RoundToInt(stamina);
    }
	public void RegenerateStamina(){
		if(!character.IsOwner){
			return;
		}
		// Only owner can edit network variables
		if (character.characterNetworkManager.isSprinting.Value){
			return;
		}
		if(character.isPerformingAction)
			return;

		staminaRegenerationTimer += Time.deltaTime;
		if(staminaRegenerationTimer >= staminaRegenarationDelay){
			if(character.characterNetworkManager.currentStamina.Value <= character.characterNetworkManager.maxStamina.Value){
				staminaTickTimer += Time.deltaTime;
				if(staminaTickTimer >= 0.1){
					staminaTickTimer = 0;
					character.characterNetworkManager.currentStamina.Value += Mathf.RoundToInt(staminaRegenAmount);
				}
			}
		}
	}

	public virtual void ResetRegenTime(float oldValue, float newValue){
		if(newValue < oldValue)
			staminaRegenerationTimer = 0;
	}
}
