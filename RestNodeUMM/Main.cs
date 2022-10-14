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
        static AgentGestalt gestalt;
        static bool loadedNodeResources = false;

        public static bool Load(UnityModManager.ModEntry entry)
        {
            Debug.Log("RestNodeUMM loaded");
            PlasmaModding.CustomNodeManager.CreateNode(GetGestalt(), "rest-api-post-request");
            return true;

        }

        public static AgentGestalt GetGestalt()
        {
            if (gestalt == null)
            {
                gestalt = (AgentGestalt)ScriptableObject.CreateInstance(typeof(AgentGestalt));
                gestalt.advanced = true; // Make true if you want the node to say advanced when hovered over in node library, you should enable this if the node can be hard to use
                gestalt.agent = typeof(RestAgent); // Very Important, the agent you set this to must be a class you define that can handle how the node acts
                gestalt.componentCategory = AgentGestalt.ComponentCategories.Behavior; // As far as I know for now this should always be behavior for nodes
                gestalt.description = "Creates a Rest POST Request"; //Description of the overall node behavior
                gestalt.displayName = "Rest"; // should be same as AgentId.displayName
                gestalt.id = (AgentGestaltEnum)1000; // should match AgentId.agentGestaltId
                gestalt.nodeAlwaysRun = false; //I think this can make it act like the ticker
                gestalt.nodeCategory = AgentCategoryEnum.Misc; // Category in node library
                gestalt.processesModuleInterfaces = false; // This should only be on if you want to have it do something involving modules
                gestalt.type = AgentGestalt.Types.Logic; // every node should have this

                gestalt.properties = new Dictionary<int, AgentGestalt.Property>();

                gestalt.ports = new Dictionary<int, AgentGestalt.Port>();
                gestalt.ports[0] = new AgentGestalt.Port();
                gestalt.ports[0].type = AgentGestalt.Port.Types.Command;
                gestalt.ports[0].position = 1;
                gestalt.ports[0].name = "URL";
                gestalt.ports[0].onlyAppliesData = false;
                gestalt.ports[0].description = "The url to post your REST request";


                gestalt.properties[0] = new AgentGestalt.Property();
            }
            return gestalt;
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
    }
}