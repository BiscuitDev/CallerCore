using System;
using System.Collections.Generic;
/// <summary>
/// @author BiscuitDev
/// @date 20-apr-2015
/// </summary>

namespace CallerCore.MainCore
{


	/// <summary>
	// FunctionContext passing data to functions
	/// 
	/// @author BiscuitDev
	/// </summary>
	public class FunctionContext
	{

		public string msg { get; set; }
		public string type { get; set; }
		private Dictionary<string, object> _mapIndex { get; set; }

		public FunctionContext() : this(type: null, msg: null)
		{
		}

		public FunctionContext(string type) : this(type: type, msg: null)
		{
		}

		public FunctionContext(Enum e) : this(type: e.ToString(), msg: null)
		{
		}
		public FunctionContext(Enum e, string msg) : this(type: e.ToString(), msg: msg)
		{
		}

		public FunctionContext(string type, string msg)
		{
			this.msg = msg;
			this.type = type;
			_mapIndex = new Dictionary<string, object>();
		}

		/// <summary>
		/// FunctionContext copy
		/// </summary>
		/// <param name="context">	il contesto di esecuzione da copiare. </param>
		public FunctionContext(FunctionContext context)
		{
			this.msg = context.msg;
			this.type = context.type;
			this._mapIndex = new Dictionary<string, object>(context._mapIndex);
		}
		public virtual bool EqualsType(Enum e)
		{
			if (type != null && type == e.ToString())
				return true;
			else
				return false;
		}

		public virtual void resetParams()
		{
			foreach (var @map in _mapIndex)
			{
				if (@map.Value != null && @map.Value is IDisposable) ((IDisposable)@map.Value).Dispose();
			}
			_mapIndex.Clear();
			this.msg = null;
			this.type = null;

		}
		public virtual int NumParam
		{
			get
			{
				return @_mapIndex.Count;
			}
		}
		public virtual bool ContainsKey(string sindex)
		{
			return _mapIndex.ContainsKey(sindex);
		}

		public virtual void setParam(string sindex, object obj)
		{
			_mapIndex.Add(sindex, obj);
		}
		public virtual void setParam(int i, object obj)
		{
			_mapIndex.Add(i.ToString(), obj);
		}
		public virtual object getParam(string sindex, object @default = null)
		{
			return _mapIndex.ContainsKey(sindex) ? _mapIndex[sindex] : @default;
		}
		public virtual object getParam(int i)
		{
			return getParam(i.ToString());
		}
		public virtual void setIntParam(string sindex, int value)
		{
			setParam(sindex, new int?(value));
		}
		public virtual void setIntParam(int i, int value)
		{
			setParam(i, new int?(value));
		}
		public virtual int getIntOrDefaultParam(string sindex, int i = default(int))
		{
			if (!_mapIndex.ContainsKey(sindex)) return i;
			return (int)_mapIndex[sindex];
		}
		public virtual int? getIntParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as int?;
		}
		public virtual int getIntParam(int i)
		{
			return getIntOrDefaultParam(i.ToString());
		}

