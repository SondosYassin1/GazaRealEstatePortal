using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this PropertyType propertyType)
        {
            return propertyType switch
            {
                PropertyType.Apartment => "شقة",
                PropertyType.House => "منزل",
                PropertyType.Land => "أرض",
                PropertyType.Store => "محل تجاري",
                PropertyType.Villa => "فيلا",
                PropertyType.Office => "مكتب",
                _ => propertyType.ToString()
            };
        }
    }
}
