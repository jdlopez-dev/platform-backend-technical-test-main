namespace AcmeCorporation.Services.Helpers
{
    public static class RegularExpression
    {
        public const string DNI_REGEX = @"^(\d{8})([A-Z])$";
        public const string NIE_REGEX = @"^[XYZ]\d{7,8}[A-Z]$";
        public const string NIF_REGEX = @"^([ABCDEFGHJKLMNPQRSUVW])(\d{7})([0-9A-J])$";
    }
}