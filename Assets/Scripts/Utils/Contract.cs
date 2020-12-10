using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

internal static class Contract
{
    /// Condition to raise a particular Exception
    /// <typeparam name="T">Generic exeption</typeparam>
    /// <param name="condition">Condition to raise the exception</param>
    /// <param name="message">Message to return if the exception is raised</param>
    [System.Diagnostics.DebuggerStepThrough]
	public static void Requires<T>(bool condition, string message = null) where T : Exception, new()
	{
			
		if (!condition)
		{
			//An exception of type T that is created using reflexion
			//throw (T)Activator.CreateInstance(typeof(T), message);
		}
	}

    /// Condition to raise a particular Exception
    /// <param name="condition">CCondition to raise the exception</param>
    /// <param name="message">Message to return if the exception is raised</param>
    /// <exception cref="Exception"></exception>
    [System.Diagnostics.DebuggerStepThrough]
	public static void Requires(bool condition, string message = null)
	{
		if (!condition)
		{
			//An exception of type T that is created using reflexion
			//throw new Exception(message);
		}
	}
}
