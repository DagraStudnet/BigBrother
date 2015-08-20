﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClientBigBrother.WcfServiceLibrary {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="WcfServiceLibrary.ILibrary")]
    public interface ILibrary {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILibrary/AddUser", ReplyAction="http://tempuri.org/ILibrary/AddUserResponse")]
        void AddUser(ClassLibrary.UserLibrary.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILibrary/AddUser", ReplyAction="http://tempuri.org/ILibrary/AddUserResponse")]
        System.Threading.Tasks.Task AddUserAsync(ClassLibrary.UserLibrary.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILibrary/IsAlive", ReplyAction="http://tempuri.org/ILibrary/IsAliveResponse")]
        bool IsAlive();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/ILibrary/IsAlive", ReplyAction="http://tempuri.org/ILibrary/IsAliveResponse")]
        System.Threading.Tasks.Task<bool> IsAliveAsync();
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ILibraryChannel : ClientBigBrother.WcfServiceLibrary.ILibrary, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class LibraryClient : System.ServiceModel.ClientBase<ClientBigBrother.WcfServiceLibrary.ILibrary>, ClientBigBrother.WcfServiceLibrary.ILibrary {
        
        public LibraryClient() {
        }
        
        public LibraryClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public LibraryClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LibraryClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public LibraryClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void AddUser(ClassLibrary.UserLibrary.User user) {
            base.Channel.AddUser(user);
        }
        
        public System.Threading.Tasks.Task AddUserAsync(ClassLibrary.UserLibrary.User user) {
            return base.Channel.AddUserAsync(user);
        }
        
        public bool IsAlive() {
            return base.Channel.IsAlive();
        }
        
        public System.Threading.Tasks.Task<bool> IsAliveAsync() {
            return base.Channel.IsAliveAsync();
        }
    }
}
