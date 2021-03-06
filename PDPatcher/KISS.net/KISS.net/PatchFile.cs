﻿/*This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at
http://mozilla.org/MPL/2.0/.

The Original Code is the KISS.Net.

The Initial Developer of the Original Code is
Afr0. All Rights Reserved.

Contributor(s): ______________________________________.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace KISS.net
{
    /// <summary>
    /// A patchfile is a file referenced by a manifest.
    /// </summary>
    public class PatchFile
    {
        public string FileHash;
        public string Address;
        public string URL;

        public static string CalculateHash(string Address)
        {
            BinaryReader Reader = new BinaryReader(File.Open(Address, FileMode.Open));
            byte[] FileData = ReadAllBytes(Reader);

            SHA256 Hash = SHA256Managed.Create();
            Hash.Initialize();
            return Convert.ToBase64String(Hash.ComputeHash(FileData));
        }

        private static byte[] ReadAllBytes(BinaryReader reader)
        {
            const int bufferSize = 4096;
            using (var ms = new MemoryStream())
            {
                byte[] buffer = new byte[bufferSize];
                int count;
                while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                    ms.Write(buffer, 0, count);
                return ms.ToArray();
            }
        }
    }
}
