using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilis.Extensions
{
	public static class StringExtensions
	{
		public static bool ContainsAny ( this string s, IEnumerable<string> aStrings, bool bCaseInsensitive )
		{
			if ( s == null || aStrings == null )
				return false;

			string sLower = s.ToLower ( );
			foreach ( string sSub in aStrings )
			{
				if ( sLower.Contains ( sSub.ToLower ( ), bCaseInsensitive ) )
					return true;
			}

			return false;
		}

		public static bool ContainsAll ( this string s, IEnumerable<string> aStrings )
		{
			if ( s == null || aStrings == null )
				return false;

			string sLower = s.ToLower ( );
			foreach ( string sSub in aStrings )
			{
				if ( !sLower.Contains ( sSub.ToLower ( ) ) )
					return false;
			}

			return true;
		}

		public static bool EndsWithAny ( this string s, IEnumerable<string> aStrings, bool bCaseInsensitive )
		{
			if ( s == null || aStrings == null )
				return false;

			string sLower = s.ToLower ( );
			return aStrings.Any (
				sSub => sLower.EndsWith (
					sSub.ToLower ( ),
					bCaseInsensitive ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture ) );
		}

		public static bool Contains ( this string s1, string s2, bool bCaseInsensitive )
		{
			if ( bCaseInsensitive )
				return System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf ( s1, s2, System.Globalization.CompareOptions.OrdinalIgnoreCase ) > -1;
			else
				return System.Globalization.CultureInfo.CurrentCulture.CompareInfo.IndexOf ( s1, s2, System.Globalization.CompareOptions.Ordinal ) > -1;
		}

		public static void TrimEnd ( this StringBuilder sb, char[ ] trimChars )
		{
			if ( trimChars == null || trimChars.Length == 0 || sb == null || sb.Length == 0 )
				return;

			while ( sb.Length > 0 && trimChars.Contains ( sb[ sb.Length - 1 ] ) )
				sb.Remove ( sb.Length - 1, 1 );
		}

		public static bool IsNullOrEmpty ( this string s )
		{
			return string.IsNullOrEmpty ( s );
		}
	}
}
