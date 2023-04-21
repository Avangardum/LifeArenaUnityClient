using Avangardum.LifeArena.Shared;
using Avangardum.LifeArena.UnityClient.Interfaces;
using Avangardum.LifeArena.UnityClient.IO;
using Avangardum.LifeArena.UnityClient.Presenters;
using Avangardum.LifeArena.UnityClient.ServerCommunication;
using Avangardum.LifeArena.UnityClient.Views;
using Zenject;

namespace Avangardum.LifeArena.UnityClient.Installers
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<InputManager>().AsSingle();

            Container.Bind<IFieldView>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IWindowManager>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IHeader>().FromComponentInHierarchy().AsSingle();
            Container.Bind<IGameView>().To<GameView>().AsSingle();

            Container.Bind<ILivingCellsArrayPreserializer>().To<LivingCellsArrayPreserializer>().AsSingle();
            Container.Bind<IServerFacade>().To<ServerFacade>().AsSingle();
            
            Container.Bind<FieldInputPresenter>().AsSingle().NonLazy();
            Container.Bind<GamePresenter>().AsSingle().NonLazy();
        }
    }
}