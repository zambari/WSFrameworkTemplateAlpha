using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Z;
public struct zOSCMessage
{
	[SerializeField]
	public string _address;
	public string address
	{
		get { if (string.IsNullOrEmpty(_address)) return "/none"; return _address; }
		set { _address = value; }
	}
	public enum ParseStatus { raw, adressKnown, typesKnown, offsetsKnown }
	// public int currentRreadIndex;
	public ParseStatus parseStatus;
	public List<object> objects;
	public List<System.Type> types;
	public byte[] data;
	public long timeStamp;
	public int typetagOffset;
	public int payloadOffset;
	[SerializeField] List<string> _addressSegments;
	public List<string> addressSegments { get { if (_addressSegments == null) _addressSegments = new List<string>(); return _addressSegments; } }
	public int[] payloadOffsets;

	public zOSCMessage(string address)
	{
		_address = address;
		timeStamp = DateTime.UtcNow.Ticks;
		parseStatus = ParseStatus.adressKnown;
		_addressSegments = null; // new List<string>(address.Split('/'));
		types = new List<System.Type>();
		payloadOffsets = null;
		objects = null;
		data = null;
		typetagOffset = 0;
		payloadOffset = 0;
	}
	static System.Text.StringBuilder stringBuilder;
	public zOSCMessage(byte[] sourceBuff, int startindex = 0)
	{
		UnityEngine.Debug.Log("Datratlen " + sourceBuff.Length);
		data = sourceBuff;
		timeStamp = DateTime.UtcNow.Ticks;
		objects = null;
		parseStatus = ParseStatus.raw;
		payloadOffsets = null;
		typetagOffset = 0;
		payloadOffset = 0;
		_addressSegments = null; // new List<string>();
		UnityEngine.Debug.Log("soruce " + data.ByteArrayToStringAsHex());
		OSCWordReader reader = new OSCWordReader();
		_address = data.UnpackString(reader);
		types = null;
		// UnityEngine.Debug.Log("found address " + _address + " pos is now " + reader.readIndex);

		UnityEngine.Debug.Log("after alignment " + reader.readIndex);
		typetagOffset = reader.readIndex;
		//
		types = this.UnpackTypeTag(reader);
		//	reader.Align(data);
		// payloadOffset = reader.readIndex;
		UnityEngine.Debug.Log("found type count = " + typetagOffset + "  payloadOffset= " + payloadOffset + " types.count=" + types.Count);
		// //
		// //FindAddressesAndTypeTagIndex();
	}

	void FindAddressesAndTypeTagIndex(byte[] sourceBuff)
	{
		//  stringBuilder=new System.Text.StringBuilder();
		// lock(stringBuilder)
		// {
		// 	stringBuilder.Append

		// }
	}

}