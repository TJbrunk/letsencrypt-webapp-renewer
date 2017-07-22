﻿using System;
using System.Collections.Generic;
using System.Linq;
using static System.FormattableString;

namespace OhadSoft.AzureLetsEncrypt.Renewal.Management
{
    public sealed class RenewalParameters
    {
        public Guid SubscriptionId { get; }
        public string TenantId { get; }
        public string ResourceGroup { get; }
        public string WebApp { get; }
        public IReadOnlyList<string> Hosts { get; }
        public string Email { get; }
        public Guid ClientId { get; }
        public string ClientSecret { get; }
        public bool UseIpBasedSsl { get; }
        public int RsaKeyLength { get; }
        public Uri AcmeBasedUri { get; }

        public RenewalParameters(
            Guid subscriptionId,
            string tenantId,
            string resourceGroup,
            string webApp,
            IReadOnlyList<string> hosts,
            string email,
            Guid clientId,
            string clientSecret,
            bool useIpBasedSsl = false,
            int rsaKeyLength = 2048,
            Uri acmeBasedUri = null)
        {
            SubscriptionId = subscriptionId != Guid.Empty
                ? subscriptionId
                : throw new ArgumentException("Subscription ID must not be an empty GUID", nameof(subscriptionId));

            TenantId = !String.IsNullOrWhiteSpace(tenantId)
                ? tenantId
                : throw new ArgumentException("Tenant ID must not be null or whitespace", nameof(tenantId));

            ResourceGroup = !String.IsNullOrWhiteSpace(resourceGroup)
                ? resourceGroup
                : throw new ArgumentException("Resource group name must not be null or whitespace", nameof(resourceGroup));

            WebApp = !String.IsNullOrWhiteSpace(webApp)
                ? webApp
                : throw new ArgumentException("Web app name must not be null or whitespace", nameof(webApp));

            Hosts = hosts != null && hosts.Count > 0 && !hosts.Any(String.IsNullOrWhiteSpace)
                ? hosts
                : throw new ArgumentException("Host collection must be non-null, contain at least one element, and contain no whitespace-only entries", nameof(hosts));

            Email = !String.IsNullOrWhiteSpace(email) && email.Contains("@") && email.Length >= 3 && email.Length <= 254
                    && !email.StartsWith("@", StringComparison.OrdinalIgnoreCase) && !email.EndsWith("@", StringComparison.OrdinalIgnoreCase)
                ? email
                : throw new ArgumentException("E-mail address must not be null and must be valid", nameof(email));

            ClientId = clientId != Guid.Empty ?
                clientId :
                throw new ArgumentException("Client ID must not be an empty GUID", nameof(clientId));

            ClientSecret = !String.IsNullOrWhiteSpace(clientSecret)
                ? clientSecret
                : throw new ArgumentException("Client secret must not be null or whitespace", nameof(clientSecret));

            UseIpBasedSsl = useIpBasedSsl;

            RsaKeyLength = rsaKeyLength > 0
                ? rsaKeyLength
                : throw new ArgumentException("RSA key length must be positive", nameof(rsaKeyLength));

            AcmeBasedUri = acmeBasedUri == null || acmeBasedUri.IsAbsoluteUri
                ? acmeBasedUri
                : throw new ArgumentException("ACME base URI must be either null or absolute", nameof(acmeBasedUri));
        }

        public override string ToString()
        {
            return Invariant($"{nameof(TenantId)}: {TenantId}, {nameof(SubscriptionId)}: {SubscriptionId}, {nameof(ClientId)}: {ClientId}, {nameof(ResourceGroup)}: {ResourceGroup}, {nameof(WebApp)}: {WebApp}, {nameof(Email)}: {Email}, {nameof(Hosts)}: {Hosts}, {nameof(UseIpBasedSsl)}: {UseIpBasedSsl}, {nameof(RsaKeyLength)}: {RsaKeyLength}, {nameof(AcmeBasedUri)}: {AcmeBasedUri}, {nameof(ClientSecret)}: [SCRUBBED]");
        }
    }
}