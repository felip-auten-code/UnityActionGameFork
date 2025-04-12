using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI_HUDManager : MonoBehaviour
{
    PlayerUI_HUDManager instance;
    [SerializeField] UI_StatBar staminaBar;

    private void Awake()
    {
		if(instance == null){
			instance = this;
		}else{
			Destroy(gameObject);
		}        

    }
    public void Start()
    {
        
    }

    public void SetNewStaminaValue(float previousValue, float newValue){
        staminaBar.SetStat(Mathf.RoundToInt(newValue));
    }
    public void SetMaxStaminaValue(float newValue){
        staminaBar.SetMaxStat(Mathf.RoundToInt(newValue));
    }
}
