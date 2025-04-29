/*
 | | | | ___   ___  _ __  _   _ / ___| __ _ _ __ ___   ___ 
 | |_| |/ _ \ / _ \| '_ \| | | | |  _ / _` | '_ ` _ \ / _ \
 |  _  | (_) | (_) | |_) | |_| | |_| | (_| | | | | | |  __/
 |_| |_|\___/ \___/| .__/ \__, |\____|\__,_|_| |_| |_|\___|
                   |_|    |___/                            
┌──────────────────────────────────────────────┐
│　Copyright(C) 2025 by HoopyGameStudio
│　描   述*：算法扩展
│           洗牌算法（Knuth算法）
│           哈希（MD5,SHA-256）对称（AES）
│　创 建 人*：Hoopy
│　创建时间：2025-01-01 00:00:00
└──────────────────────────────────────────────┘
┌──────────────────────────────────────────────┐
│　修 改 人：
│　修改描述：
└──────────────────────────────────────────────┘
*/
using System.Collections.Generic;
using System.Text;
using System;
using System.Security.Cryptography;
using Random = UnityEngine.Random;

namespace HoopyGame
{
    public static class AlgorithmExtension
    {
        #region 洗牌算法
        //Knuth算法
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                // 生成一个 [0, i] 范围内的随机索引
                int j = Random.Range(0, i + 1);
                // 交换元素
                (array[i], array[j]) = (array[j], array[i]);
            }
        }
        // 针对 List<T> 的洗牌扩展方法
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
        #endregion
        #region 加密算法
        public static class EncryptionUtils
        {
            //Hash不可逆加密类型
            public enum HashEncryptType
            {
                MD5,
                SHA256
            }

            private const string _defaultKey = "dinghaopi1314521";//16位
            private const string _defaultIV = "0000000000000000";//16位

            #region HASH
            /// <summary>
            /// 哈希不可逆加密
            /// </summary>
            /// <param name="value">加密内容</param>
            /// <param name="hashEncryptType">加密类型</param>
            /// <returns></returns>
            public static string HashEncrypt(string value, HashEncryptType hashEncryptType)
            {
                if (string.IsNullOrEmpty(value)) return null;
                byte[] inputBytes = Encoding.UTF8.GetBytes(value);
                byte[] hashBytes;

                switch (hashEncryptType)
                {
                    case HashEncryptType.MD5:
                        using (MD5 md5 = MD5.Create())
                            hashBytes = md5.ComputeHash(inputBytes);
                        break;
                    case HashEncryptType.SHA256:
                        using (SHA256 sha256 = SHA256.Create())
                            hashBytes = sha256.ComputeHash(inputBytes);
                        break;
                    default:
                        hashBytes = null;
                        break;
                }
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
            /// <summary>
            /// 哈希不可逆加密
            /// </summary>
            /// <param name="value">加密内容</param>
            /// <param name="hashEncryptType">加密类型</param>
            /// <returns></returns>
            public static string HashEncrypt(byte[] value, HashEncryptType hashEncryptType)
            {
                if (value.Length <= 0) return null;
                byte[] hashBytes;

                switch (hashEncryptType)
                {
                    case HashEncryptType.MD5:
                        using (MD5 md5 = MD5.Create())
                            hashBytes = md5.ComputeHash(value);
                        break;
                    case HashEncryptType.SHA256:
                        using (SHA256 sha256 = SHA256.Create())
                            hashBytes = sha256.ComputeHash(value);
                        break;
                    default:
                        hashBytes = null;
                        break;
                }
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
            #endregion
            #region AES
            #region 加密
            /// <summary>
            /// 加密字符串
            /// </summary>
            /// <param name="value"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public static string EncryptStrByAES(string value, string key, string IV) =>
                Convert.ToBase64String(AesEncrypt(Encoding.UTF8.GetBytes(value), key.Length == 16 ? value : _defaultKey, IV));
            /// <summary>
            /// AES 算法加密(ECB模式 PKCS7填充) 将明文加密
            /// </summary>
            /// <param name="data">明文</param>
            /// <param name="key">密钥</param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns>加密后base64编码的密文</returns>
            public static byte[] AesEncrypt(byte[] data, string key, string IV, CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7) =>
                AesEncrypt(data, Encoding.UTF8.GetBytes(key), IV, cipherMode, paddingMode);
            /// <summary>
            /// AES 算法加密 将明文加密
            /// </summary>
            /// <param name="data">明文</param>
            /// <param name="key">密钥</param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns>加密后base64编码的密文</returns>
            public static byte[] AesEncrypt(byte[] data, byte[] key, string IV, CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7) =>
                AesEncrypt(data, key, IV, 0, data.Length, cipherMode, paddingMode);

            /// <summary>
            /// AES 算法加密 将明文加密
            /// </summary>
            /// <param name="data">明文</param>
            /// <param name="key">密钥</param>
            /// <param name="offset">明文数据偏移</param>
            /// <param name="length">明文数据长度</param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns>加密后base64编码的密文</returns>
            public static byte[] AesEncrypt(byte[] data, byte[] key, string IV, int offset, int length,
                CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7)
            {
                try
                {
                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = key;
                    rDel.Mode = cipherMode;
                    rDel.Padding = paddingMode;
                    if (!string.IsNullOrEmpty(IV))
                    {
                        IV = IV.Length >= 16 ? IV : _defaultIV;
                        byte[] tIV = Encoding.UTF8.GetBytes(IV);
                        rDel.IV = tIV;
                    }
                    return rDel.CreateEncryptor().TransformFinalBlock(data, offset, length);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            #endregion
            #region 解密
            /// <summary>
            /// 解密字符串
            /// </summary>
            /// <param name="value"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            public static string DecryptStrByAES(string value, string key, string IV) =>
                Encoding.UTF8.GetString(AesDecrypt(Convert.FromBase64String(value), key.Length == 16 ? value : _defaultKey, IV));
            /// <summary>
            /// AES 算法解密 将密文解码进行解密，返回明文
            /// </summary>
            /// <param name="data"></param>
            /// <param name="key"></param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns></returns>
            public static byte[] AesDecrypt(byte[] data, string key, string IV, CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7) =>
                AesDecrypt(data, Encoding.UTF8.GetBytes(key), IV, cipherMode, paddingMode);
            /// <summary>
            /// AES 算法解密 将密文解码进行解密，返回明文
            /// </summary>
            /// <param name="data">密文</param>
            /// <param name="key">密钥</param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns>明文</returns>
            public static byte[] AesDecrypt(byte[] data, byte[] key, string IV, CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7) =>
                AesDecrypt(data, key, IV, 0, data.Length, cipherMode, paddingMode);
            /// <summary>
            /// AES 算法解密 将密文解码进行解密，返回明文
            /// </summary>
            /// <param name="data">密文</param>
            /// <param name="key">密钥</param>
            /// <param name="offset">明文数据偏移</param>
            /// <param name="length">明文数据长度</param>
            /// <param name="cipherMode">加密模式</param>
            /// <param name="paddingMode">填充方式</param>
            /// <returns>明文</returns>
            public static byte[] AesDecrypt(byte[] data, byte[] key, string IV, int offset, int length,
                CipherMode cipherMode = CipherMode.ECB,
                PaddingMode paddingMode = PaddingMode.PKCS7)
            {

                try
                {
                    RijndaelManaged rDel = new RijndaelManaged();
                    rDel.Key = key;
                    rDel.Mode = cipherMode;
                    rDel.Padding = paddingMode;
                    if (!string.IsNullOrEmpty(IV))
                    {
                        IV = IV.Length >= 16 ? IV : _defaultIV;
                        byte[] tIV = Encoding.UTF8.GetBytes(IV);
                        rDel.IV = tIV;
                    }
                    return rDel.CreateDecryptor().TransformFinalBlock(data, offset, length);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            #endregion
            #endregion
        }
        #endregion
    }
}