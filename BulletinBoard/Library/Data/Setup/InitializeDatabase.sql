IF
NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AnnouncementsDB')
BEGIN
    CREATE DATABASE AnnouncementsDB;
END