using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows.Patterns.Singleton
{
    internal class UserSessionManager
    {
        
        private static UserSessionManager instance;
        private Server.PlayerDTO playerUser;
        

        private UserSessionManager()
        {

        }

        public static UserSessionManager getInstance()
        {
            if (instance == null)
            {
                instance = new UserSessionManager();
            }
            return instance;
        }

        public void loginPlayer(Server.PlayerDTO player)
        {
            this.playerUser = player;
        }

        

        public void logoutPlayer()
        {
            this.playerUser = null;
        }

        

        public bool isPlayerLogIn()
        {
            return playerUser != null;
        }

       

        public Server.PlayerDTO getPlayerUserData()
        {
            return playerUser;
        }

     
    }
}
