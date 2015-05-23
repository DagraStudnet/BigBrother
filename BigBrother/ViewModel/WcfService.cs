using System;
using System.ServiceModel;

namespace ClientBigBrother.ViewModel
{
    public static class WcfService<T> where T : ICommunicationObject, new()
    {
        public static void AutoRepair(ref T communicationObject)
        {
            AutoRepair(ref communicationObject, ()=>new T());
        }

        private static void AutoRepair(ref T communicationObject, Func<T> createMethod)
        {
            if ((communicationObject != null) && (communicationObject.State == CommunicationState.Faulted))
            {
                communicationObject.Abort();
                communicationObject = default(T);
            }
            if (communicationObject == null)
            {
                communicationObject = createMethod();
            }
        }
    }
}
