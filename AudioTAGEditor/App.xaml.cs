using Prism.Unity;
using System.Windows;
using Prism.Ioc;
using AudioTAGEditor.Views;
using AudioTAGEditor.Services;
using IdSharp.Tagging.ID3v1;
using AutoMapper;
using AudioTAGEditor.Models;
using AudioTAGEditor.ViewModels;

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

            var config = new MapperConfiguration(c => c.CreateMap<AudioFile, AudioFileViewModel>());
            var mapper = config.CreateMapper();
            containerRegistry.RegisterInstance(mapper);
        }
    }

    class Ab
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class aViewModel
    {
        public aViewModel(string lastName)
        {
            LastName = lastName;
        }
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
