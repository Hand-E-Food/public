using Microsoft.Extensions.DependencyInjection;
using SoupBot;
using SoupBot.Forms;

ApplicationConfiguration.Initialize();
IServiceProvider provider = DependencyInjection.CreateServiceProvider();
Form mainForm = provider.GetRequiredService<MapForm>();
Application.Run(mainForm);
