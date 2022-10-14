using System.Collections;
using System.Reflection;

using UnityModManagerNet;
using HarmonyLib;
using UnityEngine;
using Behavior;
using Visor;
using System;
using System.Collections.Generic;

namespace RestNodeUMM
{
#if DEBUG
    [EnableReloading]
#endif
    public static class Main
    {
        static Harmony harmony;
        static AgentId restId;

        public static bool Load(UnityModManager.ModEntry entry)
        {
            harmony = new Harmony(entry.Info.Id);
            entry.OnToggle = OnToggle;

            restId = new AgentId();
            restId.type = AgentGestalt.Types.Logic;
            restId.guid = 1000;
            restId.displayName = "Rest";
            restId.agentGestaltId = (AgentGestaltEnum)1000;
            AgentGestalt gestalt = (AgentGestalt)ScriptableObject.CreateInstance(typeof(AgentGestalt));
            gestalt.properties = new Dictionary<int, AgentGestalt.Property>();
            gestalt.ports = new Dictionary<int, AgentGestalt.Port>();
            gestalt.ports[0] = new AgentGestalt.Port();
            gestalt.ports[0].position = 5;
            gestalt.ports[0].name = "URL";
            gestalt.ports[0].onlyAppliesData = false;
            gestalt.ports[0].description = "The url to post your REST request";

            gestalt.properties[0] = new AgentGestalt.Property();

            gestalt.agent = typeof(RestAgent);
            

            Holder.agentGestalts[(AgentGestaltEnum)1000] = gestalt;

            return true;

        }


        static bool OnToggle(UnityModManager.ModEntry entry, bool active)
        {
            if (active)
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
            }
            else
            {
                harmony.UnpatchAll(entry.Info.Id);
            }

            return true;
        }

#if DEBUG
        static bool OnUnload(UnityModManager.ModEntry entry) {
            return true;
        }
#endif

        public class RestAgent : Agent
        {
            
            protected override void OnSetupFinished()
            {
                this._v1 = this._runtimeProperties[0];
            }

            [SketchNodePortOperation(1)]
            public void Execute(SketchNode node)
            {
                UnityModManager.Logger.Log("Executed Node!");
            }

            private AgentProperty _v1;
            

        }


        [HarmonyPatch(typeof(WorldController), "Update")]

        class Patch
        {
            public static void Postfix()
            {
                if (Input.GetKeyDown(KeyCode.L))
                {

                    SketchViewNode node = Controllers.worldController.visor.sketchView.AddNode(restId, false);
                }
            }
        }
    }
}
