﻿using UnityEngine;
using UnityEngine.SceneManagement;
using RPG.Saving;

namespace RPG.SceneManagement
{
    public class TransitionPoint : MonoBehaviour
    {
        public string newSceneName;

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                FindObjectOfType<SaveLoad>().Save();
                SceneManager.LoadScene(newSceneName);
            }
        }
    }
}
