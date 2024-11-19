using System;
using Godot;

namespace untitledplantgame.Common;

/// <summary>
/// Collection of convenient Assertions.
/// <para>
/// Assertions are conditions that are assumed to be true at a certain point in the code.
/// Developers can use assertions to check if the code is working as expected.
/// If any assertion fails, a message is logged and an exception is thrown.
/// </para>
/// <remarks>
/// Assertions are only enabled in debug mode
/// </remarks>
/// </summary>
public static class Assert
{
	private static readonly Logger Logger = new("Assertions");

	/// <summary>
	/// Exception thrown when an assertion fails.
	/// </summary>
	private class AssertionError : Exception
	{
		public AssertionError(string message) : base(message)
		{
		}
	}

	/// <summary>
	///  Asserts that the condition is true.
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
	///  Asserts that the condition is false.
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
	///		 Asserts that the two objects are the same.
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
	///  Asserts that the two objects are not the same.
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
	///  Asserts that the two arrays are the same. (Elementwise equal)
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
	/// Logs an error message and throws an exception.
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
}
