using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ns
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameManager>(Lifetime.Singleton).As<ITest>();
            builder.Register<Show>(Lifetime.Scoped).As<ITest>();
            builder.RegisterEntryPoint<GamePlayer>();
        }
    }

}


