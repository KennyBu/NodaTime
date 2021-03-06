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
#region usings
using NUnit.Framework;
using NodaTime.Test.Text;

#endregion

namespace NodaTime.Test
{
    [TestFixture]
    public partial class OffsetTest
    {
        [Test, Category("Formatting"), Category("Format")]
        [TestCaseSource(typeof(OffsetFormattingTestSupport), "FormatWithoutFormat")]
        public void TestToString_Culture(OffsetFormattingTestSupport.OffsetData data)
        {
            FormattingTestSupport.RunFormatTest(data, () => data.V.ToString(data.C));
        }

        [Test, Category("Formatting"), Category("Format")]
        [TestCaseSource(typeof(OffsetFormattingTestSupport), "AllFormatData")]
        public void TestToString_Format(OffsetFormattingTestSupport.OffsetData data)
        {
            if (data.C != null)
            {
                data.ThreadCulture = data.C;
            }
            FormattingTestSupport.RunFormatTest(data, () => data.V.ToString(data.P));
        }

        [Test, Category("Formatting"), Category("Format")]
        [TestCaseSource(typeof(OffsetFormattingTestSupport), "AllFormatData")]
        public void TestToString_FormatCulture(OffsetFormattingTestSupport.OffsetData data)
        {
            FormattingTestSupport.RunFormatTest(data, () => data.V.ToString(data.P, data.C));
        }

        [Test, Category("Formatting"), Category("Format")]
        [TestCaseSource(typeof(OffsetFormattingTestSupport), "FormatWithoutFormat")]
        public void TestToString_NoArg(OffsetFormattingTestSupport.OffsetData data)
        {
            if (data.C != null)
            {
                data.ThreadCulture = data.C;
            }
            FormattingTestSupport.RunFormatTest(data, () => data.V.ToString());
        }
    }
}
