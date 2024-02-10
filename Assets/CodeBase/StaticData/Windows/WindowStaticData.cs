using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.StaticData.Windows {
    [CreateAssetMenu(fileName = "WindowStaticData", menuName = "StaticData/WindowStaticData")]
    public class WindowStaticData : ScriptableObject {
        public List<WindowConfig> Configs;
    }
}