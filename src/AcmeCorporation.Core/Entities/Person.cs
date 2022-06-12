namespace AcmeCorporation.Core.Entities
{
    public enum DocumentType
    {
        UNDEFINED = 0,
        NIF = 1,
        NIE = 2,
        DNI = 3,
    }

    public class Person
    {
        public int Age { get; set; }
        public string Document { get; set; }
        public DocumentType DocumentType { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}