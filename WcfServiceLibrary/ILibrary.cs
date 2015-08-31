using System.ServiceModel;
using ClassLibrary.UserLibrary;

namespace WcfServiceLibrary
{
    [ServiceContract]
    public interface ILibrary
    {
        [OperationContract]
        void AddUser(User user);
        [OperationContract]
        bool IsAlive();
    }
}