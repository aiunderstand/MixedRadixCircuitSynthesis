using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    namespace ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool isValidHeptCode(this String hepCode)
        {
            string alphabet = "0123456789ABCDEFGHKMNPRTVXZ";
            for (int i = 0; i < hepCode.Length; i++)
            {
                var symbol = hepCode[i];
                bool isValid = false;

                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (symbol.Equals(alphabet[j]))
                        isValid = true;
                }

                if (!isValid) //if no match has been found the symbol is not part of the alphabet and thus not a hepcode
                    return false;
            }

            return true;
        }
    }
}
