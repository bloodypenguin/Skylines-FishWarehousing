using System;
using System.Linq;
using ColossalFramework.Plugins;
using ICities;

namespace FishWarehousing
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public static int MaxVehicleCount;
        
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (IsModActive(1764208250))
            {
                UnityEngine.Debug.LogWarning("More Vehicles is enabled, applying compatibility workaround");
                MaxVehicleCount = ushort.MaxValue + 1;
            }
            else
            {
                UnityEngine.Debug.Log("More Vehicles is not enabled");
                MaxVehicleCount = VehicleManager.MAX_VEHICLE_COUNT;
            }
        }
        
        private static bool IsModActive(ulong modId)
        {
            try
            {
                var plugins = PluginManager.instance.GetPluginsInfo();
                return plugins.Any(p => p != null && p.isEnabled && p.publishedFileID.AsUInt64 == modId);
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Failed to detect if mod {modId} is active");
                UnityEngine.Debug.LogException(e);
                return false;
            }
        }
    }
}