using Avangardum.LifeArena.UnityClient.Interfaces;
using UnityEngine;

namespace Avangardum.LifeArena.UnityClient.Views
{
    public class WindowManager : MonoBehaviour, IWindowManager
    {
        [SerializeField] private GameObject _serverUnavailableWindow;
        [SerializeField] private GameObject _noInternetConnectionWindow;
        
        public bool IsServerUnavailableWindowVisible
        {
            set => _serverUnavailableWindow.SetActive(value);
        }
        
        public bool IsNoInternetConnectionWindowVisible 
        {
            set => _noInternetConnectionWindow.SetActive(value);
        }
    }
}