using System;
using System.ComponentModel.DataAnnotations;
namespace Unibim.Melanom.WebUI
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
        ResultType ResultType { get; set; }
        ValidationInformation ValidationInformation { get; }
    }

    public enum ResultType
    {
        Information = 1,
        Validation = 2,
        Warning = 4,
        Error = 8,
        Fatal = 16
    };
}
