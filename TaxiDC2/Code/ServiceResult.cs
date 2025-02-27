// ------------------------------------------
// Project : AUREL AIS
// Module : Core
// ------------------------------------------
// Copyright (c) 2020 Advisor Software s.r.o
// ------------------------------------------

using System.Diagnostics;

namespace TaxiDC2
{
	[DebuggerDisplay("State : ({State}) {Message}")]
	public class ServiceState(
		StateCode State = StateCode.INFO,
		string message = "",
		Exception exception = null)
	{
		public bool IsError => State == StateCode.ERROR || State == StateCode.EXCEPTION;
		public bool IsSuccess => State == StateCode.OK;
		public string Message { get; } = message;
		public Exception Exception { get; } = exception;
		public StateCode State { get; internal set; } = State;
	}
	
	[DebuggerDisplay("State : ({State}) {Message}")]
	public class ServiceState<T>(
		StateCode State = StateCode.INFO,
		T data = default,
		string message = "",
		Exception exception = null) : ServiceState(State, message, exception)

	{
		public T Data { get; internal set; } = data;
	}

	public enum StateCode
	{
		OK = 1,
		ERROR = 2,
		EXCEPTION = 3,
		INFO = 4,
		NOTFOUND = 5,
		NODATA = 6
	}

	public static class StateFactory
	{
		public static ServiceState Ok(bool data) => new ServiceState(StateCode.OK);
		public static ServiceState Ok() => new ServiceState(StateCode.OK);
		public static ServiceState Ok(bool data, string message) => new ServiceState(StateCode.OK, message);
		public static ServiceState Ok(string message) => new ServiceState(StateCode.OK, message);
		public static ServiceState Error(string message) => new ServiceState(StateCode.ERROR, message);
		public static ServiceState Exception(Exception ex) => new ServiceState(StateCode.ERROR, ex.Message, ex);
		public static ServiceState Exception(Exception ex, string message) => new ServiceState(StateCode.ERROR, message, ex);
	}

	public static class StateFactory<T>
	{
		public static ServiceState<T> Ok(T data) => new ServiceState<T>(StateCode.OK, data);
		public static ServiceState<T> Ok() => new ServiceState<T>(StateCode.OK);
		public static ServiceState<T> Ok(T data, string message) => new ServiceState<T>(StateCode.OK, data, message);
		public static ServiceState<T> Ok(string message) => new ServiceState<T>(StateCode.OK, message: message);
		public static ServiceState<T> Error(string message) => new ServiceState<T>(StateCode.ERROR, message: message);
		public static ServiceState<T> Exception(Exception ex) => new ServiceState<T>(StateCode.ERROR, message: ex.Message, exception: ex);
		public static ServiceState<T> Exception(Exception ex, string message) => new ServiceState<T>(StateCode.ERROR, message: message, exception: ex);
	}
}