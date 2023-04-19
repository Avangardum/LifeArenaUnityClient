using Avangardum.LifeArena.UnityClient.Interfaces;
using Avangardum.LifeArena.UnityClient.IO;
using Avangardum.LifeArena.UnityClient.Presenters;
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
            Container.Bind<FieldInputPresenter>().AsSingle().NonLazy();
        }
    }
}