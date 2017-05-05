using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace TwoCities.Tests
{
    public class FileStreamLineTests
    {
        private readonly ITestOutputHelper _output;

        public FileStreamLineTests(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>
        /// Tests that the reader splits by lines and performs a function at each interval.
        /// </summary>
        /// <remarks>Basically what it's doing is reading the file in small batches. When it encounters a line
        /// break, it makes a note of it, puts the extra text in a "buffer" (not to be confused with an actual stream
        /// buffer) </remarks>
        /// <returns></returns>
        [Fact]
        public async Task TestFileStreamSegmented()
        {
            var list = new List<string>();
            using (var fs = new FileReader(AppContext.BaseDirectory + @"\lipsum.log"))
            {
                // put stuff between line breaks right here.
                fs.OnLineRead += s =>
                {
                    var stringToAdd = s.Trim();
                    list.Add(stringToAdd);
                    _output.WriteLine($"Added {stringToAdd}");
                };

                await fs.Stream();
            }

            _output.WriteLine($"List with {list.Count} entries.");
            Assert.Equal("malesuada.", list.Last());
            Assert.Equal(19, list.Count);
        }
    }
}