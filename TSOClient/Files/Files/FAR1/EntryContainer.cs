﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Files.FAR1
{
    /// <summary>
    /// A container for storing FAR1Entry instances and retrieving them.
    /// </summary>
    public class EntryContainer : List<FAR1Entry>
    {
        private List<FAR1Entry> m_Entries = new List<FAR1Entry>();

        public new void Add(FAR1Entry Entry)
        {
            m_Entries.Add(Entry);
        }

        /// <summary>
        /// Gets an entry from this EntryContainer instance.
        /// </summary>
        /// <param name="Filename">Filename of entry to get.</param>
        /// <returns>A FAR1Entry instance if found, null if not found.</returns>
        public FAR1Entry this[string Filename]
        {
            get
            {
                byte[] Hash = FileUtilities.GenerateHash(Filename);

                foreach (FAR1Entry Entry in m_Entries)
                {
                    if (Entry.FilenameHash.SequenceEqual(Hash))
                        return Entry;
                }

                return null;
            }
        }

        public new IEnumerator<FAR1Entry> GetEnumerator()
        {
            using (IEnumerator<FAR1Entry> ie = base.GetEnumerator())
            {
                while (ie.MoveNext())
                    yield return ie.Current;
            }
        }

        /// <summary>
        /// Does this EntryContainer instance contain the specified FAR1Entry instance?
        /// </summary>
        /// <param name="Filename">Filename of entry to look for.</param>
        /// <returns>True if found, false if not.</returns>
        public bool Contains(string Filename)
        {
            byte[] Hash = FileUtilities.GenerateHash(Filename);

            foreach (FAR1Entry Entry in m_Entries)
            {
                if (Entry.FilenameHash.SequenceEqual(Hash))
                    return true;
            }

            return false;
        }
    }
}