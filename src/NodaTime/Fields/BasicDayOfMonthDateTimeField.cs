#region Copyright and license information
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
using NodaTime.Calendars;

namespace NodaTime.Fields
{
    /// <summary>
    /// Provides time calculations for the day of the month component of time.
    /// </summary>
    // Porting status: Needs partial and max for set support.
	[Serializable]
	internal sealed class BasicDayOfMonthDateTimeField : PreciseDurationDateTimeField
    {
        private readonly BasicCalendarSystem calendarSystem;

        internal BasicDayOfMonthDateTimeField(BasicCalendarSystem calendarSystem, DurationField days) : base(DateTimeFieldType.DayOfMonth, days)
        {
            this.calendarSystem = calendarSystem;
        }

        internal override int GetValue(LocalInstant localInstant)
        {
            return calendarSystem.GetDayOfMonth(localInstant);
        }

        internal override long GetInt64Value(LocalInstant localInstant)
        {
            return calendarSystem.GetDayOfMonth(localInstant);
        }

        internal override DurationField RangeDurationField { get { return calendarSystem.Fields.Months; } }

        internal override long GetMaximumValue()
        {
            return calendarSystem.GetMaxDaysInMonth();
        }

        internal override long GetMaximumValue(LocalInstant localInstant)
        {
            return calendarSystem.GetMaxDaysInMonth(localInstant);
        }

        internal override long GetMinimumValue()
        {
            return 1;
        }
    }
}