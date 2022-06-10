﻿using Abloom.Hashers;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abloom.ConnWPass
{
    internal class PasswordChecker
    {
        private readonly CustomPasswordHasher hasher = new();

        public bool StartCheck(List<string> passwords, string hash)
        {
            foreach (var password in passwords)
            {
                if (VerificatePassword(password, hash))
                    return true;
                
            }
            return false;
        }

        private bool VerificatePassword(string password, string hash)
        {
            if (hasher.VerifyHashedPassword(hash, password) == PasswordVerificationResult.Success)
            {
                return true;
            }
            return false;
        }
    }
}