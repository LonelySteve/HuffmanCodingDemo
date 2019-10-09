using System.Collections.Generic;
using System.IO;

namespace HuffmanCodingCore
{
    public abstract class DataWrapper
    {
         
    }
    /// <summary>
    /// 流包装类
    /// </summary>
    public class StreamWrapper: DataWrapper
    {
        public Stream BaseStream { get; }

        public StreamWrapper(Stream stream) => BaseStream = stream;
    }
    /// <summary>
    /// 文件流包装类
    /// </summary>
    public class FileStreamWrapper : DataWrapper
    {
        public Dictionary<string,FileStream> FileStreams { get; }

        public FileStreamWrapper(Dictionary<string, FileStream> fileStreams) => FileStreams = fileStreams;
    }
}