
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace Unibim.Melanom.WebUI
{
    [DataContract]
    public class Result<T> : IResult
    {
        private ValidationInformation validationInformation = new ValidationInformation();

        [DataMember]
        public bool IsSuccess { get; set; }

        [DataMember]
        public ResultType ResultType { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public ValidationInformation ValidationInformation
        {
            get
            {
                return this.validationInformation;
            }
            set
            {
                this.validationInformation = value;
            }
        }

        [DataMember]
        public T Data { get; set; }

        private Result()
        {
        }

        public Result(bool IsSuccess, ResultType ResultType, string Message)
            : this(IsSuccess, ResultType, Message, default(T), 0)
        {
        }

        public Result(bool IsSuccess, ResultType ResultType, string Message, T Data)
            : this(IsSuccess, ResultType, Message, Data, 0)
        {
        }

        public Result(bool IsSuccess, ResultType ResultType, string Message, T Data, int TotalCount)
        {
            this.IsSuccess = IsSuccess;
            this.ResultType = ResultType;
            this.Message = Message;
            this.Data = Data;
            this.TotalCount = TotalCount;
        }
    }
}
