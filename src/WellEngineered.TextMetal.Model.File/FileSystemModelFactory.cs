/*
	Copyright ©2020-2021 WellEngineered.us, all rights reserved.
	Distributed under the MIT license: http://www.opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace WellEngineered.TextMetal.Model.File
{
	public class FileSystemModelFactory : TextMetalModelFactory
	{
		#region Constructors/Destructors

		/// <summary>
		/// Initializes a new instance of the FileSystemSourceStrategy class.
		/// </summary>
		public FileSystemModelFactory()
		{
		}

		#endregion

		#region Methods/Operators

		private static void EnumerateFileSystem(string directoryPath, bool recursive, string wildcard, dynamic parent, string sourcePath)
		{
			string[] directories;
			string[] files;
			dynamic current;
			IList<dynamic> items;

			if ((object)directoryPath == null)
				throw new ArgumentNullException(nameof(directoryPath));

			if ((object)parent == null)
				throw new ArgumentNullException(nameof(parent));
			
			if (System.IO.File.Exists(directoryPath))
			{
				files = new string[] { Path.GetFullPath(directoryPath) };
				directories = null;
			}
			else
			{
				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(wildcard))
					files = Directory.GetFiles(directoryPath);
				else
					files = Directory.GetFiles(directoryPath, wildcard);

				if (SolderFascadeAccessor.DataTypeFascade.IsNullOrWhiteSpace(wildcard))
					directories = Directory.GetDirectories(directoryPath);
				else
					directories = Directory.GetDirectories(directoryPath, wildcard);
			}

			if ((object)files != null)
			{
				items = new List<dynamic>();
				
				foreach (string file in files)
				{
					FileInfo fileInfo;

					fileInfo = new FileInfo(file);

					current = new Dictionary<string, object>();
					items.Add(current);

					current.FileFullName = fileInfo.FullName;
					current.FileFullNameRelativeToSource = EvaluateRelativePath2(sourcePath, fileInfo.FullName);
					current.FileCreationTime = fileInfo.CreationTime;
					current.FileCreationTimeUtc = fileInfo.CreationTimeUtc;
					current.FileExtension = fileInfo.Extension;
					current.FileIsReadOnly = fileInfo.IsReadOnly;
					current.FileLastAccessTime = fileInfo.LastAccessTime;
					current.FileLastAccessTimeUtc = fileInfo.LastAccessTimeUtc;
					current.FileLastWriteTime = fileInfo.LastWriteTime;
					current.FileLastWriteTimeUtc = fileInfo.LastWriteTimeUtc;
					current.FileLength = fileInfo.Length;
					current.FileName = fileInfo.Name;
				}

				parent.Files = items;
			}

			if ((object)directories != null)
			{
				items = new List<dynamic>();
				
				foreach (string directory in directories)
				{
					DirectoryInfo directoryInfo;

					directoryInfo = new DirectoryInfo(directory);

					current = new Dictionary<string, object>();
					items.Add(current);

					current.DirectoryFullName = directoryInfo.FullName;
					current.DirectoryFullNameRelativeToSource = EvaluateRelativePath2(sourcePath, directoryInfo.FullName);
					current.DirectoryAttributes = directoryInfo.Attributes;
					current.DirectoryCreationTime = directoryInfo.CreationTime;
					current.DirectoryCreationTimeUtc = directoryInfo.CreationTimeUtc;
					current.DirectoryExtension = directoryInfo.Extension;
					current.DirectoryLastAccessTime = directoryInfo.LastAccessTime;
					current.DirectoryLastAccessTimeUtc = directoryInfo.LastAccessTimeUtc;
					current.DirectoryLastWriteTime = directoryInfo.LastWriteTime;
					current.DirectoryLastWriteTimeUtc = directoryInfo.LastWriteTimeUtc;
					current.DirectoryName = directoryInfo.Name;

					if (recursive)
						EnumerateFileSystem(directory, recursive, wildcard, current, sourcePath);
				}
				
				parent.Directories = items;
			}
		}

		private static string EvaluateRelativePath(string mainDirPath, string absoluteFilePath)
		{
			string[] firstPathParts = mainDirPath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);
			string[] secondPathParts = absoluteFilePath.Trim(Path.DirectorySeparatorChar).Split(Path.DirectorySeparatorChar);

			int sameCounter = 0;
			for (int i = 0; i < Math.Min(firstPathParts.Length, secondPathParts.Length); i++)
			{
				if (!firstPathParts[i].ToLower().Equals(secondPathParts[i].ToLower()))
					break;

				sameCounter++;
			}

			if (sameCounter == 0)
				return absoluteFilePath;

			string newPath = string.Empty;
			for (int i = sameCounter; i < firstPathParts.Length; i++)
			{
				if (i > sameCounter)
					newPath += Path.DirectorySeparatorChar;

				newPath += "..";
			}

			if (newPath.Length == 0)
				newPath = ".";

			for (int i = sameCounter; i < secondPathParts.Length; i++)
			{
				newPath += Path.DirectorySeparatorChar;
				newPath += secondPathParts[i];
			}

			return newPath;
		}

		private static string EvaluateRelativePath2(string mainDirPath, string absoluteFilePath)
		{
			string p = EvaluateRelativePath(mainDirPath, absoluteFilePath);

			if (p.StartsWith(".\\"))
				p = p.Substring(2);

			return p;
		}

		protected override object CoreGetModelObject(IDictionary<string, object> properties)
		{
			const string PROP_TOKEN_SOURCE_DIRECTORY_PATH = "SourceDirectoryPath";
			const string PROP_TOKEN_RECURSIVE = "Recursive";
			const string PROP_TOKEN_WILDCARD = "Wildcard";
			string sourceDirectoryPath = null;
			dynamic sourceObject;
			object value;
			bool recursive = false;
			string recursiveStr;
			string wildcard;

			if ((object)properties == null)
				throw new ArgumentNullException(nameof(properties));
			
			if (properties.TryGetValue(PROP_TOKEN_SOURCE_DIRECTORY_PATH, out value))
			{
				if ((object)value != null)
					sourceDirectoryPath = (string)value;
			}

			if (SolderFascadeAccessor.DataTypeFascade.IsWhiteSpace(sourceDirectoryPath))
				throw new InvalidOperationException(String.Format("The source directory path cannot be null or whitespace."));

			sourceDirectoryPath = Path.GetFullPath(sourceDirectoryPath);
			
			if (System.IO.File.Exists(sourceDirectoryPath))
				throw new InvalidOperationException(String.Format("The source directory path cannot be a file."));
			
			if (!Directory.Exists(sourceDirectoryPath))
				throw new InvalidOperationException(String.Format("The source directory path does not exist."));

			recursiveStr = null;
			if (properties.TryGetValue(PROP_TOKEN_RECURSIVE, out value))
			{
				if ((object)value != null)
				{
					recursiveStr = (string)value;
					if (!SolderFascadeAccessor.DataTypeFascade.TryParse<bool>(recursiveStr, out recursive))
					{
						// do nothing
					}
				}
			}

			wildcard = null;
			if (properties.TryGetValue(PROP_TOKEN_WILDCARD, out value))
			{
				if ((object)value != null)
					wildcard = (string)value;
			}

			sourceObject = new Dictionary<string, object>();

			sourceObject.SourceFullPath = sourceDirectoryPath;
			sourceObject.Recursive = recursive;
			sourceObject.Wildcard = wildcard;

			EnumerateFileSystem(sourceDirectoryPath, recursive, wildcard, sourceObject, sourceDirectoryPath);

			return sourceObject;
		}

		protected async override ValueTask<object> CoreGetModelObjectAsync(IDictionary<string, object> properties)
		{
			await Task.CompletedTask;
			return this.CoreGetModelObject(properties);
		}

		#endregion
	}
}