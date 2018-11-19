﻿using UnityEngine;
using System.Collections;
using RPG.CameraUI; // for mouse events

namespace RPG.Characters
{
    public class PlayerControl : MonoBehaviour
    {
        Character character;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;

        Coroutine attackingCoroutine;

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();
            
            RegisterForMouseEvents();
        }

        private void RegisterForMouseEvents()
        {
            var cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
        }

        void Update()
        {
             ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                StopRunningBehaviours();
                character.SetDestination(destination);
            }
        }

        private void StopRunningBehaviours()
        {
            weaponSystem.StopAttacking();
            if (attackingCoroutine != null)
            {
                StopCoroutine(attackingCoroutine);
            }
        }

        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            
            return distanceToTarget <= weaponSystem.GetMaxAttackRange();
        }

        void OnMouseOverEnemy(EnemyAI enemy)
        {
            if (Input.GetMouseButton(0))
            {
                StopRunningBehaviours();
                attackingCoroutine = StartCoroutine(MoveAndAttack(enemy));
            }
            else if (Input.GetMouseButtonDown(1))
            {
                StopRunningBehaviours();
                attackingCoroutine = StartCoroutine(MoveAndPowerAttack(enemy));
            }
        }

        IEnumerator MoveAndAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            weaponSystem.AttackTarget(enemy.gameObject);
        }

        IEnumerator MoveAndPowerAttack(EnemyAI enemy)
        {
            yield return StartCoroutine(MoveToTarget(enemy.gameObject));
            abilities.AttemptSpecialAbility(0, enemy.gameObject);
        }

        IEnumerator MoveToTarget(GameObject target)
        {
            character.SetDestination(target.transform.position);
            while (!IsTargetInRange(target))
            {
                yield return new WaitForEndOfFrame();
            }
            character.ClearDestination();
        }
    }
}