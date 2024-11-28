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
        private bool isGuest;


        private UserSessionManager()
        {
            isGuest = false;
        }

        public static UserSessionManager getInstance()
        {
            if (instance == null)
            {
                instance = new UserSessionManager();
            }
            return instance;
        }

        public void LoginPlayer(Server.PlayerDTO player, bool isGuestUser = false)
        {
            this.playerUser = player;
            this.isGuest = isGuestUser;
        }



        public void LogoutPlayer()
        {
            this.playerUser = null;
            this.isGuest = false;
        }



        public bool IsPlayerLogIn()
        {
            return playerUser != null;
        }

       
        public Server.PlayerDTO GetPlayerUserData()
        {
            return playerUser;
        }

        public bool IsGuestUser()
        {
            return isGuest;
        }


    }
}
