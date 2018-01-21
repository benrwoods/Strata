using System;

namespace Strata.Utils {
    public static class EncryptionUtil {

        //TODO implement encryption

        public static string Encrypt(string toEncrypt, string password) {
            if (string.IsNullOrEmpty(toEncrypt)) throw new ArgumentException(nameof(toEncrypt));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException(nameof(password));

            return toEncrypt;
        }

        public static string Decrypt(string toDecrypt, string password) {
            if (string.IsNullOrEmpty(toDecrypt)) throw new ArgumentException(nameof(toDecrypt));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException(nameof(password));

            return toDecrypt;
        }
    }
}
