using Prism.Unity;
using System.Windows;
using Prism.Ioc;
using AudioTAGEditor.Views;
using AudioTAGEditor.Services;
using IdSharp.Tagging.ID3v1;

namespace AudioTAGEditor
{
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
            => Container.Resolve<MainWindow>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IID3Service, ID3V1Service>(nameof(ID3V1Service));
            containerRegistry.Register<IID3Service, ID3V2Service>(nameof(ID3V2Service));
            containerRegistry.Register<IGenreService, GenreService>();
        }
    }
}
