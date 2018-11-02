using PlayerIOClient;
using System.Collections.Generic;
using System.Threading;
namespace Rabbit.Auth
{
    /// <summary>
    /// Class Simple.
    /// </summary>
    public static class Simple
    {
        /// <summary>
        /// Store Client.
        /// </summary>
        /// <param name="client">The client which will be used.</param>
        /// <returns>Client.</returns> 
        public static Client client_;
        /// <summary>
        /// Waiting for linked accounts
        /// </summary>
        /// <param name="s1">The semaphore for waiting on linked accounts.</param>
        /// <returns>Client.</returns> 
        public static Semaphore s1 = new Semaphore(0, 1);
        /// <summary>
        /// Authenticates with the specified email and password.
        /// </summary>
        /// <param name="gameId">The game id.</param>
        /// <param name="email">The email.</param>
        /// <param name="password">The password.</param>
        /// <returns>Client.</returns> 
        public static Client Authenticate(string gameId, string email, string password, string[] playerInsightSegments = null)
        {

            PlayerIO.QuickConnect.SimpleConnect(gameId, email, password, playerInsightSegments, (Client client) =>
            {

                if (!client.BigDB.LoadMyPlayerObject().Contains("linkedTo"))
                {
                    client_ = client;
                    s1.Release();
                }
                else
                {
                    client.Multiplayer.CreateJoinRoom("$service-room", "AuthRoom", true, null, new Dictionary<string, string>() { { "type", "Link" } }, (Connection con) =>
                    {
                        con.OnMessage += (object sender1, PlayerIOClient.Message m) =>
                        {
                            if (m.Type == "auth")
                            {
                                client_ = PlayerIO.Authenticate("everybody-edits-su9rn58o40itdbnw69plyw", "linked", new Dictionary<string, string>() { { "userId", m.GetString(0) }, { "auth", m.GetString(1) } }, null);
                                s1.Release();
                            }
                        };
                    });
                }
            });
            s1.WaitOne();
            return client_;
        }

    }
}
