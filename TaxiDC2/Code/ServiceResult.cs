// ------------------------------------------
// Project : AUREL AIS
// Module : Core
// ------------------------------------------
// Copyright (c) 2020 Advisor Software s.r.o
// ------------------------------------------

using System.Diagnostics;

namespace TaxiDC2
{
	[DebuggerDisplay("RESULT : ({Result}) {Message}")]
    public class ServiceResult(
	    ResultCode result = ResultCode.INFO,
	    bool data = false,
	    string message = "",
	    Exception exception = null)
	    : ServiceResult<bool>(result, data, message, exception);


    [DebuggerDisplay("RESULT : ({Result}) {Message}")]
    public class ServiceResult<T>(
	    ResultCode result = ResultCode.INFO,
	    T data = default,
	    string message = "",
	    Exception exception = null)

    {
	    public static explicit operator ServiceResult(ServiceResult<T> b) => new ServiceResult(b.Result,b.IsOk,b.Message,b.Exception);

        public bool IsOk => Result == ResultCode.OK;
        public bool IsError => Result == ResultCode.ERROR || Result == ResultCode.EXCEPTION;

        public T Data { get; internal set; } = data;
        public string Message { get; } = message;
        public Exception Exception { get; } = exception;
        public ResultCode Result { get; internal set; } = result;
        public bool Success => Result == ResultCode.OK;
        public bool NotSuccess => Result != ResultCode.OK;
        public ResultCode State => Result;
    }

	public enum ResultCode
    {
		OK = 1,
		ERROR = 2,
		EXCEPTION = 3,
		INFO = 4,
		NOTFOUND = 5,
		NODATA = 6
	}

    public static class ResultFactory
    {
        public static ServiceResult Ok(bool data) => new ServiceResult(ResultCode.OK, data);
        public static ServiceResult Ok() => new ServiceResult(ResultCode.OK);
        public static ServiceResult Ok(bool data, string message) => new ServiceResult(ResultCode.OK, data, message: message);
        public static ServiceResult Ok(string message) => new ServiceResult(ResultCode.OK, message: message);
        public static ServiceResult Error(string message) => new ServiceResult(ResultCode.ERROR, message: message);
        public static ServiceResult Exception(Exception ex) => new ServiceResult(ResultCode.ERROR, exception: ex, message: ex.Message);
        public static ServiceResult Exception(Exception ex, string message) => new ServiceResult(ResultCode.ERROR, exception: ex, message: message);
    }

	public static class ResultFactory<T>
    {
        public static ServiceResult<T> Ok(T data) => new ServiceResult<T>(ResultCode.OK, data);
        public static ServiceResult<T> Ok() => new ServiceResult<T>(ResultCode.OK);
        public static ServiceResult<T> Ok(T data, string message) => new ServiceResult<T>(ResultCode.OK, data, message: message);
        public static ServiceResult<T> Ok(string message) => new ServiceResult<T>(ResultCode.OK, message: message);
        public static ServiceResult<T> Error(string message) => new ServiceResult<T>(ResultCode.ERROR, message: message);
        public static ServiceResult<T> Exception(Exception ex) => new ServiceResult<T>(ResultCode.ERROR, exception: ex, message: ex.Message);
        public static ServiceResult<T> Exception(Exception ex, string message) => new ServiceResult<T>(ResultCode.ERROR, exception: ex, message: message);
    }
}