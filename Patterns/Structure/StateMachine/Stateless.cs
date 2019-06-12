using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stateless;

namespace Patterns.Structure.StateMachine
{
    class Stateless
    {
        public static bool ParentsNotWatching { get; private set; }

        public static void Test()
        {
            var stateMachine = new StateMachine<Health, Activity>(Health.NonReproductive);
            stateMachine.Configure(Health.NonReproductive)
              .Permit(Activity.ReachPuberty, Health.Reproductive);
            stateMachine.Configure(Health.Reproductive)
              .Permit(Activity.Historectomy, Health.NonReproductive)
              .PermitIf(Activity.HaveUnprotectedSex, Health.Pregnant,
                () => ParentsNotWatching);
            stateMachine.Configure(Health.Pregnant)
              .Permit(Activity.GiveBirth, Health.Reproductive)
              .Permit(Activity.HaveAbortion, Health.Reproductive);
        }

        public enum Health
        {
            NonReproductive,
            Pregnant,
            Reproductive
        }

        public enum Activity
        {
            GiveBirth,
            ReachPuberty,
            HaveAbortion,
            HaveUnprotectedSex,
            Historectomy,
        }


    }


}
