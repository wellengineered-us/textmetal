/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WellEngineered.TextMetal.Model.File
{
	public class ReflectionModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the ReflectionSourceStrategy class.
		/// </summary>
		public ReflectionModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		private static bool IsRealMemberInfo(MethodInfo methodInfo)
		{
			PropertyInfo[] propertyInfos;
			EventInfo[] eventInfos;
			MethodInfo accessorMethodInfo = null;

			if ((object)methodInfo == null)
				throw new ArgumentNullException(nameof(methodInfo));

			propertyInfos = methodInfo.DeclaringType.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

			if ((object)propertyInfos != null)
			{
				foreach (PropertyInfo propertyInfo in propertyInfos)
				{
					accessorMethodInfo = propertyInfo.GetGetMethod(true);

					if ((object)accessorMethodInfo != null && accessorMethodInfo.Equals(methodInfo))
						return false;

					accessorMethodInfo = propertyInfo.GetSetMethod(true);
					if ((object)accessorMethodInfo != null && accessorMethodInfo.Equals(methodInfo))
						return false;
				}
			}

			eventInfos = methodInfo.DeclaringType.GetEvents(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

			if ((object)eventInfos != null)
			{
				foreach (EventInfo eventInfo in eventInfos)
				{
					accessorMethodInfo = eventInfo.GetAddMethod(true);

					if ((object)accessorMethodInfo != null && accessorMethodInfo.Equals(methodInfo))
						return false;

					accessorMethodInfo = eventInfo.GetRemoveMethod(true);
					if ((object)accessorMethodInfo != null && accessorMethodInfo.Equals(methodInfo))
						return false;
				}
			}

			return true;
		}

		private static void ModelAssemblies(Assembly[] assemblies, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;
			
			Type[] types;
			AssemblyName[] assemblyReferences;
			AssemblyName assemblyName;

			if ((object)assemblies == null)
				throw new ArgumentNullException(nameof(assemblies));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Assemblies = items;

			foreach (Assembly assembly in assemblies)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				current.AssemblyIsDynamic = assembly.IsDynamic;
				current.AssemblyFullName = assembly.FullName;
				current.AssemblyManifestModuleName = assembly.ManifestModule.Name;
				current.AssemblyManifestModuleFullyQualifiedName = assembly.ManifestModule.FullyQualifiedName;
				
				assemblyName = assembly.GetName();

				ModelAssemblyName(assemblyName, current);

				ModelCustomAttributes(assembly, current);

				types = assembly.GetExportedTypes();

				ModelTypes("Types", types, current);

				//ModelAssemblyReferences(assemblyReferences, current);
			}
		}

		private static void ModelAssemblyName(AssemblyName assemblyName, dynamic parent)
		{
			if ((object)assemblyName == null)
				throw new ArgumentNullException(nameof(assemblyName));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));
			
			parent.AssemblyVersion = assemblyName.Version;
			parent.AssemblyName = assemblyName.Name;
			parent.AssemblyContentType = assemblyName.ContentType;
			parent.AssemblyCultureName = assemblyName.CultureName;
			parent.AssemblyFullName = assemblyName.FullName;
			parent.AssemblyFlags = assemblyName.Flags;
			parent.AssemblyProcessorArchitecture = assemblyName.ProcessorArchitecture;
		}

		private static void ModelAssemblyReferences(AssemblyName[] assemblyReferences, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			if ((object)assemblyReferences == null)
				throw new ArgumentNullException(nameof(assemblyReferences));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.AssemblyReferences = items;

			foreach (AssemblyName assemblyReference in assemblyReferences)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				ModelAssemblyName(assemblyReference, current);
			}
		}

		private static void ModelConstructors(ConstructorInfo[] constructorInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			ParameterInfo[] parameterInfos;

			if ((object)constructorInfos == null)
				throw new ArgumentNullException(nameof(constructorInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Constructors = items;

			foreach (ConstructorInfo constructorInfo in constructorInfos)
			{
				current = new Dictionary<string, object>();
				items.Add(current);
				
				current.ConstructorName = constructorInfo.Name;
				current.ConstructorCallingConvention = constructorInfo.CallingConvention;
				current.ConstructorContainsGenericParameters = constructorInfo.ContainsGenericParameters;
				current.ConstructorIsGenericMethod = constructorInfo.IsGenericMethod;
				current.ConstructorIsGenericMethodDefinition = constructorInfo.IsGenericMethodDefinition;
				current.ConstructorIsAbstract = constructorInfo.IsAbstract;
				current.ConstructorIsAssembly = constructorInfo.IsAssembly;
				current.ConstructorIsFamily = constructorInfo.IsFamily;
				current.ConstructorIsFamilyAndAssembly = constructorInfo.IsFamilyAndAssembly;
				current.ConstructorIsFamilyOrAssembly = constructorInfo.IsFamilyOrAssembly;
				current.ConstructorIsFinal = constructorInfo.IsFinal;
				current.ConstructorIsHideBySig = constructorInfo.IsHideBySig;
				current.ConstructorIsPrivate = constructorInfo.IsPrivate;
				current.ConstructorIsPublic = constructorInfo.IsPublic;
				current.ConstructorIsSpecialName = constructorInfo.IsSpecialName;
				current.ConstructorIsStatic = constructorInfo.IsStatic;
				current.ConstructorIsVirtual = constructorInfo.IsVirtual;
				current.ConstructorMethodImplementationFlags = constructorInfo.MethodImplementationFlags;
				
				ModelCustomAttributes(constructorInfo, current);

				parameterInfos = constructorInfo.GetParameters();

				ModelParameters(parameterInfos, current);
			}
		}

		private static void ModelCustomAttributes(ICustomAttributeProvider customAttributeProvider, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;
			
			dynamic child;
			IList<dynamic> childItems;

			Attribute[] customAttributes;
			PropertyInfo[] publicPropertyInfos;
			object value;

			if ((object)customAttributeProvider == null)
				throw new ArgumentNullException(nameof(customAttributeProvider));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			customAttributes = SolderFascadeAccessor.ReflectionFascade.GetAllAttributes<Attribute>(customAttributeProvider);

			items = new List<dynamic>();
			parent.CustomAttributes = items;

			if ((object)customAttributes != null)
			{
				foreach (Attribute customAttribute in customAttributes)
				{
					current = new Dictionary<string, object>();
					items.Add(current);

					current.CustomAttributeTypeFullName = customAttribute.GetType().FullName;

					publicPropertyInfos = customAttribute.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

					if ((object)publicPropertyInfos != null)
					{
						childItems = new List<dynamic>();
						current.CustomAttributeProperties = childItems;

						foreach (PropertyInfo publicPropertyInfo in publicPropertyInfos)
						{
							child = new Dictionary<string, object>();
							childItems.Add(child);

							child.CustomAttributePropertyName = publicPropertyInfo.Name;

							if (SolderFascadeAccessor.ReflectionFascade.GetLogicalPropertyValue(customAttribute, publicPropertyInfo.Name, out value))
								child.CustomAttributePropertyValue = value.SafeToString();
							else
								child.CustomAttributePropertyValue = null;
						}
					}
				}
			}
		}

		private static void ModelEvents(EventInfo[] eventInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			MethodInfo methodInfo;

			if ((object)eventInfos == null)
				throw new ArgumentNullException(nameof(eventInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Events = items;
			
			foreach (EventInfo eventInfo in eventInfos)
			{
				current = new Dictionary<string, object>();
				items.Add(current);
				
				current.EventName = eventInfo.Name;
				current.EventHandlerTypeName = eventInfo.EventHandlerType.Name;
				current.EventHandlerTypeNamespace = eventInfo.EventHandlerType.Namespace;
				current.EventHandlerTypeFullName = eventInfo.EventHandlerType.FullName;
				current.EventHandlerTypeAssemblyQualifiedName = eventInfo.EventHandlerType.AssemblyQualifiedName;
				current.EventIsSpecialName = eventInfo.IsSpecialName;
				
				methodInfo = eventInfo.GetAddMethod();

				if ((object)methodInfo != null)
				{
					current.EventAddMethodIsStatic = methodInfo.IsStatic;
				}

				methodInfo = eventInfo.GetRemoveMethod();

				if ((object)methodInfo != null)
				{
					current.EventRemoveMethodIsStatic = methodInfo.IsStatic;
				}

				methodInfo = eventInfo.GetRaiseMethod();

				if ((object)methodInfo != null)
				{
					current.EventRaiseMethodIsStatic = methodInfo.IsStatic;
				}

				ModelCustomAttributes(eventInfo, current);
			}
		}

		private static void ModelFields(FieldInfo[] fieldInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			if ((object)fieldInfos == null)
				throw new ArgumentNullException(nameof(fieldInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Fields = items;

			foreach (FieldInfo fieldInfo in fieldInfos)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				current.FieldName = fieldInfo.Name;
				current.FieldTypeName = fieldInfo.FieldType.Name;
				current.FieldTypeNamespace = fieldInfo.FieldType.Namespace;
				current.FieldTypeFullName = fieldInfo.FieldType.FullName;
				current.FieldTypeAssemblyQualifiedName = fieldInfo.FieldType.AssemblyQualifiedName;
				current.FieldIsAssembly = fieldInfo.IsAssembly;
				current.FieldIsFamily = fieldInfo.IsFamily;
				current.FieldIsFamilyAndAssembly = fieldInfo.IsFamilyAndAssembly;
				current.FieldIsFamilyOrAssembly = fieldInfo.IsFamilyOrAssembly;
				current.FieldIsInitOnly = fieldInfo.IsInitOnly;
				current.FieldIsLiteral = fieldInfo.IsLiteral;

				if (fieldInfo.IsLiteral)
				{
					current.FieldRawConstantValue = null;
				}

				current.FieldIsPrivate = fieldInfo.IsPrivate;
				current.FieldIsPublic = fieldInfo.IsPublic;
				current.FieldIsSpecialName = fieldInfo.IsSpecialName;
				current.FieldIsStatic = fieldInfo.IsStatic;

				ModelCustomAttributes(fieldInfo, current);
			}
		}

		private static void ModelMethods(MethodInfo[] methodInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			ParameterInfo[] parameterInfos;
			Type[] childTypes;

			if ((object)methodInfos == null)
				throw new ArgumentNullException(nameof(methodInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Methods = items;

			foreach (MethodInfo methodInfo in methodInfos)
			{
				if (!IsRealMemberInfo(methodInfo))
					continue;

				current = new Dictionary<string, object>();
				items.Add(current);

				current.MethodName = methodInfo.Name;
				current.MethodCallingConvention = methodInfo.CallingConvention;
				current.MethodContainsGenericParameters = methodInfo.ContainsGenericParameters;
				current.MethodIsAbstract = methodInfo.IsAbstract;
				current.MethodIsAssembly = methodInfo.IsAssembly;
				current.MethodIsConstructor = methodInfo.IsConstructor;
				current.MethodIsFamily = methodInfo.IsFamily;
				current.MethodIsFamilyAndAssembly = methodInfo.IsFamilyAndAssembly;
				current.MethodIsFamilyOrAssembly = methodInfo.IsFamilyOrAssembly;
				current.MethodIsFinal = methodInfo.IsFinal;
				current.MethodIsGenericMethod = methodInfo.IsGenericMethod;
				current.MethodIsGenericMethodDefinition = methodInfo.IsGenericMethodDefinition;
				current.MethodIsHideBySig = methodInfo.IsHideBySig;
				current.MethodIsPrivate = methodInfo.IsPrivate;
				current.MethodIsPublic = methodInfo.IsPublic;
				current.MethodIsSpecialName = methodInfo.IsSpecialName;
				current.MethodIsStatic = methodInfo.IsStatic;
				current.MethodIsVirtual = methodInfo.IsVirtual;
				current.MethodImplementationFlags = methodInfo.MethodImplementationFlags;
				current.MethodReturnTypeName = methodInfo.ReturnType.Name;
				current.MethodReturnTypeNamespace = methodInfo.ReturnType.Namespace;
				current.MethodReturnTypeFullName = methodInfo.ReturnType.FullName;
				current.MethodReturnTypeAssemblyQualifiedName = methodInfo.ReturnType.AssemblyQualifiedName;

				if (methodInfo.IsGenericMethod)
				{
					var _methodInfo = methodInfo.GetGenericMethodDefinition();

					current.MethodGenericMethodDefinitionName = _methodInfo.DeclaringType.Name;
					current.MethodGenericMethodDefinitionNamespace = _methodInfo.DeclaringType.Namespace;
					current.MethodGenericMethodDefinitionFullName = _methodInfo.DeclaringType.FullName;
					current.MethodGenericMethodDefinitionAssemblyQualifiedName = _methodInfo.DeclaringType.AssemblyQualifiedName;
				}

				ModelCustomAttributes(methodInfo, current);

				parameterInfos = methodInfo.GetParameters();
				Array.Resize(ref parameterInfos, parameterInfos.Length + 1);
				parameterInfos[parameterInfos.Length - 1] = methodInfo.ReturnParameter;

				ModelParameters(parameterInfos, current);

				childTypes = methodInfo.GetGenericArguments();

				ModelTypes("GenericArguments", childTypes, current);
			}
		}

		private static void ModelParameters(ParameterInfo[] parameterInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			if ((object)parameterInfos == null)
				throw new ArgumentNullException(nameof(parameterInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Parameters = items;

			foreach (ParameterInfo parameterInfo in parameterInfos)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				current.ParameterName = parameterInfo.Name;
				current.ParameterDefaultValue = parameterInfo.DefaultValue;
				current.ParameterHasDefaultValue = parameterInfo.HasDefaultValue;
				current.ParameterIsIn = parameterInfo.IsIn;
				current.ParameterIsOptional = parameterInfo.IsOptional;
				current.ParameterIsOut = parameterInfo.IsOut;
				current.ParameterIsRetval = parameterInfo.IsRetval;
				current.ParameterTypeName = parameterInfo.ParameterType.Name;
				current.ParameterTypeNamespace = parameterInfo.ParameterType.Namespace;
				current.ParameterTypeFullName = parameterInfo.ParameterType.FullName;
				current.ParameterTypeAssemblyQualifiedName = parameterInfo.ParameterType.AssemblyQualifiedName;
				current.ParameterIsByRef = parameterInfo.ParameterType.IsByRef;
				current.ParameterPosition = parameterInfo.Position;

				ModelCustomAttributes(parameterInfo, current);
			}
		}

		private static void ModelProperties(PropertyInfo[] propertyInfos, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			ParameterInfo[] parameterInfos;
			MethodInfo methodInfo;

			if ((object)propertyInfos == null)
				throw new ArgumentNullException(nameof(propertyInfos));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent.Properties = items;

			foreach (PropertyInfo propertyInfo in propertyInfos)
			{
				current = new Dictionary<string, object>();
				items.Add(current);

				current.PropertyName = propertyInfo.Name;
				current.PropertyCanRead = propertyInfo.CanRead;
				current.PropertyCanWrite = propertyInfo.CanWrite;
				current.PropertyIsSpecialName = propertyInfo.IsSpecialName;
				current.PropertyTypeName = propertyInfo.PropertyType.Name;
				current.PropertyTypeNamespace = propertyInfo.PropertyType.Namespace;
				current.PropertyTypeFullName = propertyInfo.PropertyType.FullName;
				current.PropertyTypeAssemblyQualifiedName = propertyInfo.PropertyType.AssemblyQualifiedName;

				methodInfo = propertyInfo.GetGetMethod();

				if ((object)methodInfo != null)
				{
					current.PropertyGetMethodIsStatic = methodInfo.IsStatic;
				}

				methodInfo = propertyInfo.GetSetMethod();

				if ((object)methodInfo != null)
				{
					current.PropertySetMethodIsStatic = methodInfo.IsStatic;
				}

				ModelCustomAttributes(propertyInfo, current);

				parameterInfos = propertyInfo.GetIndexParameters();

				ModelParameters(parameterInfos, current);
			}
		}

		private static void ModelTypes(string arrayName, Type[] types, dynamic parent)
		{
			dynamic current;
			IList<dynamic> items;

			FieldInfo[] fieldInfos;
			PropertyInfo[] propertyInfos;
			MethodInfo[] methodInfos;
			EventInfo[] eventInfos;
			ConstructorInfo[] constructorInfos;

			Type[] childTypes;

			if ((object)types == null)
				throw new ArgumentNullException(nameof(types));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));

			items = new List<dynamic>();
			parent[arrayName] = items;

			foreach (Type type in types)
			{
				//Console.WriteLine("{0} ==> '{1}'", arrayName, type.FullName);

				current = new Dictionary<string, object>();
				items.Add(current);

				current.TypeName = type.Name;
				current.TypeNamespace = type.Namespace;
				current.TypeFullName = type.FullName;
				current.TypeAssemblyQualifiedName = type.AssemblyQualifiedName;

				var _typeInfo = type.GetTypeInfo();

				if ((object)_typeInfo.BaseType != null)
				{
					current.TypeBaseName = _typeInfo.BaseType.Name;
					current.TypeBaseNamespace = _typeInfo.BaseType.Namespace;
					current.TypeBaseFullName = _typeInfo.BaseType.FullName;
					current.TypeBaseAssemblyQualifiedName = _typeInfo.BaseType.AssemblyQualifiedName;
				}

				current.TypeGuid = _typeInfo.GUID;
				current.TypeIsAbstract = _typeInfo.IsAbstract;
				current.TypeIsAnsiClass = _typeInfo.IsAnsiClass;
				current.TypeIsArray = _typeInfo.IsArray;
				current.TypeIsAutoClass = _typeInfo.IsAutoClass;
				current.TypeIsAutoLayout = _typeInfo.IsAutoLayout;
				current.TypeIsByRef = type.IsByRef;
				current.TypeIsClass = _typeInfo.IsClass;
				current.TypeIsComObject = _typeInfo.IsCOMObject;
				current.TypeIsConstructedGenericType = _typeInfo.GetGenericTypeDefinition().IsConstructedGenericType;
				current.TypeIsEnum = _typeInfo.IsEnum;
				current.TypeIsExplicitLayout = _typeInfo.IsExplicitLayout;
				current.TypeIsGenericParameter = _typeInfo.IsGenericParameter;
				current.TypeIsGenericType = _typeInfo.IsGenericType;
				current.TypeIsGenericTypeDefinition = _typeInfo.IsGenericTypeDefinition;
				current.TypeIsImport = _typeInfo.IsImport;
				current.TypeIsInterface = _typeInfo.IsInterface;
				current.TypeIsLayoutSequential = _typeInfo.IsLayoutSequential;
				current.TypeIsMarshalByRef = _typeInfo.IsMarshalByRef;
				current.TypeIsNested = type.IsNested;
				current.TypeIsNestedAssembly = _typeInfo.IsNestedAssembly;
				current.TypeIsNestedFamANDAssem = _typeInfo.IsNestedFamANDAssem;
				current.TypeIsNestedFamORAssem = _typeInfo.IsNestedFamORAssem;
				current.TypeIsNestedFamily = _typeInfo.IsNestedFamily;
				current.TypeIsNestedPrivate = _typeInfo.IsNestedPrivate;
				current.TypeIsNestedPublic = _typeInfo.IsNestedPublic;
				current.TypeIsNotPublic = _typeInfo.IsNotPublic;
				current.TypeIsPointer = type.IsPointer;
				current.TypeIsPrimitive = _typeInfo.IsPrimitive;
				current.TypeIsPublic = _typeInfo.IsPublic;
				current.TypeIsSealed = _typeInfo.IsSealed;
				current.TypeIsSerializable = _typeInfo.IsSerializable;
				current.TypeIsSpecialName = _typeInfo.IsSpecialName;
				current.TypeIsUnicodeClass = _typeInfo.IsUnicodeClass;
				current.TypeIsValueType = _typeInfo.IsValueType;
				current.TypeIsVisible = _typeInfo.IsVisible;
				current.TypeNamespace = _typeInfo.Namespace;
				current.TypeContainsGenericParameters = _typeInfo.ContainsGenericParameters;

				if (_typeInfo.IsGenericParameter)
				{
					current.TypeGenericParameterAttributes = _typeInfo.GenericParameterAttributes;
					current.TypeGenericParameterPosition = type.GenericParameterPosition;
				}

				if (_typeInfo.IsGenericType)
				{
					var _genericType = _typeInfo.GetGenericTypeDefinition();

					current.TypeGenericTypeDefinitionName = _genericType.Name;
					current.TypeGenericTypeDefinitionNamespace = _genericType.Namespace;
					current.TypeGenericTypeDefinitionFullName = _genericType.FullName;
					current.TypeGenericTypeDefinitionAssemblyQualifiedName = _genericType.AssemblyQualifiedName;
				}

				ModelCustomAttributes(type, current);

				fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelFields(fieldInfos, current);

				propertyInfos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelProperties(propertyInfos, current);

				methodInfos = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelMethods(methodInfos, current);

				eventInfos = type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelEvents(eventInfos, current);

				constructorInfos = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelConstructors(constructorInfos, current);

				childTypes = type.GenericTypeArguments;

				ModelTypes("GenericTypeArguments", childTypes, current);

				childTypes = type.GetGenericArguments();

				ModelTypes("GenericArguments", childTypes, current);

				if (_typeInfo.IsGenericParameter)
				{
					childTypes = _typeInfo.GetGenericParameterConstraints();

					ModelTypes("GenericParameterConstraints", childTypes, current);
				}

				childTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

				ModelTypes("NestedTypes", childTypes, current);
			}
		}

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_FILE_OR_DIRECTORY_PATH = "SourceFileOrDirectoryPath";
			object value;
			string sourceFileOrDirectoryPath = null;
			dynamic sourceObject;

			List<Assembly> assemblies;
			Assembly assembly;
			IEnumerable<AssemblyName> assemblyNames;

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));

			if (properties.TryGetValue(PROP_TOKEN_SOURCE_FILE_OR_DIRECTORY_PATH, out value))
			{
				if ((object)value != null)
					sourceFileOrDirectoryPath = (string)value;
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(sourceFileOrDirectoryPath))
				throw new InvalidOperationException(String.Format("The source file or directory path cannot be null or whitespace."));

			sourceFileOrDirectoryPath = Path.GetFullPath(sourceFileOrDirectoryPath);
			
			assemblies = new List<Assembly>();
			
			if (System.IO.File.Exists(sourceFileOrDirectoryPath))
				assemblyNames = new AssemblyName[] { new AssemblyName(sourceFileOrDirectoryPath) };
			else if (Directory.Exists(sourceFileOrDirectoryPath))
				assemblyNames = Directory.EnumerateFiles(sourceFileOrDirectoryPath, "*.*", SearchOption.TopDirectoryOnly).Select(f => new AssemblyName(f)); // 2016-11-01 (dpbullington@gmail.com): changed this to support wildcard directory search
			else
				assemblyNames = null;

			if ((object)assemblyNames != null)
			{
				foreach (AssemblyName assemblyName in assemblyNames)
				{
					// 2016-11-01 (dpbullington@gmail.com): changed this to fail gracefully and support wildcard directory search
					try
					{
						assembly = Assembly.Load(assemblyName);
					}
					catch (ReflectionTypeLoadException)
					{
						assembly = null;
					}

					if ((object)assembly != null)
						assemblies.Add(assembly);

					//if ((object)assembly == null) throw new InvalidOperationException(string.Format("Failed to load the assembly file '{0}' via Assembly.LoadFile(..).", sourceFilePath));}
				}
			}

			sourceObject = new Dictionary<string, object>();

			ModelAssemblies(assemblies.ToArray(), sourceObject);

			return sourceObject;
		}

		#endregion

		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			await Task.CompletedTask;
			return this.CoreGetModelObject(properties);
		}
	}
}