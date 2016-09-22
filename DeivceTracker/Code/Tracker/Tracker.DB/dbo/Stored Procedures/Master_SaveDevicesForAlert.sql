CREATE PROCEDURE [dbo].[Master_SaveDevicesForAlert]
(
	@DeviceAlertId INT,
	@DeviceId NVARCHAR(50),
	@IsToDelete BIT
)
AS
BEGIN

	IF(@IsToDelete = 1)
	BEGIN
		DELETE FROM AlertDevices
		WHERE DeviceId = @DeviceId AND DeviceAlertId = @DeviceAlertId
	END
	ELSE
	BEGIN
		IF NOT EXISTS(SELECT TOP 1 1 FROM AlertDevices WHERE DeviceId = @DeviceId AND DeviceAlertId = @DeviceAlertId)
		BEGIN
			INSERT INTO AlertDevices(DeviceId, DeviceAlertId)
			VALUES(@DeviceId, @DeviceAlertId)
		END
	END

END