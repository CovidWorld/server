﻿using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Sygic.Corona.Infrastructure.Services.AndroidAttestation
{
    public class OfflineAttestation : IAndroidAttestation
    {
        /// <summary>
        /// Parses and verifies a SafetyNet attestation statement.
        /// </summary>
        /// <param name="signedAttestationStatement">A string containing the
        /// JWT attestation statement.</param>
        /// <returns>A parsed attestation statement. null if the statement is
        /// invalid.</returns>
        public AttestationStatement ParseAndVerify(string signedAttestationStatement)
        {
            // First parse the token and get the embedded keys.
            JwtSecurityToken token;
            try
            {
                token = new JwtSecurityToken(signedAttestationStatement);
            }
            catch (ArgumentException)
            {
                // The token is not in a valid JWS format.
                return null;
            }

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKeys = GetEmbeddedKeys(token)
            };

            // Perform the validation
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(
                    signedAttestationStatement,
                    validationParameters,
                    out validatedToken);
            }
            catch (ArgumentException)
            {
                // Signature validation failed.
                return null;
            }

            // Verify the hostname
            if (!(validatedToken.SigningKey is X509SecurityKey))
            {
                // The signing key is invalid.
                return null;
            }
            if (!VerifyHostname("attest.android.com", validatedToken.SigningKey as X509SecurityKey))
            {
                // The certificate isn't issued for the hostname
                // attest.android.com.
                return null;
            }

            // Parse and use the data JSON.
            Dictionary<string, string> claimsDictionary = token.Claims
                .ToDictionary(x => x.Type, x => x.Value);

            return new AttestationStatement(claimsDictionary);
        }

        /// <summary>
        /// Verifes an X509Security key, and checks that it is issued for a
        /// given hostname.
        /// </summary>
        /// <param name="hostname">The hostname to check to.</param>
        /// <param name="securityKey">The security key to verify.</param>
        /// <returns>true if securityKey is valid and is issued to the given
        /// hostname.</returns>
        private static bool VerifyHostname(string hostname, X509SecurityKey securityKey)
        {
            string commonName;
            try
            {
                if (!securityKey.Certificate.Verify())
                {
                    return false;
                }

                commonName = securityKey.Certificate.GetNameInfo(X509NameType.DnsName, false);
            }
            catch (CryptographicException)
            {
                return false;
            }
            return (commonName == hostname);
        }

        /// <summary>
        /// Retrieves the X509 security keys embedded in a JwtSecurityToken.
        /// </summary>
        /// <param name="token">The token where the keys are to be retrieved
        /// from.</param>
        /// <returns>The embedded security keys. null if there are no keys in
        /// the security token.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the JWT data
        /// does not contain a valid signature
        /// header "x5c".</exception>
        /// <exception cref="CryptographicException">Thrwon when the JWT data
        /// does not contain valid signing
        /// keys.</exception>
        private static X509SecurityKey[] GetEmbeddedKeys(JwtSecurityToken token)
        {
            // The certificates are embedded in the "x5c" part of the header.
            // We extract them, parse them, and then create X509 keys out of
            // them.
            X509SecurityKey[] keys = null;
            keys = (token.Header["x5c"] as JArray)
                .Values<string>()
                .Select(x => new X509SecurityKey(
                    new X509Certificate2(Convert.FromBase64String(x))))
                .ToArray();
            return keys;
        }
    }
}
