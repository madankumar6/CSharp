CREATE PROCEDURE [dbo].[Master_GetDevicesForAlert]
(
	@DeviceAlertId INT
)
AS
BEGIN
	SELECT 
		DeviceId,
		DeviceAlertId
	FROM
		AlertDevices
	WHERE
		DeviceAlertId = @DeviceAlertId	
END