﻿using UnityEngine;

public class EntityHealth : BaseEntityAddon
{
	public float StartingHealth
	{
		get
		{
			return startingHealth;
		}
	}

	public float CurrentHealth { set; get; }

	[SerializeField] private float startingHealth;

	protected void Awake()
	{
		CurrentHealth = startingHealth;
	}

	protected void OnEnable()
	{
		Entity.Events.AddListener<RefillHealthEvent>(OnRefillHealthEvent);

		GlobalEvents.AddListener<DamageEvent>(OnDamageEvent);
	}

	protected void OnDisable()
	{
		Entity.Events.RemoveListener<RefillHealthEvent>(OnRefillHealthEvent);

		GlobalEvents.RemoveListener<DamageEvent>(OnDamageEvent);
	}

	private void OnDamageEvent(DamageEvent evt)
	{
		if(evt.DamageInfo.Hit.Target == Entity)
		{
			CurrentHealth -= evt.DamageInfo.Damage;
		}
	}

	private void OnRefillHealthEvent(RefillHealthEvent evt)
	{
		CurrentHealth = StartingHealth;
	}

#if UNITY_EDITOR
	[ContextMenu("Heal")]
	private void Heal()
	{
		CurrentHealth = StartingHealth;
	}

	[ContextMenu("Damage (1)")]
	private void Damage1()
	{
		Damage(1);
	}

	[ContextMenu("Damage (5)")]
	private void Damage5()
	{
		Damage(5);
	}

	private void Damage(float damage)
	{
		HitInfo hitInfo = new HitInfo(Entity, Entity);
		DamageInfo damageInfo = new DamageInfo(hitInfo, damage);

		GlobalEvents.Invoke(new DamageEvent(damageInfo));
	}
#endif
}