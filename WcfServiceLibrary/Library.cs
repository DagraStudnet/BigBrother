using System.ServiceModel;
using ClassLibrary;
using UserStorageLibrary;


namespace WcfServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Single)]
    public class Library : ILibrary
    {

        public void AddUser(User user)
        {
            var userNDatabase = UserNDatabase.ReturnDatabaseInstance();
            userNDatabase.AddUserToDBStorage(user);
        }
    }
}