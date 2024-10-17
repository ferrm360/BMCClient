using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BMCWindows
{
    public class ChatService
    {
       
            private const string MessageFilePath = "chatMessages.txt";
            private List<Message> messageBuffer = new List<Message>();
            private const int BufferSize = 100; // Cantidad de mensajes a mantener en memoria

            public ChatService()
            {
                // Cargar los mensajes recientes en memoria al iniciar
                LoadRecentMessages();
            }

            // Método para agregar un mensaje al chat
            public void AddMessage(string sender, string messageContent)
            {
                Message message = new Message { Sender = sender, Messages = messageContent, TimeSent = DateTime.Now };

                // Guardar en el buffer
                messageBuffer.Add(message);

                // Escribir el mensaje en archivo
                SaveMessageToFile(message);

                // Si el buffer supera el tamaño límite, vaciarlo
                if (messageBuffer.Count > BufferSize)
                {
                    messageBuffer.RemoveAt(0); // Remover el mensaje más antiguo
                }
            }

            // Guardar mensaje en archivo
            private void SaveMessageToFile(Message message)
            {
                using (StreamWriter sw = new StreamWriter(MessageFilePath, true))
                {
                    sw.WriteLine($"{message.TimeSent},{message.Sender},{message.Messages}");
                }
            }

            // Recuperar mensajes recientes del archivo al buffer
            private void LoadRecentMessages()
            {
                if (File.Exists(MessageFilePath))
                {

                var lines = File.ReadAllLines(MessageFilePath);
                var lastLines = lines.Skip(Math.Max(0, lines.Length - BufferSize)).ToList();
                foreach (var line in lastLines)
                    {
                        var parts = line.Split(',');
                        var message = new Message
                        {
                            TimeSent = DateTime.Parse(parts[0]),
                            Sender = parts[1],
                            Messages = parts[2]
                        };
                        messageBuffer.Add(message);
                    }
                }
            }

            // Cargar mensajes antiguos bajo demanda
            public List<Message> LoadOlderMessages(int count)
            {
                List<Message> oldMessages = new List<Message>();
                if (File.Exists(MessageFilePath))
                {
                    var lines = File.ReadAllLines(MessageFilePath).Take(count).ToList(); // Cargar los primeros N mensajes
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',');
                        var message = new Message
                        {
                            TimeSent = DateTime.Parse(parts[0]),
                            Sender = parts[1],
                            Messages = parts[2]
                        };
                        oldMessages.Add(message);
                    }
                }
                return oldMessages;
            }

            public List<Message> GetRecentMessages()
            {
                return messageBuffer;
            }
        }


    public class Message
    {
        
        public string Sender { get; set; }
        
        public string Messages { get; set; }

        public DateTime TimeSent { get; set; }
    }


}
