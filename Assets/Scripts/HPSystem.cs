using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HPSystem : MonoBehaviour
{
	[SerializeField] private int _maxHP;
	[SerializeField] private int _currentHP;

	private UnityEvent<int, int> OnUpdateMaxHP;
	private UnityEvent<int, int> OnTakeDamage;
	private UnityEvent<int, int> OnHeal;
	private UnityEvent<int, int> OnDeath;

//	public void OnUpdateMaxHP (int maxHP, int currentHP){
//		this._maxHP = maxHP;
//		this._currentHP = currentHP;
//
//		OnUpdateMaxHP.Invoke(maxHP, currentHP);
//	}

//	public void OnTakeDamage(int damage){
//		this._currentHP-= damage;
//		OnTakeDamage.Invoke(damage, this._currentHP);
//		if(this._currentHP <= 0){
//			OnDeath.Invoke();
//		}
//	}

//	public void OnHeal(int ammount){
//		this._currentHP += ammount;
//		if(this._currentHP > this._maxHP){
//			this._currentHP = this._maxHP;
//		}
//		OnHeal.Invoke(ammount, this._currentHP);
//	}
}
