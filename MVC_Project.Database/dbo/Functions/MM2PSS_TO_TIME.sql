


-- =============================================
-- Author:		<IMSC>
-- Create date: <28-05-2012>
-- Description:	<Description, , Obtiene las horas y los minutos y regresa un datatime >
-- =============================================
CREATE FUNCTION [dbo].[MM2PSS_TO_TIME]
(
	@MinutosSegundos VarChar(10)
	
)

RETURNS Datetime

AS

BEGIN
	-- Declare the return variable here

	DECLARE @ResultVar Datetime

	SET @ResultVar = CONVERT(DATETIME, 

'0:' 
           +  SUBSTRING(@MinutosSegundos, 0, CHARINDEX(':',@MinutosSegundos)   ) 
           + ':' 
           + SUBSTRING(@MinutosSegundos, CHARINDEX(':',@MinutosSegundos) + 1, LEN(@MinutosSegundos)   ) 
 ) 

	RETURN @ResultVar

END