		public virtual void setLongParam(string sindex, long value)
		{
			setParam(sindex, new long?(value));
		}
		public virtual long getLongOrDefaultParam(string sindex, long l = default(long))
		{
			if (!_mapIndex.ContainsKey(sindex)) return l;
			return (long)_mapIndex[sindex];
		}
		public virtual long? getLongParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as long?;
		}

		public virtual long getLongParam(int l)
		{
			return getLongOrDefaultParam(l.ToString());
		}

		public virtual void setDoubleValue(string sindex, double value)
		{
			setParam(sindex, new double?(value));
		}
		public virtual double getDoubleOrDefaultParam(string sindex, double d = default(double))
		{
			if (!_mapIndex.ContainsKey(sindex)) return d;
			return (double)_mapIndex[sindex];
		}
		public virtual double? getDoubleParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as double?;
		}
		public virtual double getDoubleParam(int d)
		{
			return getDoubleOrDefaultParam(d.ToString());
		}

		public virtual void setStringParam(string sindex, string value)
		{
			setParam(sindex, value);
		}
		public virtual void setStringParam(int i, string value)
		{
			setParam(i.ToString(), value);
		}
		public virtual string getStringParam(string sindex, string s = default(string))
		{
			if (!_mapIndex.ContainsKey(sindex)) return s;
			return _mapIndex[sindex] as string;
		}

		public virtual string getStringParam(int i)
		{
			return getStringParam(i.ToString());
		}

		public virtual void setByteParam(string sindex, byte value)
		{
			setParam(sindex, new byte?(value));
		}
		public virtual byte getByteOrDefaultParam(string sindex, byte b = default(byte))
		{
			if (!_mapIndex.ContainsKey(sindex)) return b;
			return (byte)_mapIndex[sindex];
		}
		public virtual byte? getByteParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as byte?;
		}
		public virtual void setBytesParam(string sindex, byte[] value)
		{
			setParam(sindex, value);
		}
		public virtual byte[] getBytesParam(string sindex, byte[] b = default(byte[]))
		{
			if (!_mapIndex.ContainsKey(sindex)) return b;
			return _mapIndex[sindex] as byte[];
		}

		public virtual void setBooleanParam(string sindex, bool value)
		{
			setParam(sindex, new bool?(value));
		}
		public virtual void setBooleanParam(int i, bool value)
		{
			setParam(i.ToString(), new bool?(value));
		}

		public virtual bool getBooleanOrDefaultParam(string sindex, bool b = default(bool))
		{
			if (!_mapIndex.ContainsKey(sindex)) return b;
			return (bool)_mapIndex[sindex];
		}
		public virtual bool? getBooleanParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as bool?;
		}
		public virtual bool getBooleanParam(int b)
		{
			return getBooleanOrDefaultParam(b.ToString());
		}

		public virtual void setCharParam(string sindex, char value)
		{
			setParam(sindex, new char?(value));
		}

		public virtual char getCharOrDefaultParam(string sindex, char c = default(char))
		{
			if (!_mapIndex.ContainsKey(sindex)) return c;
			return (char)_mapIndex[sindex];
		}

		public virtual char? getCharParam(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return null;
			return _mapIndex[sindex] as char?;
		}

		public virtual void setObject<T>(string sindex, T t)
		{
			setParam(sindex, t);
		}
		public virtual void setObject<T>(int i, T t)
		{
			setParam(i.ToString(), t);
		}
		public virtual T getObject<T>(string sindex)
		{
			if (!_mapIndex.ContainsKey(sindex)) return default(T);
			return (T)_mapIndex[sindex];
		}
		public virtual T getObject<T>(int i)
		{
			return getObject<T>(i.ToString());
		}

		public static FunctionContext NewFunctionContext()
		{
			FunctionContext fctx = new FunctionContext();
			return fctx;
		}
		public static FunctionContext NewFunctionContext(Enum e)
		{
			FunctionContext fctx = new FunctionContext(type: e.ToString());
			return fctx;
		}
		public static FunctionContext NewFunctionContext(string type)
		{
			FunctionContext fctx = new FunctionContext(type: type);
			return fctx;
		}
		public static FunctionContext NewFunctionContext(string type, string msg)
		{
			FunctionContext fctx = new FunctionContext(type: type, msg: msg);
			return fctx;
		}
		public static FunctionContext NewFunctionContext(Enum e, string msg)
		{
			FunctionContext fctx = new FunctionContext(type: e.ToString(), msg: msg);
			return fctx;
		}

	}

	public static class FunctionContextExtensions
	{
		public static FunctionContext AddType(this FunctionContext fctx, string newtype)
		{
			fctx.type = newtype;
			return fctx;
		}
		public static FunctionContext AddType(this FunctionContext fctx, Enum e)
		{
			fctx.type = e.ToString();
			return fctx;
		}
		public static FunctionContext AddParam(this FunctionContext fctx, string sindex, object param)
		{
			fctx.setParam(sindex, param);
			return fctx;
		}
	}

}