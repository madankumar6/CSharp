CREATE PROCEDURE [dbo].[Master_GetDeviceAlert](
	@DeviceAlertId INT = NULL,
	@AlertType int = NULL
)
AS
BEGIN

	SELECT * FROM [DeviceAlerts]
	WHERE
	Id = COALESCE(@DeviceAlertId, Id)
	AND
	AlertType = COALESCE(@AlertType, AlertType)

END