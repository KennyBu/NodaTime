﻿#region Copyright and license information
// Copyright 2001-2009 Stephen Colebourne
// Copyright 2009-2011 Jon Skeet
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using NodaTime.Utility;

namespace NodaTime.TimeZones
{
    /// <summary>
    /// A pair of possibly null ZoneInterval values. This is the result of fetching a time zone
    /// interval by LocalInstant, as the result could be 0, 1 or 2 matching ZoneIntervals.
    /// This is a sort of light-weight version of ZoneLocalMapping, used when we won't need to
    /// know the details of the gaps and it's handy to use a struct instead of creating a new object.
    /// </summary>
	[Serializable]
	internal struct ZoneIntervalPair : IEquatable<ZoneIntervalPair>
    {
        internal static readonly ZoneIntervalPair NoMatch = new ZoneIntervalPair(null, null, 0);
        
        private readonly ZoneInterval earlyInterval;
        private readonly ZoneInterval lateInterval;
        // Obviously this can be computed from how many of the intervals are non-null, assuming
        // we trust our callers... but computing it has a surprising impact on performance.
        private readonly int matchingIntervals;

        /// <summary>
        /// The earlier of the two zone intervals matching the original local instant, or null
        /// if there were no matches. If there is a single match (the most common case) this
        /// value will be non-null, and LateInterval will be null.
        /// </summary>
        internal ZoneInterval EarlyInterval { get { return earlyInterval; } }

        /// <summary>
        /// The later of the two zone intervals matching the original local instant, or null
        /// if there were no matches. If there is a single match (the most common case) this
        /// value will be null, and EarlyInterval will be non-null.
        /// </summary>
        internal ZoneInterval LateInterval { get { return lateInterval; } }

        private ZoneIntervalPair(ZoneInterval early, ZoneInterval late, int matchingIntervals)
        {
            // TODO: Validation, if we want it:
            // - If early is null, late must be null
            // - If both are specified, the end of early must equal the start of late
            this.earlyInterval = early;
            this.lateInterval = late;
            this.matchingIntervals = matchingIntervals;
        }

        internal static ZoneIntervalPair Unambiguous(ZoneInterval interval)
        {
            return new ZoneIntervalPair(interval, null, 1);
        }

        internal static ZoneIntervalPair Ambiguous(ZoneInterval early, ZoneInterval late)
        {
            return new ZoneIntervalPair(early, late, 2);
        }

        /// <summary>
        /// Returns the number of intervals contained within this pair - 0 for a "gap",
        /// 1 for an unambiguous match, 2 for an ambiguous match.
        /// </summary>
        internal int MatchingIntervals { get { return matchingIntervals; } }

        /// <summary>
        ///   Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="obj">The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" />.</param>
        /// <exception cref="T:System.NullReferenceException">The <paramref name = "obj" /> parameter is null.</exception>
        /// <filterpriority>2</filterpriority>
        [DebuggerStepThrough]
        public override bool Equals(object obj)
        {
            return obj is ZoneIntervalPair && Equals((ZoneIntervalPair)obj);
        }

        /// <summary>
        ///   Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///   true if the current object is equal to the <paramref name = "other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        /// </param>
        [DebuggerStepThrough]
        public bool Equals(ZoneIntervalPair other)
        {
            return EqualityComparer<ZoneInterval>.Default.Equals(earlyInterval, other.earlyInterval) &&
                   EqualityComparer<ZoneInterval>.Default.Equals(lateInterval, other.lateInterval);
        }

        /// <summary>
        ///   Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///   A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            int hash = HashCodeHelper.Initialize();
            hash = HashCodeHelper.Hash(hash, earlyInterval);
            hash = HashCodeHelper.Hash(hash, lateInterval);
            return hash;
        }

        public override string ToString()
        {
            switch (MatchingIntervals)
            {
                case 0:
                    return "No match (gap)";
                case 1:
                    return "Unambiguous: " + earlyInterval;
                case 2:
                    return "Ambiguous between " + earlyInterval + " and " + lateInterval;
                default:
                    throw new InvalidOperationException("Won't happen");
            }
        }
    }
}
