using System;
using LSPD_First_Response.Mod.API;
using Rage;
using DeputyRyansPursuitCallouts.Callouts; 

namespace DeputyRyansPursuitCallouts
{
    public class Main : Plugin
    {
        public override void Initialize()
        {
            Functions.OnOnDutyStateChanged += OnDutyStateChanged;
            Game.LogTrivial("DeputyRyansPursuitCallouts has been initialized.");
        }

        private void OnDutyStateChanged(bool onDuty)
        {
            if (onDuty)
            {
                Functions.RegisterCallout(typeof(PursuitInProgress));
                Functions.RegisterCallout(typeof(LargeVehiclePursuit));
                Functions.RegisterCallout(typeof(DrunkDriverPursuit));
                Functions.RegisterCallout(typeof(ArmedRobberyGetaway));
                Functions.RegisterCallout(typeof(StolenVehiclePursuit));
            }
        }

        public override void Finally()
        {
            Game.LogTrivial("DeputyRyansPursuitCallouts has been cleaned up.");
        }
    }
}
