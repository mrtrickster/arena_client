using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using UnityEngine;

public class StringConverter
{
    public static string EncodeToHex(string str)
    {
        byte[] ba = Encoding.Default.GetBytes(str);
        var hexString = BitConverter.ToString(ba);
        hexString = hexString.Replace("-", "");
        return "0x" + hexString;
    }
    public static string DecodeFeltHex(string felt)
    {
        return (DecodeFelt(DecodeHex(felt)));
    }

    public static string DecodeFelt(string felt)
    {
        //Debug.Log("decoding felt: " + felt);

        felt = felt.Replace("0x", "");
        string result = "";
        BigInteger feltNumber = BigInteger.Parse(felt);
        while (feltNumber > 0)
        {
            BigInteger charValue = feltNumber % 256;
            result = (char)(int)charValue + result;
            feltNumber /= 256;
        }
        return result;
    }

    public static string DecodeHex(string hex)
    {
        //Debug.Log("decoding hex: " + hex);
        hex = hex.Substring(2);
        return BigInteger.Parse(hex, NumberStyles.AllowHexSpecifier).ToString("d");
    }

    public static BigInteger TextToFelt(string text)
    {
        // Convert text to UTF-8 byte array
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        // Convert the byte array to a BigInteger
        BigInteger felt = new BigInteger(bytes, isUnsigned: true, isBigEndian: true);

        // Apply 252-bit limit: 2^252 - 1
        BigInteger maxFeltValue = BigInteger.One << 252 - 1;
        if (felt >= maxFeltValue)
        {
            felt %= maxFeltValue;
        }

        return felt;
    }


    public static string StringToFelt(string input)
    {
        BigInteger feltNumber = 0;
        BigInteger baseMultiplier = 1;
        BigInteger maxFeltValue = BigInteger.Pow(2, 251) - 1;

        var inputArr = input.Split("");
        Array.Reverse(inputArr);
        input = string.Join("", inputArr);

        foreach (char c in input)
        {
            // Convert each character to its ASCII value and add it to the BigInteger
            BigInteger charValue = (int)c;
            feltNumber += charValue * baseMultiplier;

            // Increase the base multiplier for the next character
            baseMultiplier *= 256;

            // Check if feltNumber exceeds the max felt value
            if (feltNumber > maxFeltValue)
            {
                throw new ArgumentOutOfRangeException("The resulting number is out of range for a StarkNet 'felt'.");
            }
        }

        return feltNumber.ToString();
    }
}
