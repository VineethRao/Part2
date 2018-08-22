﻿namespace RPG.Core.Saving
{
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using UnityEngine;
    using System.Collections.Generic;
    using System;
    using LevelState = System.Collections.Generic.Dictionary<string, object>;

    public class SaveLoad : MonoBehaviour
    {

        [SerializeField]
        [Tooltip("In seconds")]
        float AutoSaveInterval = 60;

        float TimeSinceLastSave = 0;

        void Start()
        {
            Load(GetLastSaveFile());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load(GetLastSaveFile());
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                Clear();
            }

            HandleAutoSave();
        }

        public void Save(bool isAuto = false)
        {
            var levelState = GetLevelState();
            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(GetSavePath(isAuto), FileMode.Create))
            {
                formatter.Serialize(stream, levelState);
            }
        }

        public bool Load(string saveFile)
        {
            var savePath = GetPathFromSaveFile(saveFile);
            if (!File.Exists(savePath))
            {
                return false;
            }

            var formatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(savePath, FileMode.Open))
            {
                var levelState = (LevelState)formatter.Deserialize(stream);
                UpdateLevelFromState(levelState);
            }
            return true;
        }

        public string[] GetSaveFileList()
        {
            var filePaths = Directory.GetFiles(Application.persistentDataPath);
            var fileNames = new string[filePaths.Length];
            for (int i = 0; i < filePaths.Length; ++i)
            {
                fileNames[i] = Path.GetFileNameWithoutExtension(filePaths[i]);
            }
            return fileNames;
        }

        public string GetLastSaveFile()
        {
            var saveFiles = GetSaveFileList();
            string lastSaveFile = null;
            DateTime lastSaveFileWriteTime = DateTime.MinValue;
            foreach (var saveFile in saveFiles)
            {
                var writeTime = File.GetLastWriteTime(GetPathFromSaveFile(saveFile));
                if (writeTime > lastSaveFileWriteTime)
                {
                    lastSaveFileWriteTime = writeTime;
                    lastSaveFile = saveFile;
                }
            }
            return lastSaveFile;
        }

        public void Clear()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
            }
        }

        private void HandleAutoSave()
        {
            TimeSinceLastSave += Time.deltaTime;
            if (TimeSinceLastSave > AutoSaveInterval)
            {
                TimeSinceLastSave = 0;
                Save(isAuto: true);
            }
        }

        LevelState GetLevelState()
        {
            var saveables = Resources.FindObjectsOfTypeAll(typeof(SaveableEntity));
            var levelState = new LevelState();
            foreach (SaveableEntity saveable in saveables)
            {
                if (levelState.ContainsKey(saveable.UniqueIdentifier))
                {
                    Debug.LogErrorFormat("Cannot have Saveables with the same name. This id duplicates another: {0}", saveable);

                    continue;
                }

                levelState[saveable.UniqueIdentifier] = saveable.CaptureState();
            }
            return levelState;
        }

        void UpdateLevelFromState(LevelState levelState)
        {
            foreach (KeyValuePair<string, object> entry in levelState)
            {
            }
            var saveables = Resources.FindObjectsOfTypeAll(typeof(SaveableEntity));
            foreach (SaveableEntity saveable in saveables)
            {
                if (levelState.ContainsKey(saveable.UniqueIdentifier))
                {
                    var saveableState = levelState[saveable.UniqueIdentifier];
                    saveable.RestoreState(saveableState);
                }
            }
        }

        string GetSavePath(bool isAuto = true)
        {
            var prefix = "Manual Save";
            if (isAuto)
            {
                prefix = "Auto Save";
            }
            var dateString = DateTime.Now.ToString("yyyy-MM-dd HH.mm.ss");
            return GetPathFromSaveFile(String.Format("{0} {1}", prefix, dateString));
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, String.Format("{0}.sav", saveFile));
        }
    }
}