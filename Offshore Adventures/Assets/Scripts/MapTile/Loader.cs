using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

namespace MapTile
{
    public class Loader : MonoBehaviour
    {
        public Data sceneMap;

        private void Start()
        {
            foreach (var boundary in GetComponentsInChildren<Boundary>())
            {
                boundary.OnEnterShip = UpdateNextMap;
                if (boundary.location == Location.Center)
                    boundary.OnLeaveShip = RemoveScene;
            }
        }

        private void UpdateNextMap(Location location)
        {
            switch (location)
            {
                case Location.North:
                    LoadScene(sceneMap.NorthIndex);
                    break;
                case Location.NorthEast:
                    LoadScene(sceneMap.NorthEastIndex);
                    break;
                case Location.NorthWest:
                    LoadScene(sceneMap.NorthWestIndex);
                    break;
                case Location.South:
                    LoadScene(sceneMap.SouthIndex);
                    break;
                case Location.SouthEast:
                    LoadScene(sceneMap.SouthEastIndex);
                    break;
                case Location.SouthWest:
                    LoadScene(sceneMap.SouthWestIndex);
                    break;
                case Location.East:
                    LoadScene(sceneMap.EastIndex);
                    break;
                case Location.West:
                    LoadScene(sceneMap.WestIndex);
                    break;
            }
        }

        private void LoadScene(int sceneIndex)
        {
            if (ValidSceneIndex(sceneIndex))
                if (!SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
                    SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
        }

        private static bool ValidSceneIndex(int sceneIndex)
        {
            return sceneIndex > 0 && sceneIndex < SceneManager.sceneCountInBuildSettings;
        }

        private void UnloadScene(int sceneIndex)
        {
            if (ValidSceneIndex(sceneIndex))
                if (SceneManager.GetSceneByBuildIndex(sceneIndex).isLoaded)
                    SceneManager.UnloadSceneAsync(sceneIndex);
        }

        public void RemoveScene()
        {
            UnloadScene(sceneMap.OwnerIndex);
        }
    }

    [Serializable]
    public struct Data
    {
        public int OwnerIndex;
        public int NorthIndex;
        public int NorthWestIndex;
        public int NorthEastIndex;
        public int SouthIndex;
        public int SouthWestIndex;
        public int SouthEastIndex;
        public int WestIndex;
        public int EastIndex;

    }

    public enum Location
    {
        Center, North, South, East, West, NorthEast, NorthWest, SouthEast, SouthWest,
    }
}