using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TJ.Decaf.Infra
{
    public static class ModuleConstants
    {
        public class ModuleData
        {
            public string RootModuleResourceName { get; }
            public List<string> SubModuleResourceNames { get; }
            public int Order { get; }
            
            public ModuleData(string rootModuleResource, List<string> subModuleResources, int order)
            {
                this.RootModuleResourceName = rootModuleResource;
                this.SubModuleResourceNames = subModuleResources;
                this.Order = order;
            }
        }

        public static class SplashScene
        {
            public static readonly List<ModuleData> ModuleResourceNames = new List<ModuleData>()
            {
                new ModuleData("Modules/Module - Splash", null, 0),
            };
        }

        public static class OnBoardingScene
        {
            public static readonly List<ModuleData> ModuleResourceNames = new List<ModuleData>()
            {
                new ModuleData("Modules/Module - OnBoarding", null, 0),
            };
        }

        public static class MainScene
        {
            public static readonly List<ModuleData> ModuleResourceNames = new List<ModuleData>()
            {
                new ModuleData("Modules/Module - Main", null, 0),
                new ModuleData("Modules/Module - BottomTab", null, 1),
            };
        }
    }

}

