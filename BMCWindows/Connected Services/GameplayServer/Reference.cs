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
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BMCWindows.GameplayServer.AttackPositionDTO))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BMCWindows.GameplayServer.CellDeadDTO))]
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="AttackPositionDTO", Namespace="http://schemas.datacontract.org/2004/07/Service.DTO")]
    [System.SerializableAttribute()]
    public partial class AttackPositionDTO : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int XField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int YField;
        
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
        public int X {
            get {
                return this.XField;
            }
            set {
                if ((this.XField.Equals(value) != true)) {
                    this.XField = value;
                    this.RaisePropertyChanged("X");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Y {
            get {
                return this.YField;
            }
            set {
                if ((this.YField.Equals(value) != true)) {
                    this.YField = value;
                    this.RaisePropertyChanged("Y");
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
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CellDeadDTO", Namespace="http://schemas.datacontract.org/2004/07/Service.DTO")]
    [System.SerializableAttribute()]
    public partial class CellDeadDTO : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CardNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LobbyIdField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LooserField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int XField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int YField;
        
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
        public string CardName {
            get {
                return this.CardNameField;
            }
            set {
                if ((object.ReferenceEquals(this.CardNameField, value) != true)) {
                    this.CardNameField = value;
                    this.RaisePropertyChanged("CardName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LobbyId {
            get {
                return this.LobbyIdField;
            }
            set {
                if ((object.ReferenceEquals(this.LobbyIdField, value) != true)) {
                    this.LobbyIdField = value;
                    this.RaisePropertyChanged("LobbyId");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Looser {
            get {
                return this.LooserField;
            }
            set {
                if ((object.ReferenceEquals(this.LooserField, value) != true)) {
                    this.LooserField = value;
                    this.RaisePropertyChanged("Looser");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int X {
            get {
                return this.XField;
            }
            set {
                if ((this.XField.Equals(value) != true)) {
                    this.XField = value;
                    this.RaisePropertyChanged("X");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int Y {
            get {
                return this.YField;
            }
            set {
                if ((this.YField.Equals(value) != true)) {
                    this.YField = value;
                    this.RaisePropertyChanged("Y");
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
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/Attack", ReplyAction="http://tempuri.org/IGameService/AttackResponse")]
        BMCWindows.GameplayServer.OperationResponse Attack(string lobbyId, string attacker, BMCWindows.GameplayServer.AttackPositionDTO attackPosition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/Attack", ReplyAction="http://tempuri.org/IGameService/AttackResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> AttackAsync(string lobbyId, string attacker, BMCWindows.GameplayServer.AttackPositionDTO attackPosition);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/NotifyGameOver", ReplyAction="http://tempuri.org/IGameService/NotifyGameOverResponse")]
        BMCWindows.GameplayServer.OperationResponse NotifyGameOver(string lobbyId, string loser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/NotifyGameOver", ReplyAction="http://tempuri.org/IGameService/NotifyGameOverResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> NotifyGameOverAsync(string lobbyId, string loser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/NotifyCellDead", ReplyAction="http://tempuri.org/IGameService/NotifyCellDeadResponse")]
        BMCWindows.GameplayServer.OperationResponse NotifyCellDead(BMCWindows.GameplayServer.CellDeadDTO cellDeadDTO);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IGameService/NotifyCellDead", ReplyAction="http://tempuri.org/IGameService/NotifyCellDeadResponse")]
        System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> NotifyCellDeadAsync(BMCWindows.GameplayServer.CellDeadDTO cellDeadDTO);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IGameServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnGameStarted")]
        void OnGameStarted();
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnPlayerReady")]
        void OnPlayerReady(string player);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnAttackReceived")]
        void OnAttackReceived(BMCWindows.GameplayServer.AttackPositionDTO attackPosition);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnTurnChanged")]
        void OnTurnChanged(bool isPlayerTurn);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnGameOver")]
        void OnGameOver(string loser);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/IGameService/OnCellDead")]
        void OnCellDead(BMCWindows.GameplayServer.CellDeadDTO cellDeadDTO);
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
        
        public BMCWindows.GameplayServer.OperationResponse Attack(string lobbyId, string attacker, BMCWindows.GameplayServer.AttackPositionDTO attackPosition) {
            return base.Channel.Attack(lobbyId, attacker, attackPosition);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> AttackAsync(string lobbyId, string attacker, BMCWindows.GameplayServer.AttackPositionDTO attackPosition) {
            return base.Channel.AttackAsync(lobbyId, attacker, attackPosition);
        }
        
        public BMCWindows.GameplayServer.OperationResponse NotifyGameOver(string lobbyId, string loser) {
            return base.Channel.NotifyGameOver(lobbyId, loser);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> NotifyGameOverAsync(string lobbyId, string loser) {
            return base.Channel.NotifyGameOverAsync(lobbyId, loser);
        }
        
        public BMCWindows.GameplayServer.OperationResponse NotifyCellDead(BMCWindows.GameplayServer.CellDeadDTO cellDeadDTO) {
            return base.Channel.NotifyCellDead(cellDeadDTO);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.GameplayServer.OperationResponse> NotifyCellDeadAsync(BMCWindows.GameplayServer.CellDeadDTO cellDeadDTO) {
            return base.Channel.NotifyCellDeadAsync(cellDeadDTO);
        }
    }
}
