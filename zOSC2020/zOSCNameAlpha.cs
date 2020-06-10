using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class zOSCNameAlpha
{

	static string charset
	{
		get
		{
			if (string.IsNullOrEmpty(_charset))
				_charset = "bcdefgahin"; // 10 letters for 10 digits
			// _charset = "bcdefgahin"; // 10 letters for 10 digits
			return _charset;
		}
		set { _charset = value; }
	}
	static string _charset;
	static char CharsetAt(int i)
	{
		if (i < 0) return 'x';
		i = i % charset.Length;
		return charset[i];
	}
	public static string PortToName(int port)
	{
		string nocase = PortToNameNoCase(port);
		return nocase;
	}
	public static string PortToNameNoCase(int port)
	{
		//if (port <0) return null;
		var thischar = CharsetAt(port).ToString();
		if (port % 2 == 0) thischar = thischar.ToUpper();
		if (port < charset.Length)
		{

			return thischar;
		}
		else
		{
			int digit = port % charset.Length;
			return PortToNameNoCase(port / charset.Length) + thischar;
		}
		//	port=port/10;

		//string s = 
		//string n=CharsetAt
	}

}