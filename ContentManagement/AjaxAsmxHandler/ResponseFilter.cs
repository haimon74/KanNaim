using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;

namespace AJAXASMXHandler
{
    public class ResponseFilter : Stream
    {
        Stream responseStream;
        long length;

        public ResponseFilter(HttpResponse response)
        {
            this.responseStream = response.Filter;
        }

        #region Filter overrides
        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Close()
        {
            responseStream.Close();
        }

        public override void Flush()
        {
            responseStream.Flush();
        }

        public override long Length
        {
            get { return length; }
        }

        public override long Position
        {
            get { return 0; }
            set { }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return responseStream.Seek(offset, origin);
        }

        public override void SetLength(long length)
        {
            responseStream.SetLength(length);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return responseStream.Read(buffer, offset, count);
        }
        #endregion

        public override void Write(byte[] buffer, int offset, int count)
        {
            this.responseStream.Write(buffer, offset, count);
            length += count;
        }
    }
}
