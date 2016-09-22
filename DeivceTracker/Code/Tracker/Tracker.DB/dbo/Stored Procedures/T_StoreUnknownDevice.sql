CREATE PROCEDURE [dbo].[T_StoreUnknownDevice]
(
	@DeviceId NVARCHAR(50) = NULL
)
AS
BEGIN

	DECLARE @ActionTime DATETIME;
	SET @ActionTime = GETUTCDATE();
	
	IF EXISTS(SELECT 1 FROM UnknownDeviceData WHERE DeviceId = @DeviceId)
	BEGIN
		UPDATE UnknownDeviceData
		SET
		  [ActionTime] = @ActionTime
		WHERE
			DeviceId = @DeviceId
	END
	ELSE
	BEGIN
		INSERT INTO UnknownDeviceData(
				DeviceId
			   ,ActionTime
			   )
		 VALUES
			   (
			   @DeviceId
			   ,@ActionTime)
	END  	           
END