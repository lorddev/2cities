using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TwoCities.Tests
{
    #pragma warning disable S3881 // "IDisposable" is already implemented correctly
    public class FileReader : IDisposable
    {
        private readonly StreamReader _fileStream;

        public Action<string> OnLineRead { get; set; } = s => { };

        public FileReader(string path)
        {
            _fileStream = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
        }


        public async Task Stream()
        {
            string line;
            while ((line = _fileStream.ReadLine()) != null)
            { 
                await Task.Delay(20);
                foreach (var evt in OnLineRead.GetInvocationList())
                {
                    await Task.Run(() =>
                    {
                        try
                        {
                            evt.DynamicInvoke(line);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }

                    });
                }

                Console.WriteLine("Batch complete...");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _fileStream.Dispose();
            }
        }
    }
}