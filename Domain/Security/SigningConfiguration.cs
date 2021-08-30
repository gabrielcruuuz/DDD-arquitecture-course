﻿using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Api.Domain.Security
{
    public class SigningConfiguration
    {
        public SecurityKey Key { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfiguration()
        {
            using (var provider = new RSACryptoServiceProvider(2048) )
            {
                this.Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            this.SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
