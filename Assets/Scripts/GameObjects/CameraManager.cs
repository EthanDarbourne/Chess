using Assets.Scripts.Misc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameObjects
{
    public class CameraManager : MonoBehaviour
    {
        private List<Camera> _cameras = new();
        private int _cameraIndex = 0;

        public Camera EnabledCamera => _cameras[_cameraIndex];

        public void Start()
        {
            _cameras = GetComponentsInChildren<Camera>().ToList();
            if(_cameras.Count == 0)
            {
                CustomLogger.LogError("No cameras registered");
                return;
            }
            _cameras.ForEach(camera => camera.enabled = false);
            _cameras[0].enabled = true;
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                ChangeToNextCamera();
            }
        }

        public void ChangeToNextCamera() 
        {
            if (_cameras.Count == 0) return;
            _cameras[_cameraIndex].enabled = false;
            _cameraIndex = (_cameraIndex + 1) % _cameras.Count;
            _cameras[_cameraIndex].enabled = true;
        }
    }
}
