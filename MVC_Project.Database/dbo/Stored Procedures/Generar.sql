CREATE PROCEDURE [dbo].[Generar]
  @pdf AS image ,
  @id AS int ,
  @name AS nvarchar 
AS
BEGIN
  -- routine body goes here, e.g.
INSERT INTO prueba_pdf (archivo, id,nombre) VALUES 
(@pdf,@id, @name) 
  -- SELECT 'Navicat for SQL Server'
END