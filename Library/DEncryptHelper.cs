using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
namespace Library
{
    /// <summary>
    /// 对二进制的加密和解密
    /// </summary>
    public class DEncryptHelper
    {
        public static string keyStr = "4343fesgfqd42218";
        public DEncryptHelper() { }
        #region 加密方法 二进制加密
        /// <summary>
        /// 二进制加密
        /// </summary>
        /// <param name="keyStr">密钥</param>
        public static byte[]  EncryptBytes(byte[]b )
        {
            //通过des加密
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //通过流打开文件
           
           
            //获得加密字符串二进制字符
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            //计算指定字节组指定区域哈希值
            SHA1 ha = new SHA1Managed();
            byte[] hb = ha.ComputeHash(keyByteArray);
            //加密密钥数组
            byte[] sKey = new byte[8];
            //加密变量
            byte[] sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            //获取加密密钥
            des.Key = sKey;
            //设置加密初始化向量
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(b, 0, b.Length);
            cs.FlushFinalBlock();
            cs.Close();
            byte[]bb=ms.ToArray();
            ms.Close();
            return bb;    
        }
        #endregion
        #region 解密方法 
        /// <summary>
        /// 二进制解密
        /// </summary>
        /// <param name="keyStr">密钥</param>
        public static byte[] DecryptBytes(byte[]b)
        {
            //通过des解密
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            //通过流读取文件
           
            //密钥数组
            byte[] keyByteArray = Encoding.Default.GetBytes(keyStr);
            //定义哈希变量
            SHA1 ha = new SHA1Managed();
            //计算指定字节组指定区域哈希值
            byte[] hb = ha.ComputeHash(keyByteArray);
            //加密密钥数组
            byte[] sKey = new byte[8];
            //加密变量
            byte[] sIV = new byte[8];
            for (int i = 0; i < 8; i++)
                sKey[i] = hb[i];
            for (int i = 8; i < 16; i++)
                sIV[i - 8] = hb[i];
            //获取加密密钥
            des.Key = sKey;
            //加密变量
            des.IV = sIV;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(b, 0, b.Length);
            cs.FlushFinalBlock();
            byte[]br=ms.ToArray();
            cs.Close();
            ms.Close();
            return br;
        }
        #endregion
    }
}