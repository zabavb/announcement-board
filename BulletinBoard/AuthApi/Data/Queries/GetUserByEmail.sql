SELECT u.Id,
       u.FullName,
       u.Email,
       u.Pasword
FROM Users u
WHERE (u.Email = @Email);
