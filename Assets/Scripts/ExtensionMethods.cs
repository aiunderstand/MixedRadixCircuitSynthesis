using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

    namespace ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool isValidHeptCode(this String hepCode)
        {
            string alphabet = "0123456789ABCDEFGHKMNPRTVXZ";
            for (int i = 0; i < hepCode.Length; i++)
            {
                var symbol = hepCode[i];
                bool isValid = false;

                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (symbol.Equals(alphabet[j]))
                        isValid = true;
                }

                if (!isValid) //if no match has been found the symbol is not part of the alphabet and thus not a hepcode
                    return false;
            }

            return true;
        }
    }

    //https://ivanderevianko.com/2015/05/unity3d-waitforframes-in-coroutine
    public static class WaitFor
    {
        public static IEnumerator Frames(int frameCount)
        {
            if (frameCount <= 0)
            {
                throw new ArgumentOutOfRangeException("frameCount", "Cannot wait for less that 1 frame");
            }

            while (frameCount > 0)
            {
                frameCount--;
                yield return null;
            }
        }
    }

    public static class FileHelpers
    {
        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
