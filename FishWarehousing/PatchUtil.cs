using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace FishWarehousing
{
    internal static class PatchUtil
    {
        private const string HarmonyId = "github.com/bloodypenguin/Skylines-FishWarehousing";
        private static Harmony _harmonyInstance = null;

        private static Harmony HarmonyInstance =>
            _harmonyInstance ??= new Harmony(HarmonyId);

        internal static void Patch(
            MethodDefinition original,
            MethodDefinition prefix = null,
            MethodDefinition postfix = null,
            MethodDefinition transpiler = null)
        {
            if (prefix == null && postfix == null && transpiler == null)
            {
                throw new Exception(
                    $"Fish Warehousing: prefix, postfix and transpiler are null for method {original.Type.FullName}.{original.MethodName}");
            }

            try
            {
                Debug.Log($"Fish Warehousing: Patching method {original.Type.FullName}.{original.MethodName}");
                var methodInfo = GetOriginal(original);
                HarmonyInstance.Patch(methodInfo,
                    prefix: prefix == null ? null : new HarmonyMethod(GetPatch(prefix)),
                    postfix: postfix == null ? null : new HarmonyMethod(GetPatch(postfix)),
                    transpiler == null ? null : new HarmonyMethod(GetPatch(transpiler))
                );
            }
            catch (Exception e)
            {
                Debug.LogError($"Fish Warehousing: Failed to patch method {original.Type.FullName}.{original.MethodName}");
                Debug.LogException(e);
            }
        }

        internal static void Unpatch(MethodDefinition original)
        {
            Debug.Log($"Fish Warehousing: Unpatching method {original.Type.FullName}.{original.MethodName}");
            HarmonyInstance.Unpatch(GetOriginal(original), HarmonyPatchType.All, HarmonyId);
        }

        private static MethodInfo GetOriginal(MethodDefinition original)
        {
            var bindingFlags = original.BindingFlags == BindingFlags.Default
                ? BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly
                : original.BindingFlags;
            var methodInfo = original.ArgumentTypes == null
                ? original.Type.GetMethod(original.MethodName, bindingFlags)
                : original.Type.GetMethod(original.MethodName, bindingFlags, null, original.ArgumentTypes, null);
            if (methodInfo == null)
            {
                throw new Exception(
                    $"Fish Warehousing: Failed to find original method {original.Type.FullName}.{original.MethodName}");
            }

            return methodInfo;
        }

        private static MethodInfo GetPatch(MethodDefinition patch)
        {
            var bindingFlags = patch.BindingFlags == BindingFlags.Default
                ? BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly
                : patch.BindingFlags;
            var methodInfo = patch.ArgumentTypes == null
                ? patch.Type.GetMethod(patch.MethodName, bindingFlags)
                : patch.Type.GetMethod(patch.MethodName, bindingFlags, null, patch.ArgumentTypes, null);
            
            if (methodInfo == null)
            {
                throw new Exception($"Fish Warehousing: Failed to find patch method {patch.Type.FullName}.{patch.MethodName}");
            }

            return methodInfo;
        }

        internal class MethodDefinition
        {
            public MethodDefinition(Type type, string methodName, 
                BindingFlags bindingFlags = BindingFlags.Default,
                Type[] argumentTypes = null)
            {
                Type = type;
                MethodName = methodName;
                BindingFlags = bindingFlags;
                ArgumentTypes = argumentTypes;
            }

            public Type Type { get; }
            public string MethodName { get; }

            public BindingFlags BindingFlags { get; }
            
            public Type[] ArgumentTypes { get; }
        }
    }
}