﻿using System.ServiceModel;
using ClassLibrary;


namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface ILibrary
    {
        [OperationContract]
        [ServiceKnownType(typeof(User))]
        void AddUser(User user);
    }
}