using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TJ.Decaf.Manager
{

    public class DeviceConfigManager
    {
        public void SetTargetFrame(int frame)
            => Application.targetFrameRate = frame;
    }

}
