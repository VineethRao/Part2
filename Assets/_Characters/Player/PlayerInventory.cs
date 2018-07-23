﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters
{
    public class PlayerInventory : MonoBehaviour
    {
        int coin;
        List<GameObject> inventory = new List<GameObject>();
        [SerializeField] bool hasDeliveryItem = true; // TODO go from mock to real

        public void AddCoin(int amount)
        {
            coin += amount;
        }

        public int GetCoinAmount()
        {
            return coin;
        }

        public void AddToInventory(GameObject gameObject)
        {
            inventory.Add(gameObject);
        }

        public bool IsPlayerCarrying()  // TODO pass paramater
        {
            return hasDeliveryItem;
        }
    }
}