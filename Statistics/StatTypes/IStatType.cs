using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public interface IStatType
{
	public static List<Type> GetAllTypes()
	{
		Type parentType = typeof(IStatType);
		Assembly assembly = parentType.Assembly;

		var childTypes = assembly.GetTypes()
			.Where(t => t != parentType && parentType.IsAssignableFrom(t));
        
		return childTypes.ToList();
	}
}
