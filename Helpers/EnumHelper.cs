using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using GazaRealEstatePortal.Models.Enums;

namespace GazaRealEstatePortal.Helpers;

public static class EnumHelper
{
    public static List<SelectListItem> GetLocalizedEnumSelectList<T>() where T : struct, Enum
    {
        var list = new List<SelectListItem>();
        foreach (var item in Enum.GetValues<T>())
        {
            list.Add(new SelectListItem
            {
                Value = item.ToString(),
                Text = GetLocalizedName(item)
            });
        }
        return list;
    }

    public static string GetLocalizedName<T>(T item) where T : struct, Enum
    {
        if (typeof(T) == typeof(PropertyType))
        {
            return item.ToString() switch
            {
                "Apartment" => "شقة",
                "House" => "منزل",
                "Land" => "أرض",
                "Store" => "محل تجاري",
                "Villa" => "فيلا",
                "Office" => "مكتب",
                _ => item.ToString()
            };
        }
        if (typeof(T) == typeof(OperationType))
        {
            return item.ToString() switch
            {
                "Sale" => "بيع",
                "Rent" => "إيجار",
                "Exchange" => "استبدال",
                _ => item.ToString()
            };
        }
        return item.ToString();
    }

    public static string ToArabicText(this PropertyType type) => type switch
    {
        PropertyType.Apartment => "شقة",
        PropertyType.House => "منزل",
        PropertyType.Villa => "فيلا",
        PropertyType.Land => "أرض",
        PropertyType.Store => "محل تجاري",
        PropertyType.Office => "مكتب",
        _ => type.ToString()
    };

    public static string ToArabicText(this OperationType type) => type switch
    {
        OperationType.Sale => "للبيع",
        OperationType.Rent => "للإيجار",
        OperationType.Exchange => "للاستبدال",
        _ => type.ToString()
    };
}
