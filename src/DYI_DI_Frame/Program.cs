using DiFrame;
using DYI_DI_Frame;


var services = new ServiceCollection();

services.AddSingleton<IConsoleWriter, ConsoleWriter>();
services.AddSingleton<IdGeneratorService>();
services.AddTransient<IIdGenerator, IdGeneratorService>();

var serviceProvider = services.BuildServiceProvider();

var writer = serviceProvider.GetRequiredService<IConsoleWriter>();
var singletonIdGen1 = serviceProvider.GetRequiredService<IdGeneratorService>();
var singletonIdGen2 = serviceProvider.GetRequiredService<IdGeneratorService>();
var transIdGen1 = serviceProvider.GetRequiredService<IIdGenerator>();
var transIdGen2 = serviceProvider.GetRequiredService<IIdGenerator>();


writer.Write("test singletonIdGen1: ");
singletonIdGen1.Print();
writer.Write("test singletonIdGen2: ");
singletonIdGen2.Print();
writer.Write("test transIdGen1: ");
transIdGen1.Print();
writer.Write("test transIdGen2: ");
transIdGen2.Print();


