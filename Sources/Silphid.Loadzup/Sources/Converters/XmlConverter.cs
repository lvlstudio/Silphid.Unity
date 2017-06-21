﻿using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Silphid.Loadzup
{
    public class XmlConverter : IConverter
    {
        public bool Supports<T>(byte[] bytes, ContentType contentType) =>
            contentType.MediaType == KnownMediaType.ApplicationXml;

        public T Convert<T>(byte[] bytes, ContentType contentType, Encoding encoding)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream(bytes))
                return (T) serializer.Deserialize(stream);
        }
    }
}