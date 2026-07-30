USE GazaRealEstateDb;
GO

-- 1. Clean up existing test users with that email if any
DELETE FROM Users WHERE Email = 'sondosalaa687@gmail.com';
GO

-- 2. Insert new user
INSERT INTO Users (FullName, Email, PhoneNumber, Role, IsActive, PasswordHash, CreatedAt)
VALUES ('Sondos Alaa Yassin', 'sondosalaa687@gmail.com', '0593617699', 'RegisteredUser', 1, '$2a$11$EbXyQBCIEPmReCYun1S4e.E7hs9PNHy1stk841TCweJ4hCK9g37mG', GETDATE());
GO

-- Get the inserted ID
DECLARE @NewUserId INT;
SELECT @NewUserId = Id FROM Users WHERE Email = 'sondosalaa687@gmail.com';

-- 3. Update properties
UPDATE Properties
SET UserId = @NewUserId,
    ContactPhone = '0593617699',
    WhatsAppNumber = '00970593617699'
WHERE UserId = 1;
GO

-- 4. Verify properties
SELECT p.Id, p.Title, p.UserId, u.FullName, u.Email
FROM Properties p 
JOIN Users u ON p.UserId = u.Id;
GO
