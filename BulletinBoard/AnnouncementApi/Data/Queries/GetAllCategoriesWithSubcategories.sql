SELECT c.Id   AS CategoryId,
       c.Name AS CategoryName,
       s.Id   AS SubcategoryId,
       s.Name AS SubcategoryName
FROM Categories c
         LEFT JOIN Subcategories s ON s.ParentId = c.Id
ORDER BY c.Name, s.Name;
