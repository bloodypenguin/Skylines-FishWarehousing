using CitiesHarmony.API;
using ICities;

namespace FishWarehousing
{
    public class Mod : IUserMod
    {
        public string Name => "Fish Warehousing";

        public string Description => "Allows to store fish in the Industries warehouses";
        
        public void OnEnabled() {
            HarmonyHelper.EnsureHarmonyInstalled();
        }
    }
}