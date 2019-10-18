using Prism.Unity;
using System.Windows;
using Prism.Ioc;
using AudioTAGEditor.Views;
using AudioTAGEditor.Services;
using IdSharp.Tagging.ID3v1;
using AutoMapper;
using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;
using IdSharp.Tagging.ID3v2;

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
            containerRegistry.Register<IAudiofileConverter, AudiofileConverter>();
            containerRegistry.Register<IFileService, FileService>();
            containerRegistry.RegisterSingleton<IHistoryService, HistoryService>();
            containerRegistry.Register<IAudiofileComparerService, AudiofileComparerService>();
            containerRegistry.RegisterSingleton<ILogService, LogService>();
            
            var config = new MapperConfiguration(c =>
            {
                c.CreateMap<Audiofile, AudiofileViewModel>();
                c.CreateMap<Audiofile, AudiofileID3v1ViewModel>();
                c.CreateMap<AudiofileViewModel, Audiofile>();
                c.CreateMap<AudiofileID3v1ViewModel, Audiofile>();
                c.CreateMap<ID3v1Tag, Audiofile>();
                c.CreateMap<ID3v2Tag, Audiofile>();
                c.CreateMap<Audiofile, ID3v1Tag>();
                c.CreateMap<Audiofile, ID3v2Tag>();
            });

            var mapper = config.CreateMapper();
            containerRegistry.RegisterInstance(mapper);
        }
    }
}
