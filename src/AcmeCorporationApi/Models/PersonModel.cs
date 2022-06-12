namespace AcmeCorporationApi.Models
{
    public record PersonModel(int Id, string Name, int Age, string Document, string DocumentType);

    public record PersonSaveModel(string Name, int Age, string Document);
}