using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class XmlExtensions
	{
		public static System.Xml.Linq.XElement ElementRequired ( this System.Xml.Linq.XElement xParent, System.Xml.Linq.XName xName, Action<string> actNotFound )
		{
			System.Xml.Linq.XElement xChild = xParent.Element ( xName );

			if ( xChild == null )
				actNotFound ( xName.ToString ( ) );

			return xChild;
		}

		public static T GetValue<T> ( this System.Xml.Linq.XElement xElement, T Default, Func<string, T> fnConverter )
		{
			try
			{
				if ( xElement == null )
					return Default;
				else
					return fnConverter ( xElement.Value );
			}
			catch ( FormatException )
			{
				return Default;
			}
		}

		public static T GetEnumValue<T> ( this System.Xml.Linq.XElement xElement, T Default )
		{
			try
			{
				if ( xElement == null )
					return Default;
				else
					return (T)Enum.Parse ( typeof ( T ), xElement.GetValue ( ) );
			}
			catch ( FormatException )
			{
				return Default;
			}
		}

		public static T GetValue<T> ( this System.Xml.Linq.XAttribute xElement, T Default, Func<string, T> fnConverter )
		{
			try
			{
				if ( xElement == null )
					return Default;
				else
					return fnConverter ( xElement.Value );
			}
			catch ( FormatException )
			{
				return Default;
			}
		}

		public static T GetEnumValue<T> ( this System.Xml.Linq.XAttribute xElement, T Default )
		{
			try
			{
				if ( xElement == null )
					return Default;
				else
					return (T)Enum.Parse ( typeof ( T ), xElement.GetValue ( "", s => s ) );
			}
			catch ( FormatException )
			{
				return Default;
			}
		}

		public static T GetValue<T> ( this System.Xml.Linq.XElement xElement, Func<T> fnDefault, Func<string, T> fnConverter )
		{
			try
			{
				if ( xElement == null )
					return fnDefault ( );
				else
					return fnConverter ( xElement.Value );
			}
			catch ( FormatException )
			{
				return fnDefault ( );
			}
		}

		public static string GetValue ( this System.Xml.Linq.XElement xElement )
		{
			if ( xElement == null )
				return null;
			else
				return xElement.Value;
		}
	}
}
