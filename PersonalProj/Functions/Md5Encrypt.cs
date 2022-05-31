using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PersonalProj.Functions
{
    public class Md5Encrypt
    {
        private string PasswordKeyword = "SimplePasswordEncryption";
        private string UserNameKeyword = "SimpleUserNameEncryption";

        private string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }

        public string EncryptPassword(string Password)
        {
            int PassLength = Password.Length;
            int EncryptLength = PasswordKeyword.Length;
            string NewString = string.Empty;
            if (PassLength > EncryptLength)
            {
                for (int i = 0; i < EncryptLength; i++)
                {
                    NewString = NewString + (Password[i] + PasswordKeyword[i]);
                }
            }
            else
            {
                for (int i = 0; i < PassLength; i++)
                {
                    NewString = NewString + (Password[i] + PasswordKeyword[i]);
                }
            }
            return MD5Hash(NewString);
        }

        public string EncryptUserName(string UserName)
        {
            int PassLength = UserName.Length;
            int EncryptLength = UserNameKeyword.Length;
            string NewString = string.Empty;
            if (PassLength > EncryptLength)
            {
                for (int i = 0; i < EncryptLength; i++)
                {
                    NewString = NewString + (UserName[i] + UserNameKeyword[i]);
                }
            }
            else
            {
                for (int i = 0; i < PassLength; i++)
                {
                    NewString = NewString + (UserName[i] + UserNameKeyword[i]);
                }
            }
            return MD5Hash(NewString);
        }
    }
}