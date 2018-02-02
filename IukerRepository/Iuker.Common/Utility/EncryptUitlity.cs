﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Iuker.Common.Base;

namespace Iuker.Common.Utility
{
#if DEBUG
    /// <summary>
    ///加密解密工具
    /// </summary>
    [CreateDesc("liuruoyu1981", "liuruoyu1981@gmail.com", "20170902 17:40:54")]
    [ClassPurposeDesc("加密解密工具", "加密解密工具")]
#endif
    public static class EncryptUitlity
    {
        private static string encryptKey = "8423";    //定义密钥  

        public static string Encrypt(string str)
        {
            var descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            var key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            var data = Encoding.Unicode.GetBytes(str);//定义字节数组，用来存储要加密的字符串  
            var MStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化加密流对象   
            var CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据      
            CStream.FlushFinalBlock();              //释放加密流      
            return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
        }

        public static string Decrypt(string str)
        {
            var descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    
            var key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            var data = Convert.FromBase64String(str.Replace(' ', '+'));//定义字节数组，用来存储要解密的字符串  
            var MStream = new MemoryStream(); //实例化内存流对象      
            //使用内存流实例化解密流对象       
            var CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);      //向解密流中写入数据     
            CStream.FlushFinalBlock();               //释放解密流      
            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
        }
    }
}
