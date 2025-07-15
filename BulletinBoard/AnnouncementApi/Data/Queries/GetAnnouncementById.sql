SELECT a.Id,
       a.Title,
       a.Description,
       a.CreatedDate,
       a.Status,
       a.SubcategoryId
FROM Announcements a
WHERE a.Id = @Id;