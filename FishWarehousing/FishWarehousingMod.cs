using CitiesHarmony.API;
using ICities;

namespace FishWarehousing
{
    public class FishWarehousingMod : IUserMod
    {
        public string Name => "Fish Warehousing";

        public string Description => "Allows to store fish in the Industries warehouses";
        
        public void OnEnabled() 
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled() 
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }
}