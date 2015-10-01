using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Resources;
using Microsoft.Azure.Management.Resources.Models;
using Microsoft.Rest.Azure;
using AzBits.Rest;

namespace ConsoleApp1
{
    public class Program
    {
        #region GUIDS
        //replace subscriptionId and tenantID with valid values
        const string subscriptionId = "12345678-90ab-cdef-1234-567890abcdef"; 
        const string tenantId = "12345678-90ab-cdef-1234-567890abcdef";
        //from ../New-AzureServicePrincipalOwner.ps1
        const string applicationId = "12345678-90ab-cdef-1234-567890abcdef";
        const string password = "ExamplePassword";

        #endregion

        public void Main(string[] args)
        {
            var credentials = new AzurePasswordTokenCredentials(tenantId, subscriptionId, applicationId, password);
            var subClient = new SubscriptionClient(credentials);
            var armClient = new ResourceManagementClient(credentials);

            //BUG: generated operations validate that the client.SubscriptionId != null
            subClient.SubscriptionId = subscriptionId;
            armClient.SubscriptionId = subscriptionId;

            TenantListResult tenantListResult = subClient.Tenants.List();
            IList<TenantIdDescription> tenants = tenantListResult.Value;
            Console.WriteLine("TENANTS");
            foreach(TenantIdDescription tenant in tenants)
            {
                Console.WriteLine($"Id = {tenant.Id}");
                Console.WriteLine($"TenantId = {tenant.TenantId}");
                Console.WriteLine("-----");
            }

            SubscriptionListResult subListResult = subClient.Subscriptions.List();
            IList<Subscription> subscriptions = subListResult.Value;
            Console.WriteLine("SUBSCRIPTIONS");
            foreach(Subscription sub in subscriptions)
            {
                Console.WriteLine($"Displayname = {sub.DisplayName}");
                Console.WriteLine($"Id = {sub.Id}");
                Console.WriteLine($"SubscriptionId = {sub.SubscriptionId}");
                Console.WriteLine($"State = {sub.State}");
                Console.WriteLine("-----");
            }

            List<Provider> providers = new List<Provider>();
            Page<Provider> page = armClient.Providers.List();
            foreach(Provider p in page)
            {
                providers.Add(p);
            }
            while(page.NextPageLink != null)
            {
                page = armClient.Providers.ListNext(page.NextPageLink);
                foreach(Provider p in page)
                {
                    providers.Add(p);
                }
            }

            Console.WriteLine("PROVIDERS");
            foreach(Provider p in providers)
            {
                if(p.Id.EndsWith("Microsoft.MobileEngagement"))
                Console.WriteLine($"Id = {p.Id}");
                {
                    Console.WriteLine($"RegistrationState = {p.RegistrationState}");
                    Console.WriteLine($"Namespace = {p.NamespaceProperty}");
                    IList<ProviderResourceType> resourceTypes = p.ResourceTypes;
                    foreach (ProviderResourceType prt in resourceTypes)
                    {
                        Console.WriteLine($"ResourceType = {prt.ResourceType}");
                        foreach (string v in prt.ApiVersions)
                        {
                            Console.WriteLine($"Version = {v}");
                        }
                        foreach (string x in prt.Locations)
                        {
                            Console.WriteLine($"Location = {x}");
                        }
                        IDictionary<string, string> properties = prt.Properties;
                        if(properties != null)
                        {
                            ICollection<string> keys = properties.Keys;
                            foreach (string k in keys)
                            {
                                Console.WriteLine($"{k} = {properties[k]}");
                            }
                        }
                    }
                }
                Console.WriteLine("-----");
            }

            Page<GenericResource> resourcesPage = armClient.Resources.List();
            foreach(GenericResource resource in resourcesPage)
            {
                Console.WriteLine($"name = {resource.Name}");
                Console.WriteLine($"id = {resource.Id}");
            }

            Console.WriteLine("<Enter> to continue");
            Console.ReadLine();

        }
    }
}
