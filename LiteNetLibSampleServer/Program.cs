using System;
using System.Collections.Generic;
using LiteNetLibSampleServer.Connection;
using LiteNetLibSampleServer.Input;
using LiteNetLibSampleServer.UpdateLoop;
using PoorMansECS.Systems;

var packetsPipe = new PacketsPipe();
var connectionManager = new ConnectionManager(packetsPipe);
var inputCommandsPipe = new ConsoleInputCommandsPipe();
var serverLoop = new ServerLoop(new List<IUpdateable> { inputCommandsPipe, connectionManager });

inputCommandsPipe.AddReceiver(serverLoop);

connectionManager.Init();
serverLoop.Run();

connectionManager.Dispose();
Console.WriteLine("Ahh yamete");
