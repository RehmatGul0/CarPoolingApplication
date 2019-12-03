using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarPoolingApp.DataTransferObjects
{
    public class ResponseDTO
    {
        public int status { get; set; }
        public string title { get; set; }

        public ResponseDTO(int status , string title)
        {
            this.status = status;
            this.title = title;
        }
    }
    public class ResponseDTOGet<T> : ResponseDTO
    {
        public T data { get; }
        public ResponseDTOGet(int status, string title , T data) : base(status,title)
        {
            this.data = data;
        }

    }
}
