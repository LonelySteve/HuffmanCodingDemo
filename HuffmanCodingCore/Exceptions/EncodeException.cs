using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
 

namespace HuffmanCodingCore.Exceptions
{
    /// <summary>
    /// 编码异常
    /// </summary>
    public class EncodeException:Exception
    {
        public EncodeException() : base() { }
        public EncodeException(string message) : base(message) { }
        public EncodeException(string message, Exception innerException) : base(message, innerException) { }
    }
    /// <summary>
    /// 编码参数异常
    /// </summary>
    public class EncodeArgumentException : EncodeException
    {
        public EncodeArgumentException():base() { }
        public EncodeArgumentException(string message) : base(message) { }
        public EncodeArgumentException(string message, Exception innerException) : base(message, innerException) { }
    }
    /// <summary>
    /// 因为编码字典而导致的编码异常
    /// </summary>
    public class CodeBookException : EncodeException
    {
        public CodeBookException() : base() { }
        public CodeBookException(string message) : base(message) { }
        public CodeBookException(string message,Exception innerException):base(message, innerException) { }
        public CodeBookException(string message,Dictionary<byte[],BitArray> codeBook) { }
    }
    /// <summary>
    /// 因为无法在编码字典中找到指定键而导致的编码异常
    /// </summary>
    public class CodeBookKeyNotFoundException : CodeBookException
    {
        public CodeBookKeyNotFoundException(byte[] key) : this(String.Join("", key.Select(b => b.ToString(" %x ")))) { }
       
        public CodeBookKeyNotFoundException(string keyName) : base($"Cannot find a key value pair with a key value of '{keyName}' in CodeBook!")
        {
        }
    }
}