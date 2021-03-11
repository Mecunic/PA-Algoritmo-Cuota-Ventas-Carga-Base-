


-- =============================================
-- Author:		<IMSC>
-- Create date: <28-05-2012>
-- Description:	<Description, , Obtiene las horas y los minutos y regresa un datatime >
-- =============================================
CREATE FUNCTION [dbo].[ObtieneMinSegTO]
(
	@MinutosSegundos VarChar(10)
	
)

RETURNS Datetime

AS

BEGIN
	-- Declare the return variable here

	DECLARE @ResultVar Datetime
    DECLARE @MIN INT
    DECLARE @SEG INT
 
    SET @MIN = CONVERT(INT,SUBSTRING(@MinutosSegundos, 0, CHARINDEX('''',@MinutosSegundos)   ) )
    SET @SEG = SUBSTRING(@MinutosSegundos, CHARINDEX('''',@MinutosSegundos) + 1, CHARINDEX('''',@MinutosSegundos) -1  ) 

    IF @MIN > 59
        SET @MIN = 59

    IF @SEG > 59
        SET @SEG = 59
   
	SET @ResultVar = CONVERT(DATETIME, 
                '0:' 
           +  CONVERT(VARCHAR(2),  @MIN)
           + ':' 
           + CONVERT(VARCHAR(2),  @SEG) )
  

	RETURN @ResultVar

END



