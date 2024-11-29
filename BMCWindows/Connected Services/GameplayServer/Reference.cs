﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMCWindows.GameplayServer {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResponse", Namespace="http://schemas.datacontract.org/2004/07/Service.Results")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(string[]))]
    public partial class OperationResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool IsSuccessField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public object Data {
            get {
                return this.DataField;
            }
            set {
                if ((object.ReferenceEquals(this.DataField, value) != true)) {
                    this.DataField = value;
                    this.RaisePropertyChanged("Data");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ErrorKey {
            get {
                return this.ErrorKeyField;
            }
            set {
                if ((object.ReferenceEquals(this.ErrorKeyField, value) != true)) {
                    this.ErrorKeyField = value;
                    this.RaisePropertyChanged("ErrorKey");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool IsSuccess {
            get {
                return this.IsSuccessField;
            }
            set {
                if ((this.IsSuccessField.Equals(value) != true)) {
                    this.IsSuccessField = value;
                    this.RaisePropertyChanged("IsSuccess");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="GameplayServer.IGameService", CallbackContract=typeof(BMCWindows.GameplayServer.IGameServiceCallback))]
    public interface IGameService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/InitializeGame", ReplyAction="http://tempuri.org/IGameService/InitializeGameResponse")]
        BMCWindows.GameplayServer.OperationResponse InitializeGame(string lobbyId, string[] players);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/InitializeGame", ReplyAction="http://tempuri.org/IGameService/InitializeGameResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> InitializeGameAsync(string lobbyId, string[] players);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/MarkPlayerReady", ReplyAction="http://tempuri.org/IGameService/MarkPlayerReadyResponse")]
        BMCWindows.GameplayServer.OperationResponse MarkPlayerReady(string lobbyId, string player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/MarkPlayerReady", ReplyAction="http://tempuri.org/IGameService/MarkPlayerReadyResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> MarkPlayerReadyAsync(string lobbyId, string player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/StartGame", ReplyAction="http://tempuri.org/IGameService/StartGameResponse")]
        BMCWindows.GameplayServer.OperationResponse StartGame(string lobbyId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/StartGame", ReplyAction="http://tempuri.org/IGameService/StartGameResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> StartGameAsync(string lobbyId);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnGameStarted")]
        void OnGameStarted();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnPlayerReady")]
        void OnPlayerReady(string player);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceChannel : BMCWindows.GameplayServer.IGameService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class GameServiceClient : System.ServiceModel.DuplexClientBase<BMCWindows.GameplayServer.IGameService>, BMCWindows.GameplayServer.IGameService {
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public GameServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public BMCWindows.GameplayServer.OperationResponse InitializeGame(string lobbyId, string[] players) {
            return base.Channel.InitializeGame(lobbyId, players);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> InitializeGameAsync(string lobbyId, string[] players) {
            return base.Channel.InitializeGameAsync(lobbyId, players);
        }
        
        public BMCWindows.GameplayServer.OperationResponse MarkPlayerReady(string lobbyId, string player) {
            return base.Channel.MarkPlayerReady(lobbyId, player);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> MarkPlayerReadyAsync(string lobbyId, string player) {
            return base.Channel.MarkPlayerReadyAsync(lobbyId, player);
        }
        
        public BMCWindows.GameplayServer.OperationResponse StartGame(string lobbyId) {
            return base.Channel.StartGame(lobbyId);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> StartGameAsync(string lobbyId) {
            return base.Channel.StartGameAsync(lobbyId);
        }
    }
}
