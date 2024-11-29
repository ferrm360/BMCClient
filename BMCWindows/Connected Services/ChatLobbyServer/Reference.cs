﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMCWindows.ChatLobbyServer {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ChatLobbyServer.IChatLobbyService", CallbackContract=typeof(BMCWindows.ChatLobbyServer.IChatLobbyServiceCallback))]
    public interface IChatLobbyService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatLobbyService/RegisterUser", ReplyAction="http://tempuri.org/IChatLobbyService/RegisterUserResponse")]
        void RegisterUser(string username, string lobbyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IChatLobbyService/RegisterUser", ReplyAction="http://tempuri.org/IChatLobbyService/RegisterUserResponse")]
        System.Threading.Tasks.Task RegisterUserAsync(string username, string lobbyId);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatLobbyService/SendMessage")]
        void SendMessage(string lobbyId, string username, string message);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatLobbyService/SendMessage")]
        System.Threading.Tasks.Task SendMessageAsync(string lobbyId, string username, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatLobbyServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IChatLobbyService/ReceiveMessage")]
        void ReceiveMessage(string username, string message);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IChatLobbyServiceChannel : BMCWindows.ChatLobbyServer.IChatLobbyService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ChatLobbyServiceClient : System.ServiceModel.DuplexClientBase<BMCWindows.ChatLobbyServer.IChatLobbyService>, BMCWindows.ChatLobbyServer.IChatLobbyService {
        
        public ChatLobbyServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public ChatLobbyServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public ChatLobbyServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatLobbyServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public ChatLobbyServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RegisterUser(string username, string lobbyId) {
            base.Channel.RegisterUser(username, lobbyId);
        }
        
        public System.Threading.Tasks.Task RegisterUserAsync(string username, string lobbyId) {
            return base.Channel.RegisterUserAsync(username, lobbyId);
        }
        
        public void SendMessage(string lobbyId, string username, string message) {
            base.Channel.SendMessage(lobbyId, username, message);
        }
        
        public System.Threading.Tasks.Task SendMessageAsync(string lobbyId, string username, string message) {
            return base.Channel.SendMessageAsync(lobbyId, username, message);
        }
    }
}
