﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace VeeamTestTask
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            if (args.Length != 2)
            {
                throw new ArgumentException();
            }
            else
            {
                string pathToFile, pathToFolder;
                pathToFile = args[0];
                pathToFolder = args[1];
                if (File.Exists(pathToFile)&&Directory.Exists(pathToFolder))
                {
                    List<FileModel> files = new List<FileModel>();
                    if (!pathToFolder.EndsWith(@"\")) pathToFolder = pathToFolder + @"\";
                    foreach(var elem in File.ReadAllLines(pathToFile))
                    {
                        string[] file = elem.Split(' ');
                        files.Add(new FileModel(pathToFolder+file[0],file[1],file[2]));
                    }
                    foreach (var file in files)
                    {
                        Console.Write(file.Path.Substring(file.Path.LastIndexOf(@"\")+1));
                        var result = CheckFile(file);
                        switch (result)
                        {
                            case Result.NOT_FOUND:
                                Console.WriteLine("\tNOT FOUND");
                                break;
                            default:
                                Console.WriteLine("\t"+result);
                                break;
                        }
                    }
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }
        /// <summary>
        /// Compare hash between file and source data
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Result CheckFile(FileModel file)
        {
            if (!File.Exists(file.Path)) return Result.NOT_FOUND;
            
            switch (file.Method)
            {
                case CryptoMethod.MD5:
                    if (file.HashSum == GetMD5Hash(file.Path)) return Result.OK;
                    else return Result.FAIL;
                    
                case CryptoMethod.SHA1:
                    
                    if (file.HashSum == GetSHA1Hash(file.Path)) return Result.OK;
                    else return Result.FAIL;

                case CryptoMethod.SHA256:
                    if (file.HashSum == GetSHA256Hash(file.Path)) return Result.OK;
                    else return Result.FAIL;
                default:
                    return Result.FAIL;
            }
           
        }
        /// <summary>
        /// Gets a hash of the file using SHA256.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA256Hash(string filePath)
        {
            using (var sha256 = new SHA256CryptoServiceProvider())
                return GetHash(filePath, sha256);
        }
        /// <summary>
        /// Gets a hash of the file using SHA1.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetSHA1Hash(string filePath)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
                return GetHash(filePath, sha1);
        }
        /// <summary>
        /// Gets a hash of the file using MD5.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetMD5Hash(string filePath)
        {
            using (var md5 = new MD5CryptoServiceProvider())
                return GetHash(filePath, md5);
        }
        /// <summary>
        /// Represent a file as FileStream
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        private static string GetHash(string filePath, HashAlgorithm hasher)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                return GetHash(fs, hasher);
        }
        /// <summary>
        /// Calculate hash from FileStream
        /// </summary>
        /// <param name="s"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        private static string GetHash(Stream s, HashAlgorithm hasher)
        {
            var hash = hasher.ComputeHash(s);
            return BitConverter.ToString(hash).Replace("-", String.Empty).ToLower();
        }


    }
    public enum Result
    {
        OK,
        FAIL,
        NOT_FOUND
    }
    public enum CryptoMethod
    {
        MD5,
        SHA1,
        SHA256
    }
    public class FileModel
    {
        public FileModel(string path, string method, string hashSum)
        {
            Path = path;
            
            HashSum = hashSum.ToLower();

            switch (method)
            {
                case "md5":
                    Method = CryptoMethod.MD5;
                    break;
                case "sha1":
                    Method = CryptoMethod.SHA1;
                    break;
                case "sha256":
                    Method = CryptoMethod.SHA256;
                    break;
                default:
                    throw new ArgumentException();
                    
            }
        }

        public string Path { get; set; }
        public CryptoMethod Method { get; set; }
        public string HashSum { get; set; }

        public override string ToString()
        {
            return $"{Path}  {Method}  {HashSum}";
        }

    }
}
