CREATE PROCEDURE [dbo].[Genera]
  @pdf AS image ,
  @name AS nvarchar 
AS
BEGIN
  -- outine body goes here, e.g.
INSERT INTO prueba_pdf (archivo, nombre) VALUES 
(@pdf, @name) 
  -- SELECT 'Navicat for SQL Server'
END