﻿namespace WinUICommunity;

public partial class InIHelper
{
    internal string Path { get; set; }
    public InIHelper(string filePath)
    {
        Path = filePath;
    }

    private string GetProductName()
    {
        return ProcessInfoHelper.ProductName;
    }

    /// <summary>
    /// Read Data Value From the Ini File
    /// </summary>
    /// <param name="Key">must be unique</param>
    /// <param name="Section">Optional</param>
    /// <param name="Path">default is: application startup folder location</param>
    /// <returns></returns>
    public string ReadValue(string Key, string Section = null, string Path = null)
    {
        const uint MAX_Length = 255;
        Span<char> buffer = stackalloc char[(int)MAX_Length];

        unsafe
        {
            fixed (char* pBuffer = buffer)
            {
                uint result = PInvoke.GetPrivateProfileString(Section ?? GetProductName(), Key, "", new PWSTR(pBuffer), MAX_Length, Path ?? this.Path);

                if (result == 0)
                {
                    throw new Win32Exception();
                }

                return new string(pBuffer, 0, (int)result);
            }
        }
    }

    /// <summary>
    /// Write Data to the INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Value"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path">default is: application startup folder location</param>
    public void AddValue(string Key, string Value, string Section = null, string Path = null)
    {
        PInvoke.WritePrivateProfileString(Section ?? GetProductName(), Key, Value, Path ?? this.Path);
    }

    /// <summary>
    /// Delete Key from INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path"></param>
    public void DeleteKey(string Key, string Section = null, string Path = null)
    {
        AddValue(Key, null, Section ?? GetProductName(), Path ?? this.Path);
    }

    /// <summary>
    /// Delete Section from INI File
    /// </summary>
    /// <param name="Section"></param>
    /// <param name="Path"></param>
    public void DeleteSection(string Section = null, string Path = null)
    {
        AddValue(null, null, Section ?? GetProductName(), Path ?? this.Path);
    }

    /// <summary>
    /// Check if Key Exist or Not in INI File
    /// </summary>
    /// <param name="Key"></param>
    /// <param name="Section">Optional</param>
    /// <param name="Path"></param>
    /// <returns></returns>
    public bool IsKeyExists(string Key, string Section = null, string Path = null)
    {
        return ReadValue(Key, Section, Path ?? this.Path).Length > 0;
    }
}
