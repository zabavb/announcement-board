UPDATE Announcements
SET Title         = @Title,
    Description   = @Description,
    Status        = @Status,
    CategoryId    = @CategoryId,
    SubcategoryId = @SubcategoryId
WHERE Id = @Id;
