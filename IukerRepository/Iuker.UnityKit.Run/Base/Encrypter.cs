﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;


/***********************************************************************************************
Author：liuruoyu1981
CreateDate: 2017/02/14 14:59:20
Email: 35490136@qq.com
QQCode: 35490136
CreateNote: 
***********************************************************************************************/


/****************************************修改日志***********************************************
1. 修改日期： 修改人： 修改内容：
2. 修改日期： 修改人： 修改内容：
3. 修改日期： 修改人： 修改内容：
4. 修改日期： 修改人： 修改内容：
5. 修改日期： 修改人： 修改内容：
****************************************修改日志***********************************************/

namespace Iuker.UnityKit.Run
{
    /// <summary>
    /// 加密解密器
    /// </summary>
    public class Encrypter
    {
        private static string encryptKey = "8423";    //定义密钥  
        private Encrypter() { }

        public static readonly Encrypter Instance = new Encrypter();

        public string Encrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象   
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            byte[] data = Encoding.Unicode.GetBytes(str);//定义字节数组，用来存储要加密的字符串  
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      

            //使用内存流实例化加密流对象   
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateEncryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);  //向加密流中写入数据      
            CStream.FlushFinalBlock();              //释放加密流      
            return Convert.ToBase64String(MStream.ToArray());//返回加密后的字符串  
        }

        public string Decrypt(string str)
        {
            DESCryptoServiceProvider descsp = new DESCryptoServiceProvider();   //实例化加/解密类对象    
            byte[] key = Encoding.Unicode.GetBytes(encryptKey); //定义字节数组，用来存储密钥    
            byte[] data = Convert.FromBase64String(str.Replace(' ', '+'));//定义字节数组，用来存储要解密的字符串  
            MemoryStream MStream = new MemoryStream(); //实例化内存流对象      
            //使用内存流实例化解密流对象       
            CryptoStream CStream = new CryptoStream(MStream, descsp.CreateDecryptor(key, key), CryptoStreamMode.Write);
            CStream.Write(data, 0, data.Length);      //向解密流中写入数据     
            CStream.FlushFinalBlock();               //释放解密流      
            return Encoding.Unicode.GetString(MStream.ToArray());       //返回解密后的字符串  
        }
    }
}
