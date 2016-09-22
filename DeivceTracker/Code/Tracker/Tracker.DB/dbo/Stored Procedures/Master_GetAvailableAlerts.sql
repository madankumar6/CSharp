CREATE PROCEDURE [dbo].[Master_GetAvailableAlerts](
	@AlertType int = NULL
)
AS
BEGIN

	SELECT * FROM [DeviceAlerts]
	WHERE
	--DeviceId = @DeviceId
	--AND
	AlertType = coalesce(@AlertType, AlertType)

END