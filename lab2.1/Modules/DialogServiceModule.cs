using Autofac;
using lab2._1.VM;

namespace lab2._1.Modules
{
    class DialogServiceModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // ContainerBuiler позволяет произвести необходимую привязку Producer-Consumer

            // С помощью метода этого класса  RegisterType мы делаем привязку интерфейса  с реализацией.
            // Сервисом они называют интерфейс, который должен быть реализован компонентом-провайдером
            // и использован компонентом-потребителем
            builder.RegisterType<DefaultDialogService>().As<IDialogService>(); 
            builder.RegisterType<TextFileService>().As<IFileService>();
            
            // Для того, чтобы зарегистрировать компонент  и указать, что он предоставляет себе, 
            // в качестве сервиса используется метод AsSelf(). 

            builder.RegisterType<CipherViewModel>().AsSelf();
        }
    }
}
