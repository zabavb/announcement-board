SELECT
    a.Id, a.Title, a.Description, a.CreatedDate, a.Status,
    c.Id AS CategoryId, c.Name AS CategoryName,
    sc.Id AS SubcategoryId, sc.Name AS SubcategoryName
FROM Announcements a
         LEFT JOIN Categories c ON a.CategoryId = c.Id
         LEFT JOIN Categories sc ON a.SubcategoryId = sc.Id
WHERE (@SubcategoryId IS NULL OR sc.Id = @SubcategoryId)
ORDER BY a.CreatedDate DESC;
