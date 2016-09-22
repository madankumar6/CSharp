CREATE PROCEDURE [dbo].[T_StoreNumOfDevicesConnected]
(
	@Protocol NVARCHAR(50) = NULL,
	@Port INT = NULL,
	@Count INT = NULL
)
AS
BEGIN

	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();
	
	IF EXISTS(SELECT 1 FROM ProtocolServer WHERE [ProtocolServer] = @Protocol AND Port = @Port)
	BEGIN
		UPDATE ProtocolServer
		SET
		   DevicesConnected = @Count
		   ,ModifiedDate= @ActionTime
		   ,ActionTime = @ActionTime
		WHERE
			[ProtocolServer] = @Protocol
		AND
			Port = @Port
	END
	ELSE
	BEGIN
		INSERT INTO ProtocolServer(
				[ProtocolServer]
			   ,Port
			   ,DevicesConnected
			   ,[CreatedDate]
			   ,[ModifiedDate]
			   ,ActionTime
			   )
		 VALUES
			   (
			   @Protocol
			   ,@Port
			   ,@Count
			   ,@ActionTime
			   ,@ActionTime
			   ,@ActionTime)
	END
	   
END