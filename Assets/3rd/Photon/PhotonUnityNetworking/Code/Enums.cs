// ----------------------------------------------------------------------------
// <copyright file="Enums.cs" company="Exit Games GmbH">
//   PhotonNetwork Framework for Unity - Copyright (C) 2018 Exit Games GmbH
// </copyright>
// <summary>
// Wraps up several enumerations for PUN.
// </summary>
// <author>developer@exitgames.com</author>
// ----------------------------------------------------------------------------


namespace Photon.Pun
{
    /// <summary>Which PhotonNetwork method was called to connect (which influences the regions we want pinged).</summary>
    /// <remarks>PhotonNetwork.ConnectUsingSettings will call either ConnectToMaster, ConnectToRegion or ConnectToBest, depending on the settings.</remarks>
    public enum ConnectMethod { NotCalled, ConnectToMaster, ConnectToRegion, ConnectToBest }


    /// <summary>Used to define the level of logging output created by the PUN classes. Either log errors, info (some more) or full.</summary>
    /// \ingroup publicApi
    public enum PunLogLevel
    {
        /// <summary>Show only errors. Minimal output. Note: Some might be "runtime errors" which you have to expect.</summary>
        ErrorsOnly,

        /// <summary>Logs some of the workflow, calls and results.</summary>
        Informational,

        /// <summary>Every available log call gets into the console/log. Only use for debugging.</summary>
        Full
    }


    /// <summary>Enum of "target" options for RPCs. These define which remote clients get your RPC call. </summary>
    /// \ingroup publicApi
    public enum RpcTarget
    {
        /// <summary>Sends the RPC to everyone else and executes it immediately on this client. Player who join later will not execute this RPC.</summary>
        All,

        /// <summary>Sends the RPC to everyone else. This client does not execute the RPC. Player who join later will not execute this RPC.</summary>
        Others,

        /// <summary>Sends the RPC to MasterClient only. Careful: The MasterClient might disconnect before it executes the RPC and that might cause dropped RPCs.</summary>
        MasterClient,

        /// <summary>Sends the RPC to everyone else and executes it immediately on this client. New players get the RPC when they join as it's buffered (until this client leaves).</summary>
        AllBuffered,

        /// <summary>Sends the RPC to everyone. This client does not execute the RPC. New players get the RPC when they join as it's buffered (until this client leaves).</summary>
        OthersBuffered,

        /// <summary>Sends the RPC to everyone (including this client) through the server.通过服务器向每个人(包括这个客户端)发送RPC</summary>
        /// <remarks>
        /// This client executes the RPC like any other when it received it from the server.当从服务器接收RPC时，这个客户机像其他客户机一样执行RPC
        /// Benefit: The server's order of sending the RPCs is the same on all clients.好处:在所有客户端上，服务器发送rpc的顺序都是一样的。
        /// </remarks>
        AllViaServer,

        /// <summary>Sends the RPC to everyone (including this client) through the server and buffers it for players joining later.</summary>
        /// <remarks>
        /// This client executes the RPC like any other when it received it from the server.
        /// Benefit: The server's order of sending the RPCs is the same on all clients.
        /// </remarks>
        AllBufferedViaServer
    }


    public enum ViewSynchronization { Off, ReliableDeltaCompressed, Unreliable, UnreliableOnChange }


    /// <summary>
    /// Options to define how Ownership Transfer is handled per PhotonView.
    /// </summary>
    /// <remarks>
    /// This setting affects how RequestOwnership and TransferOwnership work at runtime.
    /// </remarks>
    public enum OwnershipOption
    {
        /// <summary>
        /// Ownership is fixed. Instantiated objects stick with their creator, room objects always belong to the Master Client.
        /// </summary>
        Fixed,
        /// <summary>
        /// Ownership can be taken away from the current owner who can't object.
        /// </summary>
        Takeover,
        /// <summary>
        /// Ownership can be requested with PhotonView.RequestOwnership but the current owner has to agree to give up ownership.
        /// </summary>
        /// <remarks>The current owner has to implement IPunCallbacks.OnOwnershipRequest to react to the ownership request.</remarks>
        Request
    }
}