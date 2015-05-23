using System.ServiceModel;
using ClassLibrary;


namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    [ServiceKnownType("GetKnownTypes",typeof(KnownTypesProvider))] // zajistuje zdedene potomky
    public interface ILibrary
    {
        [OperationContract]
        void AddUser(User user);
    }
}