CREATE PROCEDURE [dbo].[A_GetFenceList]
(
	@DeviceAlertId INT = NULL
)
AS
BEGIN

	SELECT
		DeviceAlertId,
		Latitude,
		Longitude,
		Distance,
		ListOrder
	FROM
		DeviceAlertFenceData
	WHERE
		DeviceAlertId = @DeviceAlertId
	ORDER BY DeviceAlertId, ListOrder
	
END