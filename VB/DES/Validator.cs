using System;
using System.Text.RegularExpressions;

namespace DES
{
	public enum RegexPresets{Email};

	/// <summary>
	/// Summary description for Validator.
	/// </summary>
	public abstract class Validator
	{
		public static bool IsRegexMatch(string sMatch, RegexPresets rp)
		{
			Regex re;

			switch(rp)
			{
				case RegexPresets.Email:
					re = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
					return re.IsMatch(sMatch);

				default:
					return false;
			}
		}
	}
}
