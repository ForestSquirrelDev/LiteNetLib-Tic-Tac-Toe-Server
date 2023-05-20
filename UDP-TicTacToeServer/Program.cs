using Server.Connection;
using Server.Game;
using Server.Input;
using Server.Shared.Network;
using Server.UpdateLoop;
using PoorMansECS.Systems;

var packetsPipe = new PacketsPipe();
var connectionManager = new ConnectionManager(packetsPipe);
var inputCommandsPipe = new ConsoleInputCommandsPipe();
var gameModel = new GameModel(packetsPipe, inputCommandsPipe);
var serverLoop = new ServerLoop(new List<IUpdateable> { inputCommandsPipe, connectionManager, gameModel });

inputCommandsPipe.AddReceiver(serverLoop);

gameModel.Start();
connectionManager.Start();
serverLoop.RunMainLoop();

connectionManager.Dispose();
Console.WriteLine("Ahh yamete");