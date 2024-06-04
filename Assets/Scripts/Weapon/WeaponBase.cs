using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : CanPause
{
	[SerializeField] protected BaseWeaponScriptableObject wep;
	[SerializeField] protected float timeToDeactivate;

	protected BaseCombat myCombat;

	protected float timeSinceLastAttack = 999.0f;
	protected bool active;

	public bool Active => active;

	public BaseWeaponScriptableObject Weapon => wep;

	public void SetMyCombat(BaseCombat newCombat)
	{
		myCombat = newCombat;
	}
	

	public virtual bool Fire()
	{
		if (!CanFire()) return false;
		
		return true;
	}

	protected bool CanFire()
	{
		bool val = timeSinceLastAttack > wep.timeBetweenAttacks;
		if (val) timeSinceLastAttack = 0;
		return val;
	}

	protected virtual void SendDamageInfo(BaseDamageable hit, int damage)
	{
		HitInfo hitInfo = new HitInfo()
		{
			damage = damage,
			shooter = myCombat,
			weapon = this
		};
		
		hit.TakeHit(hitInfo);
	}
	
	protected override void FrameTick()
	{
		timeSinceLastAttack += Time.deltaTime;

		active = timeToDeactivate > timeSinceLastAttack;
	}

	protected List<BaseDamageable> SearchForDamageable(List<Collider> hits)
	{
		List<GameObject> gameObjects = new List<GameObject>();
		foreach (var hit in hits)
		{
			gameObjects.Add(hit.gameObject);
		}

		return SearchForDamageable(gameObjects.ToArray());
	}

	protected List<BaseDamageable> SearchForDamageable(GameObject[] hits)
	{
		var correctHits = new List<BaseDamageable>();
		
		foreach (var hit in hits)
		{
			bool containsKey = BaseDamageable.AllDamageable.ContainsKey(hit.GetInstanceID());
			if (!containsKey) continue;
			
			correctHits.Add(BaseDamageable.AllDamageable[hit.GetInstanceID()]);
		}
		
		return correctHits;
	}

	protected BaseDamageable SearchForDamageable(GameObject hit, out bool found)
	{
		found = false;
		bool containsKey = BaseDamageable.AllDamageable.ContainsKey(hit.GetInstanceID());
		if (!containsKey) return null;

		found = true;
		return BaseDamageable.AllDamageable[hit.GetInstanceID()];
	}
}
