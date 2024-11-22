using System;
using Godot;

namespace untitledplantgame.Common;

// Functions could reuse AssertTrue()
// NOTE: Is it more readable for the error stack to have flatter structures?

/// <summary>
///     Collection of convenient Assertions.
///     <para>
///         Assertions are conditions that are assumed to be true at a certain point in the code.
///         Developers can use assertions to check if the code is working as expected.
///         If any assertion fails, a message is logged and an exception is thrown.
///     </para>
///     <remarks>
///         Assertions are only enabled in debug mode.
///     </remarks>
/// </summary>
public static class Assert
{
	private static readonly Logger Logger = new("Assertions");

	/// <summary>
	///     Asserts that the delegate returns true.
	/// </summary>
	/// <param name="delegate"></param>
	/// <param name="message"></param>
	public static void AssertTrue(Func<bool> @delegate, string message = null)
	{
		if (!@delegate())
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the condition is true.
	/// </summary>
	/// <param name="condition"></param>
	/// <param name="message"></param>
	public static void AssertTrue(bool condition, string message = null)
	{
		if (!condition)
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the condition is false.
	/// </summary>
	/// <param name="condition"></param>
	/// <param name="message"></param>
	public static void AssertFalse(bool condition, string message = null)
	{
		if (condition)
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the object is null.
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	public static void AssertNull(object obj, string message = null)
	{
		if (obj != null)
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the object is not null.
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="message"></param>
	public static void AssertNotNull(object obj, string message = null)
	{
		if (obj == null)
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the two objects are the same.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="message"></param>
	public static void AssertEquals(object a, object b, string message = null)
	{
		if (!a.Equals(b))
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the object action will throw an exception of type T.
	/// </summary>
	/// <param name="delegate"></param>
	/// <param name="message"></param>
	public static void AssertThrows<T>(Action @delegate, string message = null)
		where T : Exception
	{
		try
		{
			@delegate();
		}
		catch (T)
		{
			return;
		}

		RaiseError(message);
	}

	/// <summary>
	///     Asserts that the two objects are not the same.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="message"></param>
	public static void AssertNotSame(object a, object b, string message = null)
	{
		if (a == b)
		{
			RaiseError(message);
		}
	}

	/// <summary>
	///     Asserts that the two arrays are the same. (Elementwise equal)
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="message"></param>
	public static void AssertArrayEquals(object[] a, object[] b, string message = null)
	{
		if (a.Length != b.Length)
		{
			RaiseError(message);
		}

		for (var i = 0; i < a.Length; i++)
		{
			if (!a[i]?.Equals(b[i]) ?? b[i] != null)
			{
				RaiseError(message);
			}
		}
	}

	/// <summary>
	///     Logs an error message and throws an exception.
	/// </summary>
	/// <param name="message"></param>
	/// <exception cref="AssertionError"></exception>
	private static void RaiseError(string message)
	{
		if (OS.IsDebugBuild())
		{
			return;
		}

		if (message == null)
		{
			Logger.Error("Assertion failed");
			throw new AssertionError("Assertion failed");
		}

		Logger.Error(message);
		throw new AssertionError(message);
	}

	/// <summary>
	///     Exception thrown when an assertion fails.
	/// </summary>
	private class AssertionError : Exception
	{
		public AssertionError(string message)
			: base(message) { }
	}
}
