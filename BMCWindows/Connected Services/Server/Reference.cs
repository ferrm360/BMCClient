﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMCWindows.Server {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="PlayerDTO", Namespace="http://schemas.datacontract.org/2004/07/Service.DTO")]
    [System.SerializableAttribute()]
    public partial class PlayerDTO : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string PasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private int PlayerIDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
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
        public string Email {
            get {
                return this.EmailField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailField, value) != true)) {
                    this.EmailField = value;
                    this.RaisePropertyChanged("Email");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Password {
            get {
                return this.PasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.PasswordField, value) != true)) {
                    this.PasswordField = value;
                    this.RaisePropertyChanged("Password");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public int PlayerID {
            get {
                return this.PlayerIDField;
            }
            set {
                if ((this.PlayerIDField.Equals(value) != true)) {
                    this.PlayerIDField = value;
                    this.RaisePropertyChanged("PlayerID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Username {
            get {
                return this.UsernameField;
            }
            set {
                if ((object.ReferenceEquals(this.UsernameField, value) != true)) {
                    this.UsernameField = value;
                    this.RaisePropertyChanged("Username");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResultOfanyType", Namespace="http://schemas.datacontract.org/2004/07/Service.Results")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BMCWindows.Server.OperationResultOfPlayerDTOQo1Oyztf))]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BMCWindows.Server.PlayerDTO))]
    public partial class OperationResultOfanyType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private object DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
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
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResultOfPlayerDTOQo1Oyztf", Namespace="http://schemas.datacontract.org/2004/07/Service.Results")]
    [System.SerializableAttribute()]
    public partial class OperationResultOfPlayerDTOQo1Oyztf : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private BMCWindows.Server.PlayerDTO DataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string ErrorKeyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool SuccessField;
        
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
        public BMCWindows.Server.PlayerDTO Data {
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
        public bool Success {
            get {
                return this.SuccessField;
            }
            set {
                if ((this.SuccessField.Equals(value) != true)) {
                    this.SuccessField = value;
                    this.RaisePropertyChanged("Success");
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="Server.IAccountService")]
    public interface IAccountService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/Register", ReplyAction="http://tempuri.org/IAccountService/RegisterResponse")]
        BMCWindows.Server.OperationResultOfanyType Register(BMCWindows.Server.PlayerDTO player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/Register", ReplyAction="http://tempuri.org/IAccountService/RegisterResponse")]
        System.Threading.Tasks.Task<BMCWindows.Server.OperationResultOfanyType> RegisterAsync(BMCWindows.Server.PlayerDTO player);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/Login", ReplyAction="http://tempuri.org/IAccountService/LoginResponse")]
        BMCWindows.Server.OperationResultOfPlayerDTOQo1Oyztf Login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IAccountService/Login", ReplyAction="http://tempuri.org/IAccountService/LoginResponse")]
        System.Threading.Tasks.Task<BMCWindows.Server.OperationResultOfPlayerDTOQo1Oyztf> LoginAsync(string username, string password);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IAccountServiceChannel : BMCWindows.Server.IAccountService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class AccountServiceClient : System.ServiceModel.ClientBase<BMCWindows.Server.IAccountService>, BMCWindows.Server.IAccountService {
        
        public AccountServiceClient() {
        }
        
        public AccountServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public AccountServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public AccountServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BMCWindows.Server.OperationResultOfanyType Register(BMCWindows.Server.PlayerDTO player) {
            return base.Channel.Register(player);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.Server.OperationResultOfanyType> RegisterAsync(BMCWindows.Server.PlayerDTO player) {
            return base.Channel.RegisterAsync(player);
        }
        
        public BMCWindows.Server.OperationResultOfPlayerDTOQo1Oyztf Login(string username, string password) {
            return base.Channel.Login(username, password);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.Server.OperationResultOfPlayerDTOQo1Oyztf> LoginAsync(string username, string password) {
            return base.Channel.LoginAsync(username, password);
        }
    }
}
