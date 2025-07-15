SELECT u.Id,
       u.FullName,
       u.Email,
       u.Password
FROM Users u
WHERE (u.Email = @Email);
