﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BMCWindows.EmailServer {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="EmailDTO", Namespace="http://schemas.datacontract.org/2004/07/Service.DTO")]
    [System.SerializableAttribute()]
    public partial class EmailDTO : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string CustomBodyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string EmailTypeField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LobbyHostField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LobbyNameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string LobbyPasswordField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string RecipientField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string UsernameField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VerificationCodeField;
        
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
        public string CustomBody {
            get {
                return this.CustomBodyField;
            }
            set {
                if ((object.ReferenceEquals(this.CustomBodyField, value) != true)) {
                    this.CustomBodyField = value;
                    this.RaisePropertyChanged("CustomBody");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string EmailType {
            get {
                return this.EmailTypeField;
            }
            set {
                if ((object.ReferenceEquals(this.EmailTypeField, value) != true)) {
                    this.EmailTypeField = value;
                    this.RaisePropertyChanged("EmailType");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LobbyHost {
            get {
                return this.LobbyHostField;
            }
            set {
                if ((object.ReferenceEquals(this.LobbyHostField, value) != true)) {
                    this.LobbyHostField = value;
                    this.RaisePropertyChanged("LobbyHost");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LobbyName {
            get {
                return this.LobbyNameField;
            }
            set {
                if ((object.ReferenceEquals(this.LobbyNameField, value) != true)) {
                    this.LobbyNameField = value;
                    this.RaisePropertyChanged("LobbyName");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string LobbyPassword {
            get {
                return this.LobbyPasswordField;
            }
            set {
                if ((object.ReferenceEquals(this.LobbyPasswordField, value) != true)) {
                    this.LobbyPasswordField = value;
                    this.RaisePropertyChanged("LobbyPassword");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string Recipient {
            get {
                return this.RecipientField;
            }
            set {
                if ((object.ReferenceEquals(this.RecipientField, value) != true)) {
                    this.RecipientField = value;
                    this.RaisePropertyChanged("Recipient");
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VerificationCode {
            get {
                return this.VerificationCodeField;
            }
            set {
                if ((object.ReferenceEquals(this.VerificationCodeField, value) != true)) {
                    this.VerificationCodeField = value;
                    this.RaisePropertyChanged("VerificationCode");
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
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationResponse", Namespace="http://schemas.datacontract.org/2004/07/Service.Results")]
    [System.SerializableAttribute()]
    [System.Runtime.Serialization.KnownTypeAttribute(typeof(BMCWindows.EmailServer.EmailDTO))]
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
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="EmailServer.IEmailService")]
    public interface IEmailService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmailService/SendEmail", ReplyAction="http://tempuri.org/IEmailService/SendEmailResponse")]
        BMCWindows.EmailServer.OperationResponse SendEmail(BMCWindows.EmailServer.EmailDTO emailDTO);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IEmailService/SendEmail", ReplyAction="http://tempuri.org/IEmailService/SendEmailResponse")]
        System.Threading.Tasks.Task<BMCWindows.EmailServer.OperationResponse> SendEmailAsync(BMCWindows.EmailServer.EmailDTO emailDTO);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IEmailServiceChannel : BMCWindows.EmailServer.IEmailService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class EmailServiceClient : System.ServiceModel.ClientBase<BMCWindows.EmailServer.IEmailService>, BMCWindows.EmailServer.IEmailService {
        
        public EmailServiceClient() {
        }
        
        public EmailServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public EmailServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public BMCWindows.EmailServer.OperationResponse SendEmail(BMCWindows.EmailServer.EmailDTO emailDTO) {
            return base.Channel.SendEmail(emailDTO);
        }
        
        public System.Threading.Tasks.Task<BMCWindows.EmailServer.OperationResponse> SendEmailAsync(BMCWindows.EmailServer.EmailDTO emailDTO) {
            return base.Channel.SendEmailAsync(emailDTO);
        }
    }
}
