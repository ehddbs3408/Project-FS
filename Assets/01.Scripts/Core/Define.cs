using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cinemachine;

namespace Core
{
    public static class Define
    {
        private static Camera _mainCam = null;
        public static Camera MainCam
        {
            get
            {
                if(_mainCam == null)
                {
                    _mainCam = Camera.main;
                }
                return _mainCam;
            }
        }
    }
}