﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Abilities
{
    public abstract class AbilityConfig : ScriptableObject
    {
        [Header("Spcial Ability General")]
        [SerializeField] float energyCost = 10f;
        [SerializeField] GameObject particlePrefab;
        [SerializeField] AnimationClip abilityAnimation;
        [SerializeField] AudioClip[] audioClips;

        protected AbilityBehaviour behaviour;

        public abstract AbilityBehaviour GetBehaviourComponent(GameObject objectToAttachTo);

        public void AttachAbilityTo(GameObject objectToattachTo)
        {
            AbilityBehaviour behaviourComponent = GetBehaviourComponent(objectToattachTo);
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }

        public virtual bool IsInRange(GameObject target) => behaviour.IsInRange(target);

        public virtual bool CanUseWhenInRange(GameObject target) => behaviour.CanUseWhenInRange(target);

        public void Use(GameObject target) => behaviour.Use(target);

        public float GetEnergyCost() => energyCost;

        public GameObject GetParticlePrefab() => particlePrefab;

        public AnimationClip GetAbilityAnimation()
        {
            abilityAnimation.events = new AnimationEvent[0]; // TODO consdier centralising with RemoveAnimationEvents()
            return abilityAnimation;
        }

        public AudioClip GetRandomAbilitySound()
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }
    }
}