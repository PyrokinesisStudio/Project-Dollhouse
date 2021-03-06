﻿/*This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at
http://mozilla.org/MPL/2.0/.

The Original Code is the Files library.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s):
*/

using System.Text;
using System.IO;

namespace Files.IFF
{
    /// <summary>
    /// This chunk type contains a filename to a semi-global IFF file, 
    /// without the file extension. Semi-global files define shared 
    /// common resources that are typically used in several other IFF files. 
    /// A GLOB chunk, when present, allows other chunks in the same file to 
    /// reference the semi-global file's resources.
    /// </summary>
    public class GLOB : IFFChunk
    {
        public string SemiGlobalIFF = "";

        public GLOB(IFFChunk BaseChunk) : base(BaseChunk)
        {
            FileReader Reader = new FileReader(new MemoryStream(m_Data), false);

            ASCIIEncoding Enc = new ASCIIEncoding();
            byte[] Data = Reader.ReadToEnd();
            SemiGlobalIFF = Enc.GetString(Data);

            Reader.Close();
            m_Data = null;
        }
    }
}