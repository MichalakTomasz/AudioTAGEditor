using Prism.Unity;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using AudioTAGEditor.Views;
using AudioTAGEditor.Services;

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
        }
    }
}
