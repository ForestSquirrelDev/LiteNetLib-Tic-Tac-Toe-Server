using System;
using System.Collections.Generic;
using Server.Connection;
using Server.Game;
using Server.Input;
using Server.UpdateLoop;
using PoorMansECS.Systems;
using ServerShared.Shared.Network;
using Server.Shared.Network;

var incomingPacketsPipe = new IncomingPacketsPipe();
var outgoingPacketsPipe = new OutgoingPacketsPipe(incomingPacketsPipe);
var connectionManager = new ConnectionManager(incomingPacketsPipe);
var inputCommandsPipe = new ConsoleInputCommandsPipe();
var gameModel = new GameModel(incomingPacketsPipe, outgoingPacketsPipe, inputCommandsPipe);
var serverLoop = new ServerLoop(new List<IUpdateable> { inputCommandsPipe, connectionManager, gameModel });

inputCommandsPipe.AddReceiver(serverLoop);

gameModel.Start();
connectionManager.Start();
serverLoop.RunMainLoop();

connectionManager.Dispose();
Console.WriteLine("Ahh yamete");