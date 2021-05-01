using System.Windows;
using Autofac;
using lab2._1.Modules;
using lab2._1.View;
using lab2._1.VM;

namespace lab2._1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        // override переопределяем виртуальный метод
        protected override void OnStartup(StartupEventArgs e)
        {
            // ContainerBuiler позволяет произвести необходимую привязку Producer-Consumer
            var builder = new ContainerBuilder();
            builder.RegisterModule<DialogServiceModule>();
            // осуществляем необходимое связывание и получаем контейнер,
            // с помощью которого можем получать нужные нам данные;
            var container = builder.Build();

            var viewmodel = container.Resolve<CipherViewModel>();
            var view = new Window1 { DataContext = viewmodel };
            view.Show();
        }
    }
}
