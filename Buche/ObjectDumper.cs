using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace Buche
{

	public class ObjectDumper
	{
		// max depth to which we go (to avoid stack overflow)
		private const int MaxDepth = 10;

		public static string Log(object element, string prefix = null)
		{
			try
			{
				StringBuilder sb = new StringBuilder();
				Log(element, string.Empty, ref sb, 0);
				return sb.ToString();
			}
			catch(Exception ex)
			{
				return "Exception while dumping object, " + ex.Message;
			}
		}

		private static void Log(object element, string prefix, ref StringBuilder sb, int currDepth)
		{
			if (string.IsNullOrEmpty(prefix))
				prefix = element.GetType().Name;

			currDepth++;
			if (currDepth >= MaxDepth)
			{
				return;
			}

			if (element == null || element is ValueType || element is string)
			{
				sb.Append(prefix + "=" + WriteValue(element) + "; ");
			}

			// is the object an enumerable?
			IEnumerable enumerableElement = element as IEnumerable;
			if (enumerableElement != null)
			{
				int count = 0;
				foreach (object item in enumerableElement)
				{
					if (item == null || item is ValueType || item is string)
					{
						sb.Append(prefix + "[" + count + "]=" + WriteValue(item) + "; ");
					}
					else
					{
						string newPrefix = prefix + "." + item.GetType().Name + "[" + count + "]";
						Log(item, newPrefix, ref sb, currDepth);
					}
					count++;
				}
				// if the object is an empty enumerable print that count is 0
				if (count == 0)
					sb.Append(prefix + ".count=0;");
			}
			else
			{
			    if (element != null)
			    {
			        MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
			        foreach (MemberInfo m in members)
			        {
			            FieldInfo f = m as FieldInfo;
			            PropertyInfo p = m as PropertyInfo;
			            if (f != null || p != null)
			            {
			                Type t = f != null ? f.FieldType : p.PropertyType;
			                object value = f != null ? f.GetValue(element) : p.GetValue(element, null);
			                if (t.IsValueType || t == typeof(string) || value == null)
			                {
			                    sb.Append(prefix + "." + m.Name + "=");
			                    // if the name contains password or securityAnswer, do not print it
			                    if (LogUtil.IsSensitiveKey(m.Name))
			                        sb.Append("********");
			                    else
			                        sb.Append(WriteValue(value));
			                    sb.Append("; ");
			                }
			                else
			                {
			                    string newPrefix = prefix + "." + m.Name;
			                    Log(value, newPrefix, ref sb, currDepth);
			                }
			            }
			        }
			    }
			}
		}

		private static string WriteValue(object o)
		{
			if (o == null)
				return "null";
			if (o is DateTime)
				return ((DateTime)o).ToShortDateString();
			if (o is ValueType || o is string)
				return o.ToString();
			if (o is IEnumerable)
				return "...";
			return "{ }";
		}
	}


	
}
