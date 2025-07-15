SELECT a.Id,
       a.Title,
       a.Description,
       a.CreatedDate,
       a.Status,
       a.SubcategoryId
FROM Announcements a
         JOIN Subcategories s ON a.SubcategoryId = s.Id
         JOIN Categories c ON s.ParentId = c.Id
WHERE (@SubcategoryId IS NULL OR a.SubcategoryId = @SubcategoryId)
  AND (@CategoryId IS NULL OR c.Id = @CategoryId);
