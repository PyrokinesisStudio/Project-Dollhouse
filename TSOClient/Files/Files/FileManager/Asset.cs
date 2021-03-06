﻿/*This Source Code Form is subject to the terms of the Mozilla Public License, v. 2.0.
If a copy of the MPL was not distributed with this file, You can obtain one at
http://mozilla.org/MPL/2.0/.

The Original Code is the Files.

The Initial Developer of the Original Code is
Mats 'Afr0' Vederhus. All Rights Reserved.

Contributor(s):
*/

using System;
using System.Threading;
using System.Reflection;
using log4net;

namespace Files.Manager
{
    /// <summary>
    /// An in-memory asset. Loaded and managed by FileManager.
    /// </summary>
    public class Asset : IDisposable
    {
        private DateTime m_LastAccessed = DateTime.Now;
        private ManualResetEvent m_LastAccessedLock = new ManualResetEvent(false);
        private uint m_Size;
        private ulong m_AssetID = 0;
        private byte[] m_AssetFilename;
        private object m_AssetData;

        private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DateTime LastAccessed
        {
            get
            {
                m_LastAccessedLock.WaitOne();
                return m_LastAccessed;
            }
        }

        /// <summary>
        /// Get the ID of this asset. May be null.
        /// </summary>
        public ulong AssetID
        {
            get { return m_AssetID; }
        }

        /// <summary>
        /// Get the hash of this asset's filename. May be null.
        /// </summary>
        public byte[] FilenameHash
        {
            get { return m_AssetFilename; }
        }

        /// <summary>
        /// Gets the size of this asset in bytes.
        /// </summary>
        public uint Size
        {
            get { return m_Size; }
        }

        /// <summary>
        /// Gets the data of this asset.
        /// </summary>
        public object AssetData
        {
            get
            {
                m_LastAccessedLock.Reset();
                m_LastAccessed = DateTime.Now;
                m_LastAccessedLock.Set();

                return m_AssetData;
            }
        }

        /// <summary>
        /// Creates a new Asset instance.
        /// </summary>
        /// <param name="AssetID">ID of this asset.</param>
        /// <param name="Size">The size of this asset.</param>
        /// <param name="Data">Data of asset.</param>
        public Asset(ulong AssetID, uint Size, object Data)
        {
            m_AssetID = AssetID;
            m_AssetData = Data;
            m_Size = Size;

            m_LastAccessedLock.Reset();
            m_LastAccessed = DateTime.Now;
            m_LastAccessedLock.Set();
        }

        /// <summary>
        /// Creates a new Asset instance.
        /// </summary>
        /// <param name="AssetID">Hash of this asset's filename.</param>
        /// <param name="Data">Data of asset.</param>
        public Asset(byte[] AssetID, uint Size, object Data)
        {
            m_AssetFilename = AssetID;
            m_AssetData = Data;
            m_Size = Size;

            m_LastAccessedLock.Reset();
            m_LastAccessed = DateTime.Now;
            m_LastAccessedLock.Set();
        }

        ~Asset()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes of the resources used by this Asset instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes of the resources used by this Asset instance.
        /// <param name="Disposed">Was this resource disposed explicitly?</param>
        /// </summary>
        protected virtual void Dispose(bool Disposed)
        {
            if (Disposed)
            {
                m_LastAccessedLock.Dispose();
                // Prevent the finalizer from calling ~Asset, since the object is already disposed at this point.
                GC.SuppressFinalize(this);
            }
            else
                m_Logger.Error("Asset not explicitly disposed!");
        }
    }
}
