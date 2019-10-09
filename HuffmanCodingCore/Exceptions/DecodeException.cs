using System;

namespace HuffmanCodingCore.Exceptions
{
    /// <summary>
    /// 解码异常
    /// </summary>
    public class DecodeException:Exception
    {
        
    }
    /// <summary>
    /// 文件格式异常
    /// </summary>
    public class FileFormatException : DecodeException
    {

    }
    /// <summary>
    /// 版本不匹配异常
    /// </summary>
    public class VersionMismatchException : DecodeException
    {

    }
    /// <summary>
    /// 版本不匹配异常
    /// </summary>
    public class CopyRightMismatchException : DecodeException
    {

    }

}