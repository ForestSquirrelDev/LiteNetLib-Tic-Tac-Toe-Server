using System.Collections.Generic;
using Server.Connection;
using Server.Game;
using Server.UpdateLoop;
using PoorMansECS.Systems;
using Server.ConsoleInput;
using ServerShared.Shared.Network;
using Server.Shared.Network;

var incomingPacketsPipe = new IncomingMessagesPipe();
var outgoingPacketsPipe = new OutgoingMessagesPipe(incomingPacketsPipe);
var connectionManager = new ConnectionManager(incomingPacketsPipe, outgoingPacketsPipe);
var inputCommandsPipe = new ConsoleInputCommandsPipe();
var gameModel = new GameModel(incomingPacketsPipe, outgoingPacketsPipe, inputCommandsPipe);
var serverLoop = new ServerLoop(new List<IUpdateable> { inputCommandsPipe, connectionManager, gameModel });

inputCommandsPipe.AddReceiver(serverLoop);

gameModel.Start();
connectionManager.Start();
serverLoop.RunMainLoop();

connectionManager.Dispose();