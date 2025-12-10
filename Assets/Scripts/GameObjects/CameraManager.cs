using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.GameObjects
{
    public class CameraManager
    {

        private List<Camera> _cameras = new();
        private int _cameraIndex = 0;

        public Camera EnabledCamera => _cameras[_cameraIndex];

        public void RegisterCamera(Camera camera)
        {
            _cameras.Add(camera);
            camera.enabled = false;
        }

        public void ChangeToNextCamera() 
        {
            if (_cameras.Count == 0) return;
            _cameras[_cameraIndex].enabled = false;
            _cameraIndex = (_cameraIndex + 1) % _cameras.Count;
            _cameras[_cameraIndex].enabled = true;
        }

        public void EnableCamera(Camera camera)
        {
            _cameras[_cameraIndex].enabled = false;

            _cameraIndex = _cameras.IndexOf(camera);

            if(_cameraIndex == -1)
            {
                throw new Exception("Camera not registered with CameraManager");
            }
            _cameras[_cameraIndex].enabled = true;
        }
    }
}
