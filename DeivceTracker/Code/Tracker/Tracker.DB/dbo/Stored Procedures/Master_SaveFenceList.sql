CREATE PROCEDURE [dbo].[Master_SaveFenceList]
(
	@DeviceAlertId INT,
	@IsToDelete BIT = NULL,
	@DeviceFenceList CustomDataType READONLY
)
AS
BEGIN

	DELETE FROM DeviceAlertFenceData WHERE DeviceAlertId = @DeviceAlertId;
	
	IF(@IsToDelete <> 1)
	BEGIN
		INSERT INTO DeviceAlertFenceData(DeviceAlertId, Latitude, Longitude, Distance, ListOrder)
		SELECT @DeviceAlertId, Col2, Col3, 0, Col5
		FROM
			@DeviceFenceList
	END
END