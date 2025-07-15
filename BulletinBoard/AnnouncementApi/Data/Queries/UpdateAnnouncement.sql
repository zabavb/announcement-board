UPDATE Announcements
SET Title         = @Title,
    Description   = @Description,
    Status        = @Status,
    SubcategoryId = @SubcategoryId
WHERE Id = @Id;
