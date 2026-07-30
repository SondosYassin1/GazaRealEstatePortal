USE GazaRealEstateDb;
GO

-- 1. Delete existing properties to start fresh
DELETE FROM PropertyImages;
DELETE FROM PropertyReviews;
DELETE FROM Properties;

DBCC CHECKIDENT ('PropertyImages', RESEED, 0);
DBCC CHECKIDENT ('PropertyReviews', RESEED, 0);
DBCC CHECKIDENT ('Properties', RESEED, 0);

DECLARE @UserId INT = 1; -- Admin
DECLARE @Now DATETIME2 = SYSDATETIME();

-- Insert 6 Properties
INSERT INTO Properties (UserId, Title, Description, Price, OperationType, PropertyType, Governorate, CityAreaCamp, DetailedAddress, Area, Rooms, Bathrooms, Floor, Features, ContactPhone, WhatsAppNumber, Status, CreatedAt, UpdatedAt)
VALUES 
(@UserId, N'شقة سكنية فاخرة', N'شقة سكنية تشطيب سوبر ديلوكس في الرمال.', 120000, 'Sale', 'Apartment', N'غزة', N'الرمال', N'شارع عمر المختار', 150, 3, 2, 2, N'موقف سيارة, مصعد', N'0593617699', N'00970593617699', 'Approved', @Now, @Now),

(@UserId, N'فيلا حديثة التصميم', N'فيلا حديثة في خانيونس البلد بمساحة ممتازة.', 400, 'Rent', 'Villa', N'خانيونس', N'البلد', N'وسط البلد', 300, 5, 3, 0, N'حديقة, كراج', N'0593617699', N'00970593617699', 'Approved', @Now, @Now),

(@UserId, N'أرض زراعية', N'أرض زراعية خصبة في بيت لاهيا.', 50000, 'Sale', 'Land', N'الشمال', N'بيت لاهيا', N'منطقة العطاطرة', 1000, 0, 0, 0, N'بئر ماء', N'0593617699', N'00970593617699', 'Approved', @Now, @Now),

(@UserId, N'مكتب تجاري مجهز', N'مكتب تجاري جاهز للعمل في النصر.', 1200, 'Rent', 'Office', N'غزة', N'النصر', N'شارع النصر', 85, 2, 1, 1, N'تكييف, إنترنت', N'0593617699', N'00970593617699', 'Approved', @Now, @Now),

(@UserId, N'منزل مستقل', N'منزل مستقل وواسع في تل السلطان.', 85000, 'Sale', 'House', N'رفح', N'تل السلطان', N'غرب تل السلطان', 200, 4, 2, 1, N'حوش, تهوية ممتازة', N'0593617699', N'00970593617699', 'Approved', @Now, @Now),

(@UserId, N'شقة عائلية', N'شقة عائلية مريحة في النصيرات.', 60000, 'Sale', 'Apartment', N'الوسطى', N'النصيرات', N'المخيم الجديد', 130, 3, 1, 3, N'قريبة من الخدمات', N'0593617699', N'00970593617699', 'Approved', @Now, @Now);

-- Insert images for these properties
INSERT INTO PropertyImages (PropertyId, ImageUrl, UploadedAt)
VALUES 
(1, N'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now),
(2, N'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now),
(3, N'https://images.unsplash.com/photo-1564013799919-ab600027ffc6?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now),
(4, N'https://images.unsplash.com/photo-1497366216548-37526070297c?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now),
(5, N'https://images.unsplash.com/photo-1600607687920-4e2a09cf159d?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now),
(6, N'https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=800&q=80', @Now);
GO
