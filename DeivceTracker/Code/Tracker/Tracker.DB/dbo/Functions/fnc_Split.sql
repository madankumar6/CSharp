	CREATE
	FUNCTION 
	[dbo].[fnc_Split]( @Data VARCHAR(2000),@Sep VARCHAR(5))
	  RETURNS @Temp TABLE (	Id INT IDENTITY(1,1),Data NVARCHAR(1000)) 
	  AS  
	  BEGIN 
		  DECLARE @Cnt INT	SET @Cnt = 1	
		  WHILE (CHARINDEX(@Sep,@Data)>0)	
			BEGIN		
				  INSERT INTO @Temp(data)		
				  SELECT Data = LTRIM(RTRIM(SUBSTRING(@Data,1,CHARINDEX(@Sep,@Data)-1)))		
				  SET @Data = SUBSTRING(@Data,CHARINDEX(@Sep,@Data)+1,len(@Data))		
				  SET @Cnt = @Cnt + 1	
			END		
		  INSERT INTO @Temp (data)	
		  SELECT Data = LTRIM(RTRIM(@Data))	
  RETURN
END