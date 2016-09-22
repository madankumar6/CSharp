CREATE PROCEDURE [dbo].[Master_GetDeviceAlertReceivers](
	@DeviceAlertId INT
)
AS
BEGIN

	SELECT * FROM DeviceAlertReceivers
	WHERE
	DeviceAlertId = @DeviceAlertId

END