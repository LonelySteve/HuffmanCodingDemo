using System.Collections.Generic;
using System.IO;

namespace HuffmanCodingCore
{
    public abstract class DataWrapper
    {
    }

    /// <summary>
    ///     流包装类
    /// </summary>
    public class StreamWrapper : DataWrapper
    {
        public StreamWrapper(Stream stream)
        {
            BaseStream = stream;
        }

        public Stream BaseStream { get; }
    }

    /// <summary>
    ///     文件流包装类
    /// </summary>
    public class FileStreamWrapper : DataWrapper
    {
        public FileStreamWrapper(Dictionary<string, FileStream> fileStreams)
        {
            FileStreams = fileStreams;
        }

        public Dictionary<string, FileStream> FileStreams { get; }
    }
}