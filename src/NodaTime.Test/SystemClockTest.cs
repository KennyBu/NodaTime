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
using NUnit.Framework;
using NodaTime.Utility;

namespace NodaTime.Test
{
    [TestFixture]
    public class SystemClockTest
    {
        [Test]
        public void SystemNow()
        {
            long frameworkNowTicks = DateTime.UtcNow.Ticks - NodaConstants.DateTimeEpochTicks;
            long nodaTicks = SystemClock.SystemNow.Ticks;
            Assert.Less(Math.Abs(nodaTicks - frameworkNowTicks), Duration.FromSeconds(1).Ticks);
        }

        [Test]
        public void InstanceNow()
        {
            long frameworkNowTicks = DateTime.UtcNow.Ticks - NodaConstants.DateTimeEpochTicks;
            long nodaTicks = SystemClock.Instance.Now.Ticks;
            Assert.Less(Math.Abs(nodaTicks - frameworkNowTicks), Duration.FromSeconds(1).Ticks);
        }

        [Test]
        public void Sanity()
        {
            // Previously all the conversions missed the SystemConversions.DateTimeEpochTicks,
            // so they were self-consistent but not consistent with sanity.
            Instant minimumExpected = Instant.FromUtc(2011, 8, 1, 0, 0);
            Instant maximumExpected = Instant.FromUtc(2020, 1, 1, 0, 0);
            Instant now = SystemClock.SystemNow;
            Assert.Less(minimumExpected.Ticks, now.Ticks);
            Assert.Less(now.Ticks, maximumExpected.Ticks);
        }
    }
}
