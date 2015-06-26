using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ClassLibrary
{
    public static class GenericResolverInstaller
    {
        public static void AddGenericResolver(this ServiceHost host, params Type[] typesToResolver)
        {
            foreach (ServiceEndpoint endpoint in host.Description.Endpoints)
            {
                AddGenericResolver(endpoint, typesToResolver);
            }
        }

        public static void AddGenericResolver<T>(this ClientBase<T> proxy, params Type[] typesToResolver)
            where T : class
        {
            AddGenericResolver(proxy.Endpoint, typesToResolver);
        }

        public static void AddGenericResolver<T>(this ChannelFactory<T> channel, params Type[] typesToResolver)
            where T : class
        {
            AddGenericResolver(channel.Endpoint, typesToResolver);
        }

        private static void AddGenericResolver(ServiceEndpoint endpoint, Type[] typesToResolver)
        {
            foreach (OperationDescription operation in endpoint.Contract.Operations)
            {
                var behavior =
                    operation.Behaviors.Find<DataContractSerializerOperationBehavior>();

                GenericResolver newResolver;

                if (typesToResolver == null || typesToResolver.Any() == false)
                {
                    newResolver = new GenericResolver();
                }
                else
                {
                    newResolver = new GenericResolver(typesToResolver);
                }

                var oldResolver = behavior.DataContractResolver as GenericResolver;
                behavior.DataContractResolver = GenericResolver.Merge(oldResolver, newResolver);
            }
        }
    }
}