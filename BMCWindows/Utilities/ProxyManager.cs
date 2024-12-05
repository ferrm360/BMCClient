using BMCWindows.GameplayServer;
using System;
using System.ServiceModel;

namespace BMCWindows.Utilities
{
    public static class ProxyManager
    {
        public static void CloseProxy(IClientChannel proxy)
        {
            if (proxy != null)
            {
                try
                {
                    if (proxy.State == CommunicationState.Faulted)
                    {
                        proxy.Abort();
                    }
                    else
                    {
                        proxy.Close();
                    }
                    Console.WriteLine("Proxy cerrado exitosamente.");
                }
                catch (CommunicationException e)
                {
                    Console.WriteLine($"Error de comunicación al cerrar el proxy: {e.Message}");
                    proxy.Abort();
                }
                catch (TimeoutException e)
                {
                    Console.WriteLine($"Tiempo de espera agotado al cerrar el proxy: {e.Message}");
                    proxy.Abort();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error inesperado al cerrar el proxy: {e.Message}");
                    proxy.Abort();
                }
            }
        }

        public static void CloseGameServiceProxy(GameServiceClient proxy)
        {
            CloseProxy((IClientChannel)proxy);
        }
    }
}
