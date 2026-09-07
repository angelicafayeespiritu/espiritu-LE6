-- Ensure a user with Id = 1 exists first
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Id = 1)
BEGIN
    INSERT INTO dbo.Users (UserName, FirstName, LastName, Password)
    VALUES ('afvespiritu', 'AFV', 'Espiritu', 'afvespiritu');
END

-- Insert Post with Id = 1 using the 'Content' column
SET IDENTITY_INSERT dbo.Posts ON;

INSERT INTO dbo.Posts (Id, Title, Body, Content, DateCreated, UserId)
VALUES (
    1, 
    'First Blog Post', 
    'This is the content for post 1.', 
    'This is the content for post 1.', 
    GETDATE(), 
    1
);

SET IDENTITY_INSERT dbo.Posts OFF;