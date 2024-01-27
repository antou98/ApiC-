namespace WebApi.Models
{
    public class GraphFile
    {
        public GraphFile(byte[] data, string name, string format)
        {
            this.data = data;
            Name = name;
            this.format = format;
        }

        public byte[] data { get; set; }
        public string Name { get; set; }
        public string format { get; set; }


    }
}
