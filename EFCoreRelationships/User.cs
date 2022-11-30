using System.Reflection.PortableExecutable;

namespace EFCoreRelationships
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<Character> Character { get; set; }
    }
}
