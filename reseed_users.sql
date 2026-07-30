USE GazaRealEstateDb;
GO

BEGIN TRANSACTION;

BEGIN TRY
    -- 1. احفظ بيانات Sondos الحالية بمتغيرات مؤقتة
    DECLARE @OldSondosId INT = 1002;
    DECLARE @FullName NVARCHAR(100), @Email NVARCHAR(150),
            @PasswordHash NVARCHAR(MAX), @PhoneNumber NVARCHAR(20),
            @Role NVARCHAR(20), @CreatedAt DATETIME2, @IsActive BIT,
            @ExternalProvider NVARCHAR(50), @ExternalProviderId NVARCHAR(100);

    SELECT @FullName = FullName, @Email = Email,
           @PasswordHash = PasswordHash, @PhoneNumber = PhoneNumber,
           @Role = Role, @CreatedAt = CreatedAt, @IsActive = IsActive,
           @ExternalProvider = ExternalProvider,
           @ExternalProviderId = ExternalProviderId
    FROM Users WHERE Id = @OldSondosId;

    -- 2. فكّ ربط عقاراتها مؤقتاً (نعطيها UserId مؤقت = الأدمن لحظياً بس)
    UPDATE Properties SET UserId = 1 WHERE UserId = @OldSondosId;

    -- 3. احذف صفها القديم برقم 1002
    DELETE FROM Users WHERE Id = @OldSondosId;

    -- 4. صفّر العداد ليبدأ صح بعد الأدمن (#1)
    DBCC CHECKIDENT ('Users', RESEED, 1);

    -- 5. أعد إدخالها من جديد - هلق رح تاخذ تلقائياً الرقم 2
    INSERT INTO Users (FullName, Email, PasswordHash, PhoneNumber, Role,
                        CreatedAt, IsActive, ExternalProvider, ExternalProviderId)
    VALUES (@FullName, @Email, @PasswordHash, @PhoneNumber, @Role,
            @CreatedAt, @IsActive, @ExternalProvider, @ExternalProviderId);

    DECLARE @NewSondosId INT = SCOPE_IDENTITY();

    -- 6. اربط عقاراتها الستة بالرقم الجديد الصحيح
    UPDATE Properties SET UserId = @NewSondosId
    WHERE ContactPhone = '0593617699' AND UserId = 1;

    -- تحقق قبل الـ Commit: اعرض لي النتيجة هون قبل ما تكمل
    SELECT Id, FullName, Email FROM Users ORDER BY Id;
    SELECT Id, Title, UserId FROM Properties;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    SELECT ERROR_MESSAGE() AS ErrorMessage;
END CATCH;
GO
