using Microsoft.Extensions.DependencyInjection;
using SoupBot;
using SoupBot.Forms;
using SoupBot.Mapping;
using SoupBot.Models;

ApplicationConfiguration.Initialize();
IServiceProvider provider = DependencyInjection.CreateServiceProvider();
Map map = provider.GetRequiredService<Map>();
Player player = provider.GetRequiredService<Player>();
player.Location = new(map.Size.Width / 2, 9);
map.Player = player;
Form mainForm = provider.GetRequiredService<MapForm>();
Application.Run(mainForm);
