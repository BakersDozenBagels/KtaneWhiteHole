using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
using HarmonyLib;
using System.Reflection;
using System.Linq;

public class WhiteHolePatcher : MonoBehaviour {
    private Harmony harmony;

    private void Start()
    {
#if UNITY_EDITOR
        StartCoroutine(Patch(KMGameInfo.State.Gameplay));
#else
        FindObjectOfType<KMGameInfo>().OnStateChange += x => StartCoroutine(Patch(x));
#endif
    }

    private IEnumerator Patch(KMGameInfo.State state)
    {
        yield return new WaitForSecondsRealtime(1f);
        Debug.Log(Resources.FindObjectsOfTypeAll<KMBombModule>().Select(m => m.ModuleDisplayName).Join(", "));
        List<KMBombModule> blackHoles = Resources.FindObjectsOfTypeAll<KMBombModule>().Where(m => m.ModuleDisplayName == "Black Hole").ToList();
        if (blackHoles.Count == 0) yield break;
        foreach (KMBombModule mod in blackHoles)
        {
            try
            {
                Debug.Log("[White Hole Patcher] Start patch...");
                if (harmony == null)
                    harmony = new Harmony("WhiteHolePatcher.Patch");

                Debug.Log(mod.gameObject.GetComponents<MonoBehaviour>().Where(c => c.GetType().Name == "BlackHoleModule").Count());
                Debug.Log(mod.gameObject.GetComponents<MonoBehaviour>().Select(c => c.GetType().Name).Join(", "));
                Debug.Log(mod.gameObject.GetComponents<MonoBehaviour>().Where(c => c.GetType().Name == "BlackHoleModule").First());
                Type modType = mod.gameObject.GetComponents<MonoBehaviour>().Where(c => c.GetType().Name == "BlackHoleModule").First().GetType();
                Debug.Log(modType);
                MethodInfo swirlInfo = modType.GetMethod("DisappearSwirl", BindingFlags.Instance | BindingFlags.NonPublic);
                MethodInfo thisInfo = GetType().GetMethod("Prefix", BindingFlags.Static | BindingFlags.NonPublic);
                harmony.Patch(swirlInfo, new HarmonyMethod(thisInfo));
                Debug.Log("[White Hole Patcher] Patch successful!");
            }
            catch (Exception e)
            {
                Debug.LogWarning(e.ToString());
            }
        }
    }

    public static event Action<int, GameObject> OnSwirlDisappear = new Action<int, GameObject>((x, y) => { });

    private static void Prefix(int ix)
    {
        Debug.Log("YAAAAAY");
        OnSwirlDisappear(0, new GameObject());
    }
}
